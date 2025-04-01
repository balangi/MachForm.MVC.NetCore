using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MachForm.NetCore.Helpers;

public class UtilHelper
{
    public static string MfGetSslSuffix()
    {
        if (HttpContext.Current.Request.IsSecureConnection ||
            string.Equals(HttpContext.Current.Request.Headers["X-Forwarded-Proto"], "https", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(HttpContext.Current.Request.Headers["Front-End-Https"], "on", StringComparison.OrdinalIgnoreCase))
        {
            return "s";
        }
        return "";
    }

    public static string MfGetDirname(string path)
    {
        string currentDir = System.IO.Path.GetDirectoryName(path);
        if (currentDir == "/" || currentDir == "\\")
        {
            currentDir = "";
        }
        return currentDir;
    }

    public static Dictionary<string, string> MfGetSettings()
    {
        // پیاده‌سازی دریافت تنظیمات از دیتابیس
        return new Dictionary<string, string>();
    }

    public static string MfFormatBytes(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        else if (bytes < 1048576) return $"{Math.Round(bytes / 1024.0, 2)} KB";
        else if (bytes < 1073741824) return $"{Math.Round(bytes / 1048576.0, 2)} MB";
        else if (bytes < 1099511627776) return $"{Math.Round(bytes / 1073741824.0, 2)} GB";
        else return $"{Math.Round(bytes / 1099511627776.0, 2)} TB";
    }

    public static void MfTrimValue(ref string value)
    {
        value = value?.Trim();
    }

    public static void MfStrToLowerValue(ref string value)
    {
        value = value?.ToLower();
    }

    public static string MfGetLogicJavascript(int formId, int pageNumber)
    {
        //        $form_id = (int) $form_id;

        //		//get the target elements for the current page
        //		$query = "SELECT 

        //                    A.element_id,
        //					A.rule_show_hide,
        //					A.rule_all_any,
        //					B.element_title,
        //					B.element_page_number
        //                FROM

        //                    ".MF_TABLE_PREFIX."field_logic_elements A LEFT JOIN ".MF_TABLE_PREFIX."form_elements B

        //                  ON

        //                      A.form_id = B.form_id and A.element_id = B.element_id and B.element_status = 1

        //               WHERE

        //                    A.form_id = ? and B.element_page_number = ?
        //            ORDER BY

        //                    B.element_position asc";
        //		$params = array($form_id,$page_number);
        //		$sth = mf_do_query($query,$params,$dbh);

        //		$logic_elements_array = array();

        //        while ($row = mf_do_fetch_result($sth)){
        //			$element_id = (int) $row['element_id'];

        //			$logic_elements_array[$element_id]['element_title'] = $row['element_title'];
        //			$logic_elements_array[$element_id]['rule_show_hide'] = $row['rule_show_hide'];
        //			$logic_elements_array[$element_id]['rule_all_any'] = $row['rule_all_any'];
        //        }

        //		//get the conditions array
        //		$query = "SELECT 

        //                        A.target_element_id,
        //						A.element_name,
        //						A.rule_condition,
        //						A.rule_keyword,
        //						(select

        //                               B.element_page_number
        //                           from

        //                                  ".MF_TABLE_PREFIX."form_elements B

        //                          where
        //                                  form_id = A.form_id and
        //                                  element_id = trim(leading 'element_' from substring_index(A.element_name, '_', 2))
        //						) condition_element_page_number,
        //						(select

        //                               C.element_type
        //                           from

        //                                  ".MF_TABLE_PREFIX."form_elements C

        //                          where
        //                                  form_id = A.form_id and
        //                                  element_id = trim(leading 'element_' from substring_index(A.element_name, '_', 2))
        //						) condition_element_type
        //                    FROM

        //                        ".MF_TABLE_PREFIX."field_logic_conditions A

        //                   WHERE

        //                        A.form_id = ?
        //                   ORDER by A.alc_id";
        //		$params = array($form_id);
        //		$sth = mf_do_query($query,$params,$dbh);

        //		$logic_conditions_array = array();
        //		$prev_element_id = 0;

        //		$i = 0;
        //        while ($row = mf_do_fetch_result($sth)){
        //			$target_element_id = (int) $row['target_element_id'];

        //            if ($target_element_id != $prev_element_id){
        //				$i = 0;
        //            }

        //			$logic_conditions_array[$target_element_id][$i]['element_name'] = $row['element_name'];
        //			$logic_conditions_array[$target_element_id][$i]['element_type'] = $row['condition_element_type'];
        //			$logic_conditions_array[$target_element_id][$i]['element_page_number'] = (int) $row['condition_element_page_number'];
        //			$logic_conditions_array[$target_element_id][$i]['rule_condition'] = $row['rule_condition'];
        //			$logic_conditions_array[$target_element_id][$i]['rule_keyword'] = $row['rule_keyword'];


        //			$prev_element_id = $target_element_id;
        //			$i++;
        //        }

        //		//build mf_handler_xx() function for each element
        //		$mf_handler_code = '';
        //		$mf_bind_code = '';
        //		$mf_initialize_code = '';

        //        foreach ($logic_elements_array as $element_id => $value) {
        //			$rule_show_hide = $value['rule_show_hide'];
        //			$rule_all_any = $value['rule_all_any'];

        //			$current_handler_conditions_array = array();

        //			$mf_handler_code.= "\n"."function mf_handler_{$element_id}(){"."\n"; //start mf_handler_xx
        //			/************************************************************************************/

        //			$target_element_conditions = $logic_conditions_array[$element_id];

        //			$unique_field_suffix_id = 0;

        //            //initialize the condition status for any other elements which is not within this page
        //            //store the status into a variable
        //            foreach ($target_element_conditions as $value) {
        //                if ($value['element_page_number'] == $page_number){
        //                    continue;
        //                }

        //				$unique_field_suffix_id++;

        //				$current_handler_conditions_array[] = 'condition_'.$value['element_name'].'_'.$unique_field_suffix_id;

        //				$condition_params = array();
        //				$condition_params['form_id'] = $form_id;
        //				$condition_params['element_name'] = $value['element_name'];
        //				$condition_params['rule_condition'] = $value['rule_condition'];
        //				$condition_params['rule_keyword'] = $value['rule_keyword'];

        //				$condition_status = mf_get_condition_status_from_table($dbh,$condition_params);

        //                if ($condition_status === true){
        //					$condition_status_value = 'true';
        //                }else
        //                {
        //					$condition_status_value = 'false';
        //                }

        //				$mf_handler_code.= "\t"."var condition_{$value['element_name']}_{$unique_field_suffix_id} = {$condition_status_value};"."\n";
        //            }

        //			$mf_handler_code.= "\n";

        //            //build the conditions for the current element

        //            foreach ($target_element_conditions as $value) {
        //				$unique_field_suffix_id++;

        //                //skip field which doesn't belong current page
        //                if ($value['element_page_number'] != $page_number){
        //                    continue;
        //                }

        //                //for checkbox with other, we need to replace the id
        //                if ($value['element_type'] == 'checkbox'){
        //					$checkbox_has_other = false;

        //                    if (substr($value['element_name'], -5) == 'other')
        //                    {
        //						$value['element_name'] = str_replace('_other', '_0', $value['element_name']);
        //						$checkbox_has_other = true;
        //                    }
        //                }



        //				$condition_params = array();
        //				$condition_params['form_id'] = $form_id;
        //				$condition_params['element_name'] = $value['element_name'];
        //				$condition_params['rule_condition'] = $value['rule_condition'];
        //				$condition_params['rule_keyword'] = $value['rule_keyword'];


        //				//we need to add unique suffix into the element name
        //				//so that we can use the same field multiple times to build a rule
        //				$condition_params['element_name'] = $value['element_name'].'_'.$unique_field_suffix_id;
        //				$current_handler_conditions_array[] = 'condition_'.$value['element_name'].'_'.$unique_field_suffix_id;


        //				$mf_handler_code.= mf_get_condition_javascript($dbh,$condition_params);

        //                //build the bind code
        //                if ($value['element_type'] == 'radio' || $value['element_type'] == 'rating'){
        //					$mf_bind_code.= "\$('input[name={$value['element_name']}]').on('change click', function() {\n";
        //                }else if ($value['element_type'] == 'time'){
        //					$mf_bind_code.= "\$('#{$value['element_name']}_1,#{$value['element_name']}_2,#{$value['element_name']}_3,#{$value['element_name']}_4').on('keyup mouseout change click', function() {\n";
        //                }else if ($value['element_type'] == 'money'){
        //					$mf_bind_code.= "\$('#{$value['element_name']},#{$value['element_name']}_1,#{$value['element_name']}_2').on('keyup mouseout change click', function() {\n";
        //                }else if ($value['element_type'] == 'matrix'){

        //					$exploded = array();
        //					$exploded = explode('_',$value['element_name']);

        //					$matrix_element_id = (int) $exploded[1];
        //					//we only need to bind the event to the parent element_id of the matrix
        //					$query = "select element_matrix_parent_id from ".MF_TABLE_PREFIX."form_elements where element_id = ? and form_id = ? and element_status = 1";

        //					$params = array($matrix_element_id,$form_id);
        //					$sth = mf_do_query($query,$params,$dbh);
        //					$row = mf_do_fetch_result($sth);
        //                    if (!empty($row['element_matrix_parent_id']))
        //                    {
        //						$matrix_element_id = $row['element_matrix_parent_id'];
        //                    }


        //					$mf_bind_code.= "\$('#li_{$matrix_element_id} :input').on('change click', function() {\n";
        //                }else if ($value['element_type'] == 'checkbox'){
        //                    if ($checkbox_has_other){
        //						$exploded = array();
        //						$exploded = explode('_', $value['element_name']);

        //						$mf_bind_code.= "\$('#{$value['element_name']},#element_{$exploded[1]}_other').on('keyup mouseout change click', function() {\n";
        //                    }else
        //                    {
        //						$mf_bind_code.= "\$('#{$value['element_name']}').on('keyup mouseout change click', function() {\n";
        //                    }
        //                }else if ($value['element_type'] == 'select'){
        //					$mf_bind_code.= "\$('#{$value['element_name']}').on('keyup change', function() {\n";
        //                }else if ($value['element_type'] == 'date' || $value['element_type'] == 'europe_date'){
        //					$mf_bind_code.= "\$('#{$value['element_name']}_1,#{$value['element_name']}_2,#{$value['element_name']}_3').on('keyup mouseout change click', function() {\n";
        //                }else if ($value['element_type'] == 'phone'){
        //					$mf_bind_code.= "\$('#{$value['element_name']}_1,#{$value['element_name']}_2,#{$value['element_name']}_3').on('keyup mouseout change click', function() {\n";
        //                }else
        //                {
        //					$mf_bind_code.= "\$('#{$value['element_name']}').on('keyup mouseout change click', function() {\n";
        //                }

        //				$mf_bind_code.= "\tmf_handler_{$element_id}();\n";
        //				$mf_bind_code .= "});\n";
        //			}	

        //			//evaluate all conditions
        //			if($rule_all_any == 'all'){
        //				$logic_operator = ' && ';
        //			}else{
        //				$logic_operator = ' || ';
        //			}

        //			if($rule_show_hide == 'show'){
        //				$action_code_primary 	= "\$('#li_{$element_id}').show();";
        //				$action_code_secondary  = "\$('#li_{$element_id}').hide();";
        //			}else if($rule_show_hide == 'hide'){
        //				$action_code_primary 	= "\$('#li_{$element_id}').hide();";
        //				$action_code_secondary  = "\$('#li_{$element_id}').show();";
        //			}

        //			$current_handler_conditions_joined = implode($logic_operator, $current_handler_conditions_array);
        //			$mf_handler_code .= "\tif({$current_handler_conditions_joined}){\n";
        //			$mf_handler_code .= "\t\t{$action_code_primary}\n";
        //			$mf_handler_code .= "\t}else{\n";
        //			$mf_handler_code .= "\t\t{$action_code_secondary}\n";
        //			$mf_handler_code .= "\t}\n\n";

        //			//if payment enabled, make sure to calculate totals
        //			$mf_handler_code .= "\tif($(\".total_payment\").length > 0){\n";
        //			$mf_handler_code .= "\t\tcalculate_total_payment();\n";
        //			$mf_handler_code .=	"\t}\n";

        //			//postMessage to adjust the height of the iframe
        //			$mf_handler_code .= "\tif($(\"html\").hasClass(\"embed\")){\n";
        //			$mf_handler_code .= "\t\t$.postMessage({mf_iframe_height: $('body').outerHeight(true)}, '*', parent );\n";
        //			$mf_handler_code .=	"\t}\n";

        //			/************************************************************************************/
        //			$mf_handler_code .= "}"."\n"; //end mf_handler_xx

        //			$mf_initialize_code .= "mf_handler_{$element_id}();\n";
        //		}


        //		$javascript_code = <<<EOT
        //<script type="text/javascript">
        //$(function(){

        //{$mf_handler_code}
        //{$mf_bind_code}
        //{$mf_initialize_code}

        //});
        //</script>
        //EOT;

        //		//if no handler, bind or initialize code, discard the javascript code
        //		if(empty($mf_handler_code) && empty($mf_bind_code) && empty($mf_initialize_code)){
        //			$javascript_code = '';
        //		}

        //		return $javascript_code;


        // پیاده‌سازی منطق شرطی
        // این بخش نیاز به پیاده‌سازی دقیق‌تر دارد
        return @"<script type='text/javascript'>
            $(function(){
                // کد JavaScript تولید شده
            });
            </script>";
    }

    public static bool MfGetConditionStatusFromTable(int formId, Dictionary<string, string> conditionParams)
    {
        //$form_id = (int) $condition_params['form_id'];
        //		$element_name = $condition_params['element_name']; //this could be 'element_x' or 'element_x_x' or 'approval_status'
        //		$rule_condition = $condition_params['rule_condition'];
        //		$rule_keyword = addslashes(strtolower(htmlspecialchars_decode($condition_params['rule_keyword']))); //keyword is case insensitive

        //		$session_id = session_id();

        //        if ($condition_params['use_main_table'] === true){
        //			$table_suffix = '';
        //			$record_name = 'id';
        //			$record_id = $condition_params['entry_id'];
        //        }else
        //        {
        //			$table_suffix = '_review';
        //			$record_name = 'session_id';
        //			$record_id = $session_id;
        //        }

        //		$condition_status = false; //the default status if false

        //		$exploded = explode('_', $element_name);
        //		$element_id = (int) $exploded[1];

        //		//get the element properties of the current element id
        //		$query = "select 

        //                         element_type,
        //						 element_choice_has_other,
        //						 element_time_showsecond,
        //						 element_time_24hour,
        //						 element_matrix_parent_id,
        //						 element_matrix_allow_multiselect
        //                     from

        //                          ".MF_TABLE_PREFIX."form_elements
        //                    where

        //                         form_id = ? and element_id = ? ";
        //		$params = array($form_id,$element_id);
        //		$sth = mf_do_query($query,$params,$dbh);
        //		$row = mf_do_fetch_result($sth);

        //		$element_type = $row['element_type'];
        //		$element_choice_has_other = $row['element_choice_has_other'];
        //		$element_time_showsecond = (int) $row['element_time_showsecond'];
        //		$element_time_24hour = (int) $row['element_time_24hour'];
        //		$element_matrix_parent_id = (int) $row['element_matrix_parent_id'];
        //		$element_matrix_allow_multiselect = (int) $row['element_matrix_allow_multiselect'];

        //        //approval status doesn't have element type from the ap_form_elements table
        //        if ($element_name == 'approval_status'){
        //			$element_type = 'approval_status';
        //        }

        //        //special element name, doesn't have element type from ap_form_elements table
        //        if ($element_name == 'form_is_submitted'){
        //			$element_type = 'form_is_submitted';
        //        }

        //        //if this is matrix field, we need to determine wether this is matrix choice or matrix checkboxes
        //        if ($element_type == 'matrix'){
        //            if (empty($element_matrix_parent_id))
        //            {
        //                if (!empty($element_matrix_allow_multiselect))
        //                {
        //					$element_type = 'matrix_checkbox';
        //                }
        //                else
        //                {
        //					$element_type = 'matrix_radio';
        //                }
        //            }
        //            else
        //            {
        //				//this is a child row of a matrix, get the parent id first and check the status of the multiselect option
        //				$query = "select element_matrix_allow_multiselect from ".MF_TABLE_PREFIX."form_elements where form_id = ? and element_id = ?";
        //				$params = array($form_id,$element_matrix_parent_id);
        //				$sth = mf_do_query($query,$params,$dbh);
        //				$row = mf_do_fetch_result($sth);

        //                if (!empty($row['element_matrix_allow_multiselect']))
        //                {
        //					$element_type = 'matrix_checkbox';
        //                }
        //                else
        //                {
        //					$element_type = 'matrix_radio';
        //                }
        //            }
        //        }

        //        if (in_array($element_type, array('text', 'textarea', 'simple_name', 'name', 'simple_name_wmiddle', 'name_wmiddle', 'address', 'phone', 'simple_phone', 'email', 'url')))
        //        {

        //            if ($rule_condition == 'is'){
        //				$where_operand = '=';
        //				$where_keyword = "'{$rule_keyword}'";
        //            }else if ($rule_condition == 'is_not'){
        //				$where_operand = '<>';
        //				$where_keyword = "'{$rule_keyword}'";
        //            }else if ($rule_condition == 'begins_with'){
        //				$where_operand = 'LIKE';
        //				$where_keyword = "'{$rule_keyword}%'";
        //            }else if ($rule_condition == 'ends_with'){
        //				$where_operand = 'LIKE';
        //				$where_keyword = "'%{$rule_keyword}'";
        //            }else if ($rule_condition == 'contains'){
        //				$where_operand = 'LIKE';
        //				$where_keyword = "'%{$rule_keyword}%'";
        //            }else if ($rule_condition == 'not_contain'){
        //				$where_operand = 'NOT LIKE';
        //				$where_keyword = "'%{$rule_keyword}%'";
        //            }

        //			//get the entered value on the table
        //			$query = "select 

        //                            count(`id`) total_row
        //                        from

        //                            ".MF_TABLE_PREFIX."form_{$form_id}
        //            {$table_suffix}
        //            where
        //                    ifnull(`{$element_name}`,'') {$where_operand}
        //            {$where_keyword}
        //            and `{$record_name}` = ? ";
        //			$params = array($record_id);
        //			$sth = mf_do_query($query,$params,$dbh);
        //			$row = mf_do_fetch_result($sth);

        //            if (!empty($row['total_row']))
        //            {
        //				$condition_status = true;
        //            }
        //        }
        //        else if ($element_type == 'radio' || $element_type == 'select'){

        //            if ($rule_condition == 'is'){
        //				$where_operand = '=';
        //				$where_keyword = "'{$rule_keyword}'";
        //            }else if ($rule_condition == 'is_not'){
        //				$where_operand = '<>';
        //				$where_keyword = "'{$rule_keyword}'";
        //            }else if ($rule_condition == 'begins_with'){
        //				$where_operand = 'LIKE';
        //				$where_keyword = "'{$rule_keyword}%'";
        //            }else if ($rule_condition == 'ends_with'){
        //				$where_operand = 'LIKE';
        //				$where_keyword = "'%{$rule_keyword}'";
        //            }else if ($rule_condition == 'contains'){
        //				$where_operand = 'LIKE';
        //				$where_keyword = "'%{$rule_keyword}%'";
        //            }else if ($rule_condition == 'not_contain'){
        //				$where_operand = 'NOT LIKE';
        //				$where_keyword = "'%{$rule_keyword}%'";
        //            }

        //			//get the entered value on the table
        //			$query = "SELECT 

        //                            count(B.element_title) total_row
        //                        FROM(
        //                             SELECT
        //                                   A.`{$element_name}`,
        //								   (select
        //								   		  `option` 
        //								   	  from

        //                                               ".MF_TABLE_PREFIX."element_options
        //                                        where

        //                                              form_id = ? and

        //                                              element_id = ? and

        //                                              option_id = A.element_{$element_id}
        //            and
        //            live = 1 LIMIT 1) element_title
        //                               FROM

        //                                     ".MF_TABLE_PREFIX."form_{$form_id}
        //            {$table_suffix}
        //            A
        //                              WHERE 
        //							  	   `{$record_name}` = ?
        //							) B
        //                       WHERE

        //                               B.element_title {$where_operand}
        //            {$where_keyword}
        //            ";

        //			$params = array($form_id,$element_id,$record_id);
        //			$sth = mf_do_query($query,$params,$dbh);
        //			$row = mf_do_fetch_result($sth);

        //            if (!empty($row['total_row']))
        //            {
        //				$condition_status = true;
        //            }

        //            //if the choice field has 'other' and the condition is still false, we need to check into the 'other' field
        //            if (!empty($element_choice_has_other) && $condition_status === false){
        //				$query = "SELECT 

        //                            count(B.element_title) total_row
        //                        FROM(
        //                             SELECT
        //                                   A.`element_{$element_id}`,
        //								   (select element_choice_other_label from ".MF_TABLE_PREFIX."form_elements where form_id = ? and element_id = ?) element_title
        //                               FROM

        //                                   ".MF_TABLE_PREFIX."form_{$form_id}
        //                {$table_suffix}
        //                A
        //                              WHERE

        //                                   A.`{$record_name}` = ? and

        //                                   A.element_{$element_id} = 0 and

        //                                   A.element_{$element_id}
        //                _other is not null and
        //                                   A.element_ {$element_id } _other <> ''
        //							) B
        //                       WHERE

        //                               B.element_title {$where_operand}
        //                {$where_keyword}
        //                ";

        //				$params = array($form_id,$element_id,$record_id);
        //				$sth = mf_do_query($query,$params,$dbh);
        //				$row = mf_do_fetch_result($sth);

        //                if (!empty($row['total_row']))
        //                {
        //					$condition_status = true;
        //				}

        //			}
        //		}else if($element_type == 'time'){

        //			//there are few variants of the the time field, get the specific type
        //			if(!empty($element_time_showsecond) && !empty($element_time_24hour)){
        //				$element_type = 'time_showsecond24hour';
        //			}else if(!empty($element_time_showsecond) && empty($element_time_24hour)){
        //				$element_type = 'time_showsecond';
        //			}else if(empty($element_time_showsecond) && !empty($element_time_24hour)){
        //				$element_type = 'time_24hour';
        //			}

        //			$exploded = array();
        //			$exploded = explode(':', $rule_keyword); //rule keyword format -> HH:MM:SS:AM

        //			if($element_type == 'time'){
        //				$rule_keyword = date("H:i:s",strtotime("{$exploded[0]}:{$exploded[1]}:00 {$exploded[3]}"));
        //			}else if($element_type == 'time_showsecond'){
        //				$rule_keyword = date("H:i:s",strtotime("{$exploded[0]}:{$exploded[1]}:{$exploded[2]} {$exploded[3]}"));
        //			}else if($element_type == 'time_24hour'){
        //				$rule_keyword = date("H:i:s",strtotime("{$exploded[0]}:{$exploded[1]}:00"));
        //			}else if($element_type == 'time_showsecond24hour'){
        //				$rule_keyword = date("H:i:s",strtotime("{$exploded[0]}:{$exploded[1]}:{$exploded[2]}"));
        //			}

        //			if($rule_condition == 'is'){
        //				$where_operand = '=';
        //				$where_keyword = "'{$rule_keyword}'";
        //			}else if($rule_condition == 'is_before'){
        //				$where_operand = '<';
        //				$where_keyword = "'{$rule_keyword}'";
        //			}else if($rule_condition == 'is_after'){
        //				$where_operand = '>';
        //				$where_keyword = "'{$rule_keyword}'";
        //			}

        //			//get the entered value on the table
        //			$query = "select 
        //							count(`id`) total_row 
        //						from 
        //							".MF_TABLE_PREFIX."form_{$form_id}{$table_suffix} 
        //					   where 
        //					   		time(`{$element_name}`) {$where_operand} {$where_keyword} 
        //					   		and `{$record_name}` = ?";

        //			$params = array($record_id);
        //			$sth 	= mf_do_query($query,$params,$dbh);
        //			$row 	= mf_do_fetch_result($sth);

        //			if(!empty($row['total_row'])){
        //				$condition_status = true;
        //			}
        //		}else if($element_type == 'money' || $element_type == 'number'){

        //			$rule_keyword = (float) $rule_keyword;
        //			if($rule_condition == 'is'){
        //				$where_operand = '=';
        //				$where_keyword = "{$rule_keyword}";
        //			}else if($rule_condition == 'less_than'){
        //				$where_operand = '<';
        //				$where_keyword = "{$rule_keyword}";
        //			}else if($rule_condition == 'greater_than'){
        //				$where_operand = '>';
        //				$where_keyword = "{$rule_keyword}";
        //			}

        //			//get the entered value on the table
        //			$query = "select 
        //							count(`id`) total_row 
        //						from 
        //							".MF_TABLE_PREFIX."form_{$form_id}{$table_suffix} 
        //					   where 
        //					   		ifnull(`{$element_name}`,'') {$where_operand} {$where_keyword} 
        //					   		and `{$record_name}` = ?";

        //			$params = array($record_id);
        //			$sth 	= mf_do_query($query,$params,$dbh);
        //			$row 	= mf_do_fetch_result($sth);

        //			if(!empty($row['total_row'])){
        //				$condition_status = true;
        //			}
        //		}else if($element_type == 'rating'){

        //			$rule_keyword = (int) $rule_keyword;
        //			if($rule_condition == 'is'){
        //				$where_operand = '=';
        //				$where_keyword = "{$rule_keyword}";
        //			}else if($rule_condition == 'is_not'){
        //				$where_operand = '<>';
        //				$where_keyword = "{$rule_keyword}";
        //			}else if($rule_condition == 'less_than'){
        //				$where_operand = '<';
        //				$where_keyword = "{$rule_keyword}";
        //			}else if($rule_condition == 'greater_than'){
        //				$where_operand = '>';
        //				$where_keyword = "{$rule_keyword}";
        //			}

        //			//get the entered value on the table
        //			$query = "select 
        //							count(`id`) total_row 
        //						from 
        //							".MF_TABLE_PREFIX."form_{$form_id}{$table_suffix} 
        //					   where 
        //					   		ifnull(`{$element_name}`,'') {$where_operand} {$where_keyword} 
        //					   		and `{$record_name}` = ?";

        //			$params = array($record_id);
        //			$sth 	= mf_do_query($query,$params,$dbh);
        //			$row 	= mf_do_fetch_result($sth);

        //			if(!empty($row['total_row'])){
        //				$condition_status = true;
        //			}
        //		}else if($element_type == 'matrix_radio'){

        //			if($rule_condition == 'is'){
        //				$where_operand = '=';
        //				$where_keyword = "'{$rule_keyword}'";
        //			}else if($rule_condition == 'is_not'){
        //				$where_operand = '<>';
        //				$where_keyword = "'{$rule_keyword}'";
        //			}else if($rule_condition == 'begins_with'){
        //				$where_operand = 'LIKE';
        //				$where_keyword = "'{$rule_keyword}%'";
        //			}else if($rule_condition == 'ends_with'){
        //				$where_operand = 'LIKE';
        //				$where_keyword = "'%{$rule_keyword}'";
        //			}else if($rule_condition == 'contains'){
        //				$where_operand = 'LIKE';
        //				$where_keyword = "'%{$rule_keyword}%'";
        //			}else if($rule_condition == 'not_contain'){
        //				$where_operand = 'NOT LIKE';
        //				$where_keyword = "'%{$rule_keyword}%'";
        //			}

        //			//get the entered value on the table
        //			$query = "SELECT 
        //							count(B.element_title) total_row 
        //					    FROM(
        //							 SELECT 
        //								   A.`{$element_name}`,
        //								   (select 
        //								   		  `option` 
        //								   	  from 
        //								   	  	  ".MF_TABLE_PREFIX."element_options 
        //								   	 where 
        //								   	 	  form_id = ? and 
        //								   	 	  element_id = ? and 
        //								   	 	  option_id = A.element_{$element_id} and 
        //								   	 	  live = 1 LIMIT 1) element_title
        //							   FROM 
        //							  	   ".MF_TABLE_PREFIX."form_{$form_id}{$table_suffix} A
        //							  WHERE 
        //							  	   `{$record_name}` = ?
        //							) B 
        //					   WHERE 
        //					   		B.element_title {$where_operand} {$where_keyword}";

        //			if(!empty($element_matrix_parent_id)){
        //				$element_id = $element_matrix_parent_id;
        //			}

        //			$params = array($form_id,$element_id,$record_id);
        //			$sth 	= mf_do_query($query,$params,$dbh);
        //			$row 	= mf_do_fetch_result($sth);

        //			if(!empty($row['total_row'])){
        //				$condition_status = true;
        //			}
        //		}else if($element_type == 'matrix_checkbox' || $element_type == 'checkbox'){

        //			if($rule_condition == 'is_one'){
        //				$where_operand = '>';
        //				$where_keyword = "'0'";
        //			}else if($rule_condition == 'is_zero'){
        //				$where_operand = '=';
        //				$where_keyword = "'0'";
        //			}

        //			//get the entered value on the table
        //			$query = "select 
        //							count(`id`) total_row 
        //						from 
        //							".MF_TABLE_PREFIX."form_{$form_id}{$table_suffix} 
        //					   where 
        //					   		`{$element_name}` {$where_operand} {$where_keyword} 
        //					   		and `{$record_name}` = ?";

        //			$params = array($record_id);
        //			$sth 	= mf_do_query($query,$params,$dbh);
        //			$row 	= mf_do_fetch_result($sth);

        //			if(!empty($row['total_row'])){
        //				$condition_status = true;
        //			}
        //		}else if($element_type == 'date' || $element_type == 'europe_date'){

        //			$exploded = array();
        //			$exploded = explode('/', $rule_keyword); //rule keyword format -> mm/dd/yyyy

        //			$rule_keyword = $exploded[2].'-'.$exploded[0].'-'.$exploded[1]; //this should be yyyy-mm-dd

        //			if($rule_condition == 'is'){
        //				$where_operand = '=';
        //				$where_keyword = "'{$rule_keyword}'";
        //			}else if($rule_condition == 'is_before'){
        //				$where_operand = '<';
        //				$where_keyword = "'{$rule_keyword}'";
        //			}else if($rule_condition == 'is_after'){
        //				$where_operand = '>';
        //				$where_keyword = "'{$rule_keyword}'";
        //			}

        //			//get the entered value on the table
        //			$query = "select 
        //							count(`id`) total_row 
        //						from 
        //							".MF_TABLE_PREFIX."form_{$form_id}{$table_suffix} 
        //					   where 
        //					   		date(`{$element_name}`) {$where_operand} {$where_keyword} 
        //					   		and `{$record_name}` = ?";

        //			$params = array($record_id);
        //			$sth 	= mf_do_query($query,$params,$dbh);
        //			$row 	= mf_do_fetch_result($sth);

        //			if(!empty($row['total_row'])){
        //				$condition_status = true;
        //			}
        //		}else if($element_type == 'approval_status'){
        //			if($rule_condition == 'is_approved' && $condition_params['is_approved'] === true){
        //				$condition_status = true;
        //			}else if($rule_condition == 'is_denied' && $condition_params['is_denied'] === true){
        //				$condition_status = true;
        //			}
        //		}else if($element_type == 'form_is_submitted'){
        //			if($condition_params['form_is_submitted'] === true){
        //				$condition_status = true;
        //			}
        //		}

        //    	return $condition_status;

        // پیاده‌سازی بررسی وضعیت شرط از جدول
        return false;
    }

    public static bool MfGetConditionStatusFromInput(int formId, Dictionary<string, string> conditionParams, Dictionary<string, string> userInput)
    {
        //        	$form_id = (int) $condition_params['form_id'];
        //$element_name = $condition_params['element_name']; //this could be 'element_x' or 'element_x_x'
        //$rule_condition = $condition_params['rule_condition'];
        //$rule_keyword = strtolower($condition_params['rule_keyword']); //keyword is case insensitive

        //$condition_status = false; //the default status if false

        //$exploded = explode('_', $element_name);
        //$element_id = (int) $exploded[1];

        ////get the element properties of the current element id
        //$query = "select 

        //                       element_type,
        //				 element_choice_has_other,
        //				 element_time_showsecond,
        //				 element_time_24hour,
        //				 element_constraint,
        //				 element_matrix_parent_id,
        //				 element_matrix_allow_multiselect
        //                   from

        //                        ".MF_TABLE_PREFIX."form_elements
        //                  where

        //                       form_id = ? and element_id = ? ";
        //$params = array($form_id,$element_id);
        //$sth = mf_do_query($query,$params,$dbh);
        //$row = mf_do_fetch_result($sth);

        //$element_type = $row['element_type'];
        //$element_choice_has_other = $row['element_choice_has_other'];
        //$element_time_showsecond = (int) $row['element_time_showsecond'];
        //$element_time_24hour = (int) $row['element_time_24hour'];
        //$element_constraint = $row['element_constraint'];
        //$element_matrix_parent_id = (int) $row['element_matrix_parent_id'];
        //$element_matrix_allow_multiselect = (int) $row['element_matrix_allow_multiselect'];

        //      //if this is matrix field, we need to determine wether this is matrix choice or matrix checkboxes
        //      if ($element_type == 'matrix'){
        //          if (empty($element_matrix_parent_id))
        //          {
        //              if (!empty($element_matrix_allow_multiselect))
        //              {
        //			$element_type = 'matrix_checkbox';
        //              }
        //              else
        //              {
        //			$element_type = 'matrix_radio';
        //              }
        //          }
        //          else
        //          {
        //		//this is a child row of a matrix, get the parent id first and check the status of the multiselect option
        //		$query = "select element_matrix_allow_multiselect from ".MF_TABLE_PREFIX."form_elements where form_id = ? and element_id = ?";
        //		$params = array($form_id,$element_matrix_parent_id);
        //		$sth = mf_do_query($query,$params,$dbh);
        //		$row = mf_do_fetch_result($sth);

        //              if (!empty($row['element_matrix_allow_multiselect']))
        //              {
        //			$element_type = 'matrix_checkbox';
        //              }
        //              else
        //              {
        //			$element_type = 'matrix_radio';
        //              }
        //          }
        //      }

        //      if (in_array($element_type, array('text', 'textarea', 'simple_name', 'name', 'simple_name_wmiddle', 'name_wmiddle', 'address', 'phone', 'simple_phone', 'email', 'url')))
        //      {

        //          if ($element_type == 'phone'){
        //		$element_value = $user_input[$element_name.'_1'].$user_input[$element_name.'_2'].$user_input[$element_name.'_3'];
        //          }else
        //          {
        //		$element_value = strtolower($user_input[$element_name]);
        //          }

        //          if ($rule_condition == 'is'){
        //              if ($element_value == $rule_keyword){
        //			$condition_status = true;
        //              }else
        //              {
        //			$condition_status = false;
        //              }
        //          }else if ($rule_condition == 'is_not'){
        //              if ($element_value != $rule_keyword){
        //			$condition_status = true;
        //              }else
        //              {
        //			$condition_status = false;
        //              }
        //          }else if ($rule_condition == 'begins_with'){
        //              if (stripos($element_value,$rule_keyword) === 0)
        //              {
        //			$condition_status = true;
        //              }
        //              else
        //              {
        //			$condition_status = false;
        //              }
        //          }else if ($rule_condition == 'ends_with'){
        //              if (!empty($element_value) && substr_compare($element_value, $rule_keyword, strlen($element_value) - strlen($rule_keyword), strlen($rule_keyword), true) === 0)
        //              {
        //			$condition_status = true;
        //              }
        //              else
        //              {
        //			$condition_status = false;
        //              }
        //          }else if ($rule_condition == 'contains'){
        //              if (stripos($element_value,$rule_keyword) !== false)
        //              {
        //			$condition_status = true;
        //              }
        //              else
        //              {
        //			$condition_status = false;
        //              }
        //          }else if ($rule_condition == 'not_contain'){
        //              if (stripos($element_value,$rule_keyword) === false)
        //              {
        //			$condition_status = true;
        //              }
        //              else
        //              {
        //			$condition_status = false;
        //              }
        //          }
        //      }
        //      else if ($element_type == 'radio' || $element_type == 'select'){

        //	$query = "select 
        //					`option` 
        //				from

        //                          ".MF_TABLE_PREFIX."element_options
        //                     where

        //                             form_id = ? and element_id = ? and live = 1 and option_id = ? ";

        //	$params = array($form_id,$element_id,$user_input[$element_name]);

        //	$sth = mf_do_query($query,$params,$dbh);
        //	$row = mf_do_fetch_result($sth);

        //          if (!empty($row['option']) || $row['option'] == 0 || $row['option'] == '0'){
        //		$element_value = strtolower($row['option']);
        //          }else
        //          {

        //              //if the choice has 'other' and the user entered the value
        //              if (!empty($element_choice_has_other) && !empty($user_input[$element_name.'_other']))
        //              {
        //			$query = "select element_choice_other_label from ".MF_TABLE_PREFIX."form_elements where form_id = ? and element_id = ?";
        //			$params = array($form_id,$element_id);
        //			$sth = mf_do_query($query,$params,$dbh);
        //			$row = mf_do_fetch_result($sth);

        //			$element_value = strtolower($row['element_choice_other_label']);
        //              }
        //          }

        //          if ($rule_condition == 'is'){
        //              if ($element_value == $rule_keyword){
        //			$condition_status = true;
        //              }else
        //              {
        //			$condition_status = false;
        //              }
        //          }else if ($rule_condition == 'is_not'){
        //              if ($element_value != $rule_keyword){
        //			$condition_status = true;
        //              }else
        //              {
        //			$condition_status = false;
        //              }
        //          }else if ($rule_condition == 'begins_with'){
        //              if (stripos($element_value,$rule_keyword) === 0)
        //              {
        //			$condition_status = true;
        //              }
        //              else
        //              {
        //			$condition_status = false;
        //              }
        //          }else if ($rule_condition == 'ends_with'){
        //              if (!empty($element_value) && substr_compare($element_value, $rule_keyword, strlen($element_value) - strlen($rule_keyword), strlen($rule_keyword), true) === 0)
        //              {
        //			$condition_status = true;
        //              }
        //              else
        //              {
        //			$condition_status = false;
        //              }
        //          }else if ($rule_condition == 'contains'){
        //              if (stripos($element_value,$rule_keyword) !== false)
        //              {
        //			$condition_status = true;
        //              }
        //              else
        //              {
        //			$condition_status = false;
        //              }
        //          }else if ($rule_condition == 'not_contain'){
        //              if (stripos($element_value,$rule_keyword) === false)
        //              {
        //			$condition_status = true;
        //              }
        //              else
        //              {
        //			$condition_status = false;
        //              }
        //          }
        //      }else if ($element_type == 'time'){
        //          //there are few variants of the the time field, get the specific type
        //          if (!empty($element_time_showsecond) && !empty($element_time_24hour))
        //          {
        //		$element_type = 'time_showsecond24hour';
        //          }
        //          else if (!empty($element_time_showsecond) && empty($element_time_24hour))
        //          {
        //		$element_type = 'time_showsecond';
        //          }
        //          else if (empty($element_time_showsecond) && !empty($element_time_24hour)){
        //		$element_type = 'time_24hour';
        //	}

        //	$exploded = array();
        //	$exploded = explode(':', $rule_keyword); //rule keyword format -> HH:MM:SS:AM

        //	if($element_type == 'time'){
        //		$rule_keyword  = "{$exploded[0]}:{$exploded[1]}:00 {$exploded[3]}";
        //		$element_value = $user_input[$element_name."_1"].":".$user_input[$element_name."_2"].":00 ".$user_input[$element_name."_4"];
        //	}else if($element_type == 'time_showsecond'){
        //		$rule_keyword  = "{$exploded[0]}:{$exploded[1]}:{$exploded[2]} {$exploded[3]}";
        //		$element_value = $user_input[$element_name."_1"].":".$user_input[$element_name."_2"].":".$user_input[$element_name."_3"]." ".$user_input[$element_name."_4"];
        //	}else if($element_type == 'time_24hour'){
        //		$rule_keyword  = "{$exploded[0]}:{$exploded[1]}:00";
        //		$element_value = $user_input[$element_name."_1"].":".$user_input[$element_name."_2"].":00";
        //	}else if($element_type == 'time_showsecond24hour'){
        //		$rule_keyword  = "{$exploded[0]}:{$exploded[1]}:{$exploded[2]}";
        //		$element_value = $user_input[$element_name."_1"].":".$user_input[$element_name."_2"].":".$user_input[$element_name."_3"];
        //	}

        //	$rule_keyword  = strtotime($rule_keyword);
        //	$element_value = strtotime($element_value);

        //	if($element_value !== false){
        //		if($rule_condition == 'is'){
        //			if($element_value == $rule_keyword){
        //				$condition_status = true;
        //			}else{
        //				$condition_status = false;
        //			}
        //		}else if($rule_condition == 'is_before'){
        //			if($element_value < $rule_keyword){
        //				$condition_status = true;
        //			}else{
        //				$condition_status = false;
        //			}
        //		}else if($rule_condition == 'is_after'){
        //			if($element_value > $rule_keyword){
        //				$condition_status = true;
        //			}else{
        //				$condition_status = false;
        //			}
        //		}
        //	}

        //}else if($element_type == 'money' || $element_type == 'number'){

        //	if($element_type == 'money'){
        //		if($element_constraint == 'yen'){ //yen only have one field
        //			$element_value = (float) $user_input[$element_name];
        //		}else{
        //			$element_value = (float) $user_input[$element_name."_1"].".".$user_input[$element_name."_2"];
        //		}
        //	}else if($element_type == 'number'){
        //		$element_value = (float) $user_input[$element_name];
        //	}

        //	$rule_keyword = (float) $rule_keyword;

        //	if($rule_condition == 'is'){
        //		if($element_value == $rule_keyword){
        //			$condition_status = true;
        //		}else{
        //			$condition_status = false;
        //		}
        //	}else if($rule_condition == 'less_than'){
        //		if($element_value < $rule_keyword){
        //			$condition_status = true;
        //		}else{
        //			$condition_status = false;
        //		}
        //	}else if($rule_condition == 'greater_than'){
        //		if($element_value > $rule_keyword){
        //			$condition_status = true;
        //		}else{
        //			$condition_status = false;
        //		}
        //	}

        //}else if($element_type == 'rating'){


        //	$element_value = (int) $user_input[$element_name];
        //	$rule_keyword  = (int) $rule_keyword;

        //	if($rule_condition == 'is'){
        //		if($element_value == $rule_keyword){
        //			$condition_status = true;
        //		}else{
        //			$condition_status = false;
        //		}
        //	}else if($rule_condition == 'is_not'){
        //		if($element_value != $rule_keyword){
        //			$condition_status = true;
        //		}else{
        //			$condition_status = false;
        //		}
        //	}else if($rule_condition == 'less_than'){
        //		if($element_value < $rule_keyword){
        //			$condition_status = true;
        //		}else{
        //			$condition_status = false;
        //		}
        //	}else if($rule_condition == 'greater_than'){
        //		if($element_value > $rule_keyword){
        //			$condition_status = true;
        //		}else{
        //			$condition_status = false;
        //		}
        //	}

        //}else if($element_type == 'matrix_radio'){
        //	$query = "select 
        //					`option` 
        //				from 
        //					".MF_TABLE_PREFIX."element_options 
        //			   where 
        //					form_id = ? and 
        //					element_id = ? and 
        //					option_id = ?";

        //	if(!empty($element_matrix_parent_id)){
        //		$element_id = $element_matrix_parent_id;
        //	}

        //	$params = array($form_id,$element_id,$user_input[$element_name]);
        //	$sth 	= mf_do_query($query,$params,$dbh);
        //	$row 	= mf_do_fetch_result($sth);
        //	$element_value = strtolower($row['option']);

        //	if($rule_condition == 'is'){
        //		if($element_value == $rule_keyword){
        //			$condition_status = true;
        //		}else{
        //			$condition_status = false;
        //		}
        //	}else if($rule_condition == 'is_not'){
        //		if($element_value != $rule_keyword){
        //			$condition_status = true;
        //		}else{
        //			$condition_status = false;
        //		}
        //	}else if($rule_condition == 'begins_with'){
        //		if(stripos($element_value,$rule_keyword) === 0){
        //			$condition_status = true;
        //		}else{
        //			$condition_status = false;
        //		}
        //	}else if($rule_condition == 'ends_with'){
        //		if(!empty($element_value) && substr_compare($element_value, $rule_keyword, strlen($element_value)-strlen($rule_keyword), strlen($rule_keyword), true) === 0){
        //			$condition_status = true;
        //		}else{
        //			$condition_status = false;
        //		}
        //	}else if($rule_condition == 'contains'){
        //		if(stripos($element_value,$rule_keyword) !== false){
        //			$condition_status = true;
        //		}else{
        //			$condition_status = false;
        //		}
        //	}else if($rule_condition == 'not_contain'){
        //		if(stripos($element_value,$rule_keyword) === false){
        //			$condition_status = true;
        //		}else{
        //			$condition_status = false;
        //		}
        //	}
        //}else if($element_type == 'matrix_checkbox' || $element_type == 'checkbox'){
        //	$element_value = $user_input[$element_name];

        //	if($rule_condition == 'is_one'){
        //		if(!empty($element_value)){
        //			$condition_status = true;
        //		}else{
        //			$condition_status = false;
        //		}
        //	}else if($rule_condition == 'is_zero'){
        //		if(empty($element_value)){
        //			$condition_status = true;
        //		}else{
        //			$condition_status = false;
        //		}
        //	}
        //}else if($element_type == 'date' || $element_type == 'europe_date'){
        //	$exploded = array();
        //	$exploded = explode('/', $rule_keyword); //rule keyword format -> mm/dd/yyyy

        //	$rule_keyword = strtotime($exploded[2].'-'.$exploded[0].'-'.$exploded[1]); //this should be yyyy-mm-dd

        //	if($element_type == 'date'){
        //		$element_value = $user_input[$element_name."_3"]."-".$user_input[$element_name."_1"]."-".$user_input[$element_name."_2"];
        //	}else if($element_type == 'europe_date'){
        //		$element_value = $user_input[$element_name."_3"]."-".$user_input[$element_name."_2"]."-".$user_input[$element_name."_1"];
        //	}
        //	$element_value = strtotime($element_value);

        //	if($element_value !== false){
        //		if($rule_condition == 'is'){
        //			if($element_value == $rule_keyword){
        //				$condition_status = true;
        //			}else{
        //				$condition_status = false;
        //			}
        //		}else if($rule_condition == 'is_before'){
        //			if($element_value < $rule_keyword){
        //				$condition_status = true;
        //			}else{
        //				$condition_status = false;
        //			}
        //		}else if($rule_condition == 'is_after'){
        //			if($element_value > $rule_keyword){
        //				$condition_status = true;
        //			}else{
        //				$condition_status = false;
        //			}
        //		}
        //	}
        //}

        //return $condition_status;


        // پیاده‌سازی بررسی وضعیت شرط از ورودی کاربر
        return false;
    }

    public static Dictionary<string, object> MfGetFormProperties(DbConnection dbh, int formId, string[] columns = null)
    {
        var formProperties = new Dictionary<string, object>();

        if (columns == null || columns.Length == 0)
        {
            // اگر آرایه ستون‌ها مشخص نشده باشد، همه ستون‌های جدول را می‌خوانیم
            var query = $"SHOW COLUMNS FROM {MF_TABLE_PREFIX}forms";
            using (var cmd = dbh.CreateCommand())
            {
                cmd.CommandText = query;
                using (var reader = cmd.ExecuteReader())
                {
                    var columnList = new List<string>();
                    while (reader.Read())
                    {
                        string fieldName = reader["Field"].ToString();
                        if (fieldName != "form_id" && fieldName != "form_name")
                        {
                            columnList.Add(fieldName);
                        }
                    }
                    columns = columnList.ToArray();
                }
            }
        }

        string columnsJoined = string.Join("`,`", columns);
        string query2 = $"SELECT `{columnsJoined}` FROM {MF_TABLE_PREFIX}forms WHERE form_id = @formId";

        using (var cmd = dbh.CreateCommand())
        {
            cmd.CommandText = query2;
            cmd.Parameters.Add(CreateParameter(cmd, "@formId", formId));

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    foreach (string column in columns)
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal(column)))
                        {
                            formProperties[column] = reader[column];
                        }
                    }
                }
            }
        }

        return formProperties;
    }

    private static DbParameter CreateParameter(DbCommand cmd, string name, object value)
    {
        var param = cmd.CreateParameter();
        param.ParameterName = name;
        param.Value = value ?? DBNull.Value;
        return param;
    }

    public static Dictionary<string, object> MfGetTemplateVariables(DbConnection dbh, int formId, int entryId, Dictionary<string, object> options = null)
    {
        options = options ?? new Dictionary<string, object>();

        var mfSettings = MfGetSettings(dbh);
        var templateData = new Dictionary<string, object>();
        var templateVariables = new List<string>();
        var templateValues = new List<string>();

        bool asPlainText = options.ContainsKey("as_plain_text") && (bool)options["as_plain_text"];
        bool targetIsAdmin = options.ContainsKey("target_is_admin") && (bool)options["target_is_admin"];
        bool useListLayout = options.ContainsKey("use_list_layout") && (bool)options["use_list_layout"];

        var entryOptions = new Dictionary<string, object>
        {
            ["strip_download_link"] = options.ContainsKey("strip_download_link") ? options["strip_download_link"] : false,
            ["strip_checkbox_image"] = true,
            ["machform_path"] = options.ContainsKey("machform_path") ? options["machform_path"] : "",
            ["show_image_preview"] = options.ContainsKey("show_image_preview") ? options["show_image_preview"] : false,
            ["target_is_admin"] = targetIsAdmin,
            ["hide_encrypted_data"] = options.ContainsKey("hide_encrypted_data") ? options["hide_encrypted_data"] : false,
            ["hide_password_data"] = options.ContainsKey("hide_password_data") ? options["hide_password_data"] : false,
            ["encryption_private_key"] = options.ContainsKey("encryption_private_key") ? options["encryption_private_key"] : false
        };

        var entryDetails = MfGetEntryDetails(dbh, formId, entryId, entryOptions);

        // بقیه پیاده‌سازی مشابه PHP
        // ...

        templateData["variables"] = templateVariables.ToArray();
        templateData["values"] = templateValues.ToArray();

        return templateData;
    }

    public static string MfParseTemplateVariables(DbConnection dbh, int formId, int entryId, string templateContent)
    {
        var mfSettings = MfGetSettings(dbh);

        var templateDataOptions = new Dictionary<string, object>
        {
            ["strip_download_link"] = false,
            ["as_plain_text"] = true,
            ["target_is_admin"] = true,
            ["machform_path"] = mfSettings["base_url"]
        };

        var templateData = MfGetTemplateVariables(dbh, formId, entryId, templateDataOptions);

        var templateVariables = (string[])templateData["variables"];
        var templateValues = (string[])templateData["values"];

        for (int i = 0; i < templateVariables.Length; i++)
        {
            templateContent = templateContent.Replace(templateVariables[i], templateValues[i]);
        }

        return templateContent;
    }

    public static string MfGetCountryCode(string countryName)
    {
        var countries = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["United States"] = "US",
            ["United Kingdom"] = "GB",
            ["Canada"] = "CA",
            ["Australia"] = "AU",
            // بقیه کشورها...
        };

        return countries.ContainsKey(countryName) ? countries[countryName] : null;
    }
    public static string MfTrimMaxLength(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text)) return text;

        if (text.Length > maxLength + 3)
        {
            return text.Substring(0, maxLength) + "...";
        }
        return text;
    }
    public static void MfArrayInsert<T>(ref List<T> array, int position, List<T> insertArray)
    {
        if (position < 0 || position > array.Count)
            throw new ArgumentOutOfRangeException(nameof(position));

        array.InsertRange(position, insertArray);
    }
    public static bool MfIsWhitelistedIpAddress(DbConnection dbh, string clientIpAddress)
    {
        var mfSettings = MfGetSettings(dbh);
        string ipWhitelist = mfSettings.ContainsKey("ip_whitelist") ? mfSettings["ip_whitelist"].ToString() : null;

        if (string.IsNullOrWhiteSpace(ipWhitelist))
            return true;

        var ipWhitelistArray = ipWhitelist.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string allowedIpAddress in ipWhitelistArray)
        {
            string trimmedIp = allowedIpAddress.Trim();

            if (trimmedIp == "*")
                return true;

            if (trimmedIp.Contains("*"))
            {
                // بررسی آدرس IP با wildcard
                if (IsIpMatchWithWildcard(clientIpAddress, trimmedIp))
                    return true;
            }
            else if (clientIpAddress == trimmedIp)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsIpMatchWithWildcard(string clientIp, string pattern)
    {
        string[] clientParts = clientIp.Split('.');
        string[] patternParts = pattern.Split('.');

        if (clientParts.Length != 4 || patternParts.Length != 4)
            return false;

        for (int i = 0; i < 4; i++)
        {
            if (patternParts[i] != "*" && clientParts[i] != patternParts[i])
                return false;
        }

        return true;
    }
}