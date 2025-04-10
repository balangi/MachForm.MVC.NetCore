<?php
/********************************************************************************
 MachForm
  
 Copyright 2007-2016 Appnitro Software. This code cannot be redistributed without
 permission from http://www.appnitro.com/
 
 More info at: http://www.appnitro.com/
 ********************************************************************************/
	require('includes/init.php');
	
	ob_start();
	
	require('config.php');
	require('includes/db-core.php');
	require('includes/helper-functions.php');
	require('includes/filter-functions.php');
	
	$dbh = mf_connect_db();
	$mf_settings = mf_get_settings($dbh);
	
	$upload_success = false;
	
	$machform_data_path = '';

	//the default is not to store file upload as blob, unless defined otherwise within config.php file
	defined('MF_STORE_FILES_AS_BLOB') or define('MF_STORE_FILES_AS_BLOB',false);

	if(!empty($_FILES) && !empty($_POST['form_id']) && !empty($_POST['element_id']) && !empty($_POST['file_token'])){
		
		$form_id 	= (int) $_POST['form_id'];
		$element_id = (int) $_POST['element_id'];

		$file_token = trim($_POST['file_token']);
		$file_token = preg_replace('/[^\da-z]/i', '', $file_token); //ensure only alphanumeric characters allowed

		if(MF_STORE_FILES_AS_BLOB === true){
			//check if ap_form_xxx_listfiles table exist or not, if not exist create one
			$is_listfiles_table_exist = true;
			try{
				$params = array();
				
				$query = "select count(*) from ".MF_TABLE_PREFIX."form_{$form_id}_listfiles";
				$sth = $dbh->prepare($query);

				$sth->execute($params);
			}catch(PDOException $e) {
				$is_listfiles_table_exist = false;
			}

			if($is_listfiles_table_exist === false){
				
				//create table ap_form_xxx_listfiles
				try{
					$params = array();
					$query = "CREATE TABLE `".MF_TABLE_PREFIX."form_{$form_id}_listfiles` (
									  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
									  `date_created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
									  `file_token` text,
									  `file_content` text CHARACTER SET utf8,
									  PRIMARY KEY (`id`),
									  KEY `file_token` (`file_token`(50))
									) DEFAULT CHARACTER SET utf8;";
					$sth = $dbh->prepare($query);

					$sth->execute($params);
				}catch(PDOException $e) {
					//discard silently, in case the table already exi
				}
			}
		}
		
		if(!is_writable($machform_data_path.$mf_settings['upload_dir']."/form_{$form_id}/files")){
			$old_mask = umask(0);
			
			mkdir($machform_data_path.$mf_settings['data_dir']."/form_{$form_id}",0755);
			mkdir($machform_data_path.$mf_settings['data_dir']."/form_{$form_id}/css",0755);
			if($mf_settings['data_dir'] != $mf_settings['upload_dir']){
				@mkdir($machform_data_path.$mf_settings['upload_dir']."/form_{$form_id}",0755);
			}
			mkdir($machform_data_path.$mf_settings['upload_dir']."/form_{$form_id}/files",0755);
			@file_put_contents($machform_data_path.$mf_settings['upload_dir']."/form_{$form_id}/files/index.html",' '); //write empty index.html

			umask($old_mask);
		}
		
		
		//check if this is a multi upload or not
		//if not multi upload, we need to overwrite any previous entry, which can be on the review table or list file
		$query = "select 
						element_file_enable_multi_upload,
       					element_file_type_list,
       					element_file_enable_size_limit,
       					element_file_size_max
					from 
						".MF_TABLE_PREFIX."form_elements 
				   where 
				   		form_id = ? and element_id = ? and element_status = 1";
		$params = array($form_id,$element_id);
		$sth = mf_do_query($query,$params,$dbh);
		$row = mf_do_fetch_result($sth);
		
		if(!empty($row['element_file_enable_multi_upload'])){
			$is_multi_upload = true;
		}else{
			$is_multi_upload = false;
		}
		
		$file_type_list 		= trim($row['element_file_type_list']);
		$file_enable_size_limit	= $row['element_file_enable_size_limit'];
		$file_size_max			= $row['element_file_size_max'];

		//validate file type
		$ext = pathinfo(strtolower($_FILES['Filedata']['name']), PATHINFO_EXTENSION);
		$ext = preg_replace( '/[^a-z0-9]/i', '', $ext); //make sure the extension only contain alphanumeric

		//if ext is empty, the file extension most likely malicious, reject the upload
		if(empty($ext)){
			die('Error! Filetype unknown!');
		}

		if(!empty($file_type_list)){
		
			$file_type_array = explode(',',$file_type_list);
			$file_type_array = array_map('strtolower', $file_type_array);
			$file_type_array = array_map('trim', $file_type_array);
			
			if(!in_array($ext,$file_type_array)){
				die('Error! Filetype not allowed!');
			}
		}else{
			//if file_type_list is empty, block all file upload
			die('Unable to upload. This field doesn\'t accept any file types');
		}

		//validate for double extension attack
		$exploded = explode('.',$_FILES['Filedata']['name']);
		if(count($exploded) > 2){
			die('Error! File with double extension not allowed!');
		}
		
		//validate file size if this rule being enabled
		if(!empty($file_enable_size_limit) && !empty($file_size_max)){
			$file_size_max = $file_size_max * 1048576; //turn into bytes from MB
			if($_FILES['Filedata']['size'] > $file_size_max){
				die('Error! File size exceeded!');
			}
		}
		
		
		//move file and check for invalid file
		$destination_file = $machform_data_path.$mf_settings['upload_dir']."/form_{$form_id}/files/element_{$element_id}_{$file_token}-{$_FILES['Filedata']['name']}.tmp";
		
		//if destination file already exist having the exact file name (could be happen if user is uploading multiple files using same names)
		if(file_exists($destination_file)){
			//add random numbers to the filename
			$rand_suffix = substr(strtoupper(md5(mt_rand())),0,5);
			$path_parts = pathinfo($_FILES['Filedata']['name']);

			$destination_file = $machform_data_path.$mf_settings['upload_dir']."/form_{$form_id}/files/element_{$element_id}_{$file_token}-{$path_parts['filename']}{$rand_suffix}.{$path_parts['extension']}.tmp";
		}

		$destination_file = mf_sanitize($destination_file);
		$destination_file = str_replace(array('<','>'), '', $destination_file); //prevent nasty php code tag being written into the file

		$source_file	  = $_FILES['Filedata']['tmp_name'];
		if(move_uploaded_file($source_file,$destination_file)){
			
			//fix image orientation issue for JPEG file, if any
			fix_image_orientation($destination_file);

			//add the file name into the list file
			$listfile_name = $machform_data_path.$mf_settings['upload_dir']."/form_{$form_id}/files/listfile_{$file_token}.txt";
			
			if(!file_exists($listfile_name)){ //if the listfile is not being created yet
				$listfile_content = '<?php'."\n".$destination_file."\n"."?>";
			}else{
				
				if($is_multi_upload){
					//insert the new file into the listfile, we need to make sure there is no duplicate
					$current_listfile_array = array();
					$current_listfile_array = file($listfile_name, FILE_IGNORE_NEW_LINES | FILE_SKIP_EMPTY_LINES);
					
					array_shift($current_listfile_array); //remove the first index of the array
					array_pop($current_listfile_array); //remove the last index of the array
					array_push($current_listfile_array,$destination_file); //push the new filename
					
					$current_listfile_array = array_unique($current_listfile_array); //make sure there are only uniques files
					
					array_unshift($current_listfile_array,"<?php");
					array_push($current_listfile_array,"?>");
					
					$listfile_content = implode("\n",$current_listfile_array);
				}else{
					
					//delete previous file from the listfile if any
					$current_listfile_array = array();
					$current_listfile_array = file($listfile_name, FILE_IGNORE_NEW_LINES | FILE_SKIP_EMPTY_LINES);
					
					if(file_exists($current_listfile_array[1])){
						unlink($current_listfile_array[1]);
					}
					
					$listfile_content = '<?php'."\n".$destination_file."\n"."?>";
				}
			}
			
			// Write the contents to the file
			file_put_contents($listfile_name, $listfile_content, LOCK_EX);

			if(MF_STORE_FILES_AS_BLOB === true){
				//save a copy of the uploaded file into the database
				mf_ap_form_files_insert($dbh,$form_id,$destination_file);

				//save the filename into listfiles table
				$query = "INSERT INTO `".MF_TABLE_PREFIX."form_{$form_id}_listfiles`(file_content,file_token) VALUES(?,?);";
				$params = array($destination_file,$file_token);
				mf_do_query($query,$params,$dbh);
			}
			
			$upload_success = true;
		}else{
			$upload_success = false;
			$error_message  = "Unable to move file!";
		}
		
	}
	
	$response_data = new stdClass();
	
	if($upload_success){
		$response_data->status    	= "ok";
		//we do base64_encode to prevent any weird characters from breaking the javascript
		$response_data->message 	= base64_encode(mf_sanitize($_FILES['Filedata']['name']));
	}else{
		$response_data->status    	= "error";
		$response_data->message 	= $error_message;
	}
	
	$response_json = json_encode($response_data);
	
	echo $response_json;
	
	//we need to use output buffering to be able capturing error messages
	$output = ob_get_contents();
	ob_end_clean();
	
	echo $output;


/* Functions */
function fix_image_orientation($filename) {
	if(function_exists('mime_content_type') && function_exists('exif_read_data')){
	 	if(mime_content_type($filename) == 'image/jpeg'){
		    $exif = @exif_read_data($filename);
		    if($exif && isset($exif['Orientation'])) {
		      $orientation = $exif['Orientation'];
		      
		      if($orientation != 1){
		        $img = imagecreatefromjpeg($filename);
		        $deg = 0;
		        
		        switch ($orientation) {
		          case 3: $deg = 180; break;
		          case 6: $deg = 270; break;
		          case 8: $deg = 90; break;
		        }
		        
		        if ($deg) {
		          $img = imagerotate($img, $deg, 0);        
		        }
		        
		        //rewrite the rotated image back to the disk as $filename 
		        imagejpeg($img, $filename, 95);
		      } 
		    } 
		}
  	}    
}
?>