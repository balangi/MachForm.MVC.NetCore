function activate_license()
{
	return true;
}
function version_compare(e, t, i)
{
	var n = i && i . lexicographical, i = i && i . zeroExtend, a = e . split("."), o = t . split(".");
	function c(e)
	{
		return (n ? / ^ \d + [A - Za - z] * $/ : / ^ \d + $/) . test(e)
	}
	if (!a . every(c) || !o . every(c)) return NaN;
	if (i) {
		for (; a . length < o . length;) a . push("0");
		for (; o . length < a . length;) o . push("0")
	}
	n || (a = a . map(Number), o = o . map(Number));
	for (var s = 0; s < a . length;++s) {
		if (o . length == s) return 1;
		if (a[s] != o[s]) return a[s] > o[s] ? 1 : -1
	}
	return a . length != o . length ? -1 : 0
}
$(function() {
	tippy(
		"[data-tippy-content]", {
			trigger:"click",
			placement:"bottom",
			boundary:"window",
			arrow:!0
		}
	), $("#smtp_enable") . click(
		function() {
			1 == $(this) . prop("checked") ? $("#ms_box_smtp .ms_box_email") . slideDown() : $("#ms_box_smtp .ms_box_email") . slideUp()
		}
	), $("#more_option_misc_settings") . click(
		function() {
			return "advanced options" == $(this) . text() ? ($("#ms_box_misc .ms_box_more") . slideDown(), $(this) . text("hide options")) : ($("#ms_box_misc .ms_box_more") . slideUp(), $(this) . text("advanced options")), !1
		}
	), $("#enable_ip_restriction") . click(
		function() { 1 == $(this) . prop("checked") ? $("#div_ip_whitelist") . slideDown() : $("#div_ip_whitelist") . slideUp() }
	), $("#enable_account_locking") . click(
		function() { 1 == $(this) . prop("checked") ? $("#div_account_locking") . slideDown() : $("#div_account_locking") . slideUp() }
	), $("#enable_data_retention") . click(
		function() { 1 == $(this) . prop("checked") ? $("#div_data_retention") . slideDown() : $("#div_data_retention") . slideUp() }
	), $("#button_save_main_settings") . click(
		function() {
			return "Saving..." != $("#button_save_main_settings") . text() && ($("#button_save_main_settings") . prop("disabled", !0), $("#button_save_main_settings") . text("Saving..."), $("#button_save_main_settings") . after("<img style=\"margin-left: 10px\" src='images/loader_small_grey.gif' />"), $("#ms_form") . submit()), !1
		}
	), $("#dialog-change-license") . dialog( {
			modal:!0,
			autoOpen:!1,
			closeOnEscape:!1,
			width:400,
			position:["center", 150],
			draggable:!1,
			resizable:!1,
			buttons:[ {
					text:"Activate New License", id:"dialog-change-license-btn-save-changes",

					class:"bb_button bb_small bb_green", click:function() {
						"" == $("#dialog-change-license-input") . val() ? alert("Please enter your license key!") : ($("#dialog-change-license-btn-save-changes") . prop("disabled", !0), $("#dialog-change-license-btn-save-changes") . text("Activating..."), $("#license_box") . data("licensekey", $("#dialog-change-license-input") . val()), activate_license())
					}
				}, {
					text:"Cancel", id:"dialog-change-license-btn-cancel",

					class:"btn_secondary_action", click:function() {
						$("#dialog-change-license-btn-save-changes") . prop("disabled", !1), $("#dialog-change-license-btn-save-changes") . text("Activate New License"), $(this) . dialog("close")
					}
				}
			]
		}
	), $("#ms_change_license") . click(function() { return $("#dialog-change-license") . dialog("open"), !1 }), $("#ms_upgrade_license") . click(function() { return $("#dialog-upgrade-license") . dialog("open"), !1 }), $("#lic_activate") . click(function() { return "activating..." != $(this) . text() && ($(this) . text("activating..."), activate_license()), !1 }), 0 < $("#lic_activate") . length && activate_license(), $("#dialog-change-license-form") . submit(function() { return $("#dialog-change-license-btn-save-changes") . click(), !1 }), $("#ms_btn_export_form") . click(
		function() {
			var e = $("#export_form_id") . val(), t = $("#content") . data("csrftoken");
			window . location . href = "export_form.php?form_id="+e + "&csrf_token="+t
		}
	), $("input[name=export_import_type]") . click(
		function() {
			var e = $(this) . val();
			$("#tab_export_form,#tab_import_form") . hide(), "export" == e ? $("#tab_export_form") . show() : "import" == e && $("#tab_import_form") . show()
		}
	), $("#ms_form_import_file") . uploadifive( {
			uploadScript:"import_form_uploader.php",
			buttonText:"Select File",
			removeCompleted:!0,
			formData: {
				session_id:$(".main_settings"
			) . data("session_id")
		}, auto:!0, multi:!1, onUploadError:function(e, t, i, n) { alert("The file "+e . name + " could not be uploaded: "+n) }, onUploadComplete:function(e, t) {
			var i, n, a = !1;
			try {
				var o = jQuery . parseJSON(t), a = !0
			} catch (e) {
				a = !1, alert(t)
			}
			1 == a && "ok" == o . status ? (i = o . file_name, n = $("#content") . data("csrftoken"), $. ajax( {
					type:"POST",
					async:!0,
					url:"import_form_parser.php",
					data: {
						file_name:i,
						csrf_token:n
					},
					cache:!1,
					global:!1,
					dataType:"json",
					error:function(e, t, i) { alert("Error while importing file. Error Message:\n"+e . responseText) },
					success:function(e) {
						"ok" == e . status ? ($("#dialog-form-import-success") . data("form_id", e . new_form_id), $("#dialog-form-import-success") . data("form_name", e . new_form_name), $("#form-imported-link") . text(e . new_form_name), $("#form-imported-link") . attr("href", "view.php?id="+e . new_form_id), $("#dialog-form-import-success") . dialog("open")) : ("error" == e . status && $("#dialog-warning-msg") . html(e . message), $("#dialog-warning") . dialog("open"))
					}
				}
			)) : alert("Error uploading file. Please try again.")
		}
	}), $("#ldap_enable") . click(
		function() {
			1 == $(this) . prop("checked") ? $("#ms_box_ldap .ms_box_email") . slideDown() : $("#ms_box_ldap .ms_box_email") . slideUp()
		}
	), $("#dialog-form-import-success") . dialog( {
			modal:!0,
			autoOpen:!1,
			closeOnEscape:!1,
			width:450,
			resizable:!1,
			draggable:!1,
			position:["center", "center"],
			open:function() { $("#btn-form-success-done") . blur() },
			buttons:[ {
					text:"Done", id:"btn-form-success-done",

					class:"bb_button bb_small bb_green",
					click:function() { $(this) . dialog("close") }
				}, {
					text:"Edit Form", id:"btn-form-success-edit",

					class:"btn_secondary_action",
					click:function() {
						var e = $("#dialog-form-import-success") . data("form_id");
						window . location . replace("edit_form.php?id="+e)
					}
				}
			]
		}
	), $("#dialog-warning") . dialog( {
			modal:!0,
			autoOpen:!1,
			closeOnEscape:!1,
			width:450,
			position:["center", "center"],
			draggable:!1,
			resizable:!1,
			open:function() { $(this) . next() . find("button") . blur() },
			buttons:[ {
					text:"OK",

					class:"bb_button bb_small bb_green", click:function() { $(this) . dialog("close") }
				}
			]
		}
	), $("#dialog-upgrade-license") . dialog( {
			modal:!0,
			autoOpen:!1,
			closeOnEscape:!1,
			width:500,
			position:["center", "center"],
			draggable:!1,
			resizable:!1,
			buttons:[ {
					text:"Close",

					class:"bb_button bb_small bb_green", click:function() { $(this) . dialog("close") }
				}
			]
		}
	), $("#ldap_type") . bind(
		"change",
		function() { "ad" == $(this) . val() ? $("#account_suffix_span") . fadeIn() : $("#account_suffix_span") . hide() }
	), $("#dialog-get-new-update") . dialog( {
			modal:!0,
			autoOpen:!1,
			closeOnEscape:!1,
			width:400,
			position:["center", 150],
			draggable:!1,
			resizable:!1,
			buttons:[ {
					text:"More Details", id:"dialog-get-new-update-btn-download",

					class:"bb_button bb_small bb_green",
					click:function() { window . location . replace($("#license_box") . data("update_url")) }
				}, {
					text:"Later", id:"dialog-get-new-update-btn-later",

					class:"btn_secondary_action", click:function() { $(this) . dialog("close") }
				}
			]
		}
	), $("#check_update_link") . click(
		function() {
			return "check for new version" != $(this) . text() || ($(this) . text("checking for updates..."), $("#check_update_loader") . show(), $. ajax( {
					type:"GET",
					async:!0,
					url:"",
					cache:!1,
					global:!0,
					dataType:"jsonp",
					error:function(e, t, i) { alert("Error while checking for updates. Error Message:\n"+e . responseText) },
					success:function(e) {
						var t = String($("#license_box") . data("machformversion")), i = e . latest_version;
						version_compare(t, i) < 0 ? ($("#latest_version_span") . text(i), $("#license_box") . data("update_url", e . update_url), $("#dialog-get-new-update") . dialog("open")) : alert("You're up to date! MachForm Version "+t + " is currently the newest version available."), $("#check_update_link") . text("check for new version"), $("#check_update_loader") . hide()
					}
				}
			)), !1
		}
	)
});