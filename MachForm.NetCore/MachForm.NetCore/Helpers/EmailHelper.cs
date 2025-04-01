using System.Net;
using System.Net.Mail;

namespace MachForm.NetCore.Helpers;

public class EmailUtils
{
    public static void MfSendResumeLink(int formId, int entryId, string formResumeUrl, string resumeEmail)
    {
        // دریافت تنظیمات از دیتابیس
        var mfSettings = MfGetSettings();
        var formProperties = MfGetFormProperties(formId, new[] {
            "form_name", "form_language", "form_resume_subject",
            "form_resume_content", "form_resume_from_name", "form_resume_from_email_address"
        });

        // تنظیم زبان
        MfSetLanguage(formProperties["form_language"]);

        // تنظیم مقادیر پیش‌فرض
        if (string.IsNullOrEmpty(formProperties["form_resume_subject"]))
            formProperties["form_resume_subject"] = "Your resume link"; // جایگزین با متن مناسب

        if (string.IsNullOrEmpty(formProperties["form_resume_content"]))
            formProperties["form_resume_content"] = "Click here to resume your form: {resume_url}";

        if (string.IsNullOrEmpty(formProperties["form_resume_from_name"]))
            formProperties["form_resume_from_name"] = mfSettings["default_from_name"];

        if (string.IsNullOrEmpty(formProperties["form_resume_from_email_address"]))
            formProperties["form_resume_from_email_address"] = mfSettings["default_from_email"];

        // جایگزینی متغیرهای قالب
        formProperties["form_resume_content"] = formProperties["form_resume_content"]
            .Replace("{resume_url}", formResumeUrl);

        // ارسال ایمیل
        try
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(
                    formProperties["form_resume_from_email_address"],
                    formProperties["form_resume_from_name"]);

                message.To.Add(resumeEmail);
                message.Subject = formProperties["form_resume_subject"];
                message.Body = formProperties["form_resume_content"];
                message.IsBodyHtml = true;

                using (var client = new SmtpClient(mfSettings["smtp_host"], int.Parse(mfSettings["smtp_port"])))
                {
                    if (!string.IsNullOrEmpty(mfSettings["smtp_username"]))
                    {
                        client.Credentials = new NetworkCredential(
                            mfSettings["smtp_username"],
                            mfSettings["smtp_password"]);
                    }

                    client.EnableSsl = mfSettings["smtp_secure"] == "1";
                    client.Send(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }

    public static void MfSendLoginInfo(int userId, string password)
    {
        var mfSettings = MfGetSettings();

        // دریافت اطلاعات کاربر از دیتابیس
        var userInfo = GetUserInfo(userId);

        string subject = "Your MachForm login information";
        string body = $@"
Hello {userInfo["user_fullname"]},

You can login to MachForm panel using the following information:

<b>URL:</b> {mfSettings["base_url"]}

<b>Email:</b> {userInfo["user_email"]}
<b>Password:</b> {password}

Thank you.";

        try
        {
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(
                    mfSettings["default_from_email"],
                    mfSettings["default_from_name"]);

                message.To.Add(userInfo["user_email"]);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var client = new SmtpClient(mfSettings["smtp_host"], int.Parse(mfSettings["smtp_port"])))
                {
                    if (!string.IsNullOrEmpty(mfSettings["smtp_username"]))
                    {
                        client.Credentials = new NetworkCredential(
                            mfSettings["smtp_username"],
                            mfSettings["smtp_password"]);
                    }

                    client.EnableSsl = mfSettings["smtp_secure"] == "1";
                    client.Send(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }

    private static Dictionary<string, string> GetUserInfo(int userId)
    {
        // پیاده‌سازی دریافت اطلاعات کاربر از دیتابیس
        return new Dictionary<string, string>();
    }
}