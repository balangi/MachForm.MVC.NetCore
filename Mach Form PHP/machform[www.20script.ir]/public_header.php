<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>MachForm Panel</title>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<meta name="robots" content="index, nofollow" />
<link rel="stylesheet" type="text/css" href="css/main.css<?php echo $mf_version_tag; ?>" media="screen" />   
<link rel="stylesheet" type="text/css" href="css/bb_buttons.css<?php echo $mf_version_tag; ?>" media="screen" /> 

<?php if(!empty($header_data)){ echo $header_data; } ?>
<link rel="stylesheet" type="text/css" href="css/override.css<?php echo $mf_version_tag; ?>" media="screen" /> 

<?php
	$active_admin_theme = $mf_settings['admin_theme'];
	if(!empty($_SESSION['mf_user_admin_theme'])){
		$active_admin_theme = $_SESSION['mf_user_admin_theme'];
	}

	$current_nav_tab = $current_nav_tab ?? 'manage_forms';
	
	if(!empty($active_admin_theme)){
		if($current_nav_tab == 'edit_theme' && $active_admin_theme == 'dark'){
			//speficially for dark theme on the theme editor, don't use the full dark theme CSS file
			//since it will overwrite the forms styling as well
			echo '<link href="css/themes/theme_dark_edit_theme.css'.$mf_version_tag.'" rel="stylesheet" type="text/css" />';
		}else{
			echo '<link href="css/themes/theme_'.$active_admin_theme.'.css'.$mf_version_tag.'" rel="stylesheet" type="text/css" />';
		}
	}else{
		echo '<link href="css/themes/theme_vibrant.css'.$mf_version_tag.'" rel="stylesheet" type="text/css" />';
	}
?>

</head>

<body>

<div id="bg">
<div id="header">


	<div id="header_content">
			<div id="logo">
				<span class="logo_helper"></span>
				<a href="manage_forms.php"><img src="<?php echo $machform_logo_main; ?>" <?php echo $logo_width_attr; ?> /></a>
			</div>	
			
			<div id="header_primary">
				
			</div>

			<div id="header_secondary">





			</div>
	</div>
</div><!-- /#header -->
<div id="container">
	<div id="main">
	