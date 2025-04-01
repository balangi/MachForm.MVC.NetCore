//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data.Entity;
//using System.Net.Mail;
//using System.IO;
//using iText.Html2pdf;
//using iText.Kernel.Pdf;
//using iText.Kernel.Geom;
//using iText.Layout;
//using iText.Layout.Element;
//using System.Net;
//using System.Net.Http;
//using Newtonsoft.Json;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Cryptography;
//using System.Text.RegularExpressions;
//using System.Web;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;

public class MachFormHelper
{
}
//    private readonly ApplicationDbContext _dbContext;

//    public MachFormHelper(ApplicationDbContext dbContext)
//    {
//        _dbContext = dbContext;
//    }

//    // تابع تبدیل تاریخ به فرمت نسبی
//    public string MfRelativeDate(string inputDate)
//    {
//        if (string.IsNullOrEmpty(inputDate))
//            return "N/A";

//        DateTime postedDate;
//        if (!DateTime.TryParse(inputDate, out postedDate))
//            return "N/A";

//        TimeSpan diff = DateTime.Now - postedDate;

//        if (diff.TotalDays > 30)
//        {
//            return postedDate.ToString("MMMM dd, yyyy");
//        }
//        else
//        {
//            StringBuilder relativeDate = new StringBuilder();

//            if (diff.TotalDays >= 7)
//            {
//                int weeks = (int)(diff.TotalDays / 7);
//                relativeDate.Append($"{weeks} week{(weeks > 1 ? "s" : "")}");

//                int remainingDays = (int)(diff.TotalDays % 7);
//                if (remainingDays > 0)
//                    relativeDate.Append($", {remainingDays} day{(remainingDays > 1 ? "s" : "")}");
//            }
//            else if (diff.TotalDays >= 1)
//            {
//                int days = (int)diff.TotalDays;
//                relativeDate.Append($"{days} day{(days > 1 ? "s" : "")}");

//                if (diff.Hours > 0)
//                    relativeDate.Append($", {diff.Hours} hour{(diff.Hours > 1 ? "s" : "")}");
//            }
//            else if (diff.Hours >= 1)
//            {
//                relativeDate.Append($"{diff.Hours} hour{(diff.Hours > 1 ? "s" : "")}");

//                if (diff.Minutes > 0)
//                    relativeDate.Append($", {diff.Minutes} minute{(diff.Minutes > 1 ? "s" : "")}");
//            }
//            else if (diff.Minutes >= 1)
//            {
//                relativeDate.Append($"{diff.Minutes} minute{(diff.Minutes > 1 ? "s" : "")}");
//            }
//            else
//            {
//                relativeDate.Append($"{diff.Seconds} second{(diff.Seconds > 1 ? "s" : "")}");
//            }

//            return $"{relativeDate} ago";
//        }
//    }

//    // تابع کوتاه شده تبدیل تاریخ به فرمت نسبی
//    public string MfShortRelativeDate(string inputDate)
//    {
//        if (string.IsNullOrEmpty(inputDate))
//            return string.Empty;

//        DateTime postedDate;
//        if (!DateTime.TryParse(inputDate, out postedDate))
//            return string.Empty;

//        TimeSpan diff = DateTime.Now - postedDate;

//        if (diff.TotalDays > 30)
//        {
//            if (postedDate.Year < DateTime.Now.Year)
//                return postedDate.ToString("yyyy-MM-dd");
//            else
//                return postedDate.ToString("MMM d");
//        }
//        else
//        {
//            StringBuilder relativeDate = new StringBuilder();

//            if (diff.TotalDays >= 7)
//            {
//                int weeks = (int)(diff.TotalDays / 7);
//                relativeDate.Append($"{weeks} week{(weeks > 1 ? "s" : "")}");
//            }
//            else if (diff.TotalDays >= 1)
//            {
//                int days = (int)diff.TotalDays;
//                relativeDate.Append($"{days} day{(days > 1 ? "s" : "")}");
//            }
//            else if (diff.Hours >= 1)
//            {
//                relativeDate.Append($"{diff.Hours} hour{(diff.Hours > 1 ? "s" : "")}");
//            }
//            else if (diff.Minutes >= 1)
//            {
//                relativeDate.Append($"{diff.Minutes} minute{(diff.Minutes > 1 ? "s" : "")}");
//            }
//            else
//            {
//                relativeDate.Append($"{diff.Seconds} second{(diff.Seconds > 1 ? "s" : "")}");
//            }

//            return $"{relativeDate} ago";
//        }
//    }

//    // بررسی سال کبیسه
//    public bool MfIsLeapYear(int year)
//    {
//        return DateTime.IsLeapYear(year);
//    }

//    // حذف کامل یک دایرکتوری
//    public bool MfFullRmdir(string dirPath)
//    {
//        try
//        {
//            if (Directory.Exists(dirPath))
//            {
//                Directory.Delete(dirPath, true);
//                return true;
//            }
//            return false;
//        }
//        catch
//        {
//            return false;
//        }
//    }

//    // ارسال نوتیفیکیشن وب هوک
//    public async Task MfSendWebhookNotification(int formId, int entryId, int webhookRuleId)
//    {
//        var webhookSettings = await _dbContext.WebhookOptions
//            .FirstOrDefaultAsync(w => w.FormId == formId && w.RuleId == webhookRuleId);

//        if (webhookSettings == null)
//            return;

//        var parameters = await _dbContext.WebhookParameters
//            .Where(w => w.FormId == formId && w.RuleId == webhookRuleId)
//            .OrderBy(w => w.AwpId)
//            .ToListAsync();

//        // دریافت متغیرهای تمپلیت
//        var templateData = await MfGetTemplateVariables(formId, entryId, new TemplateDataOptions
//        {
//            AsPlainText = true,
//            TargetIsAdmin = true
//        });

//        // آماده سازی داده وب هوک
//        object webhookData;
//        if (webhookSettings.WebhookFormat == "key-value")
//        {
//            var data = new Dictionary<string, string>();
//            foreach (var param in parameters)
//            {
//                var paramValue = ReplaceTemplateVariables(param.ParamValue, templateData.Variables, templateData.Values);
//                data[param.ParamName] = paramValue;
//            }
//            webhookData = data;
//        }
//        else
//        {
//            webhookData = ReplaceTemplateVariables(webhookSettings.WebhookRawData, templateData.Variables, templateData.Values);
//        }

//        // ارسال درخواست
//        using (var client = new HttpClient())
//        {
//            // تنظیم هدرها
//            if (webhookSettings.EnableCustomHttpHeaders)
//            {
//                var headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(webhookSettings.CustomHttpHeaders);
//                foreach (var header in headers)
//                {
//                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
//                }
//            }

//            // احراز هویت
//            if (webhookSettings.EnableHttpAuth)
//            {
//                var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{webhookSettings.HttpUsername}:{webhookSettings.HttpPassword}"));
//                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authValue);
//            }

//            HttpResponseMessage response;
//            switch (webhookSettings.WebhookMethod.ToLower())
//            {
//                case "post":
//                    response = await client.PostAsJsonAsync(webhookSettings.WebhookUrl, webhookData);
//                    break;
//                case "put":
//                    response = await client.PutAsJsonAsync(webhookSettings.WebhookUrl, webhookData);
//                    break;
//                default:
//                    var queryString = string.Join("&", ((Dictionary<string, string>)webhookData).Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
//                    response = await client.GetAsync($"{webhookSettings.WebhookUrl}?{queryString}");
//                    break;
//            }

//            if (!response.IsSuccessStatusCode)
//            {
//                var errorMessage = $"Error Sending Webhooks! Status: {response.StatusCode}";
//                switch (response.StatusCode)
//                {
//                    case HttpStatusCode.NotFound:
//                        errorMessage += " - Website URL Not Found";
//                        break;
//                    case HttpStatusCode.Unauthorized:
//                        errorMessage += " - Unauthorized Access. Incorrect HTTP Username/Password";
//                        break;
//                    case HttpStatusCode.Forbidden:
//                        errorMessage += " - Forbidden. You don't have permission to access the Website URL";
//                        break;
//                    case HttpStatusCode.Redirect:
//                        errorMessage += " - Page Moved Temporarily";
//                        break;
//                    case HttpStatusCode.MovedPermanently:
//                        errorMessage += " - Page Moved Permanently";
//                        break;
//                    case HttpStatusCode.InternalServerError:
//                        errorMessage += " - Internal Server Error";
//                        break;
//                }
//                throw new Exception(errorMessage);
//            }
//        }
//    }

//    // ارسال ایمیل نوتیفیکیشن
//    public async Task MfSendNotification(int formId, int entryId, string toEmails, EmailParameters emailParams)
//    {
//        var settings = await _dbContext.Settings.FirstOrDefaultAsync();
//        var formProperties = await _dbContext.Forms
//            .Where(f => f.FormId == formId)
//            .Select(f => new
//            {
//                f.FormEntryEditEnable,
//                f.FormEntryEditHideEditlink
//            })
//            .FirstOrDefaultAsync();

//        // دریافت متغیرهای تمپلیت
//        var templateData = await MfGetTemplateVariables(formId, entryId, new TemplateDataOptions
//        {
//            AsPlainText = emailParams.AsPlainText,
//            TargetIsAdmin = emailParams.TargetIsAdmin,
//            MachformPath = emailParams.MachformBasePath
//        });

//        // جایگزینی متغیرها
//        var fromName = ReplaceTemplateVariables(emailParams.FromName ?? settings.DefaultFromName, templateData.Variables, templateData.Values);
//        var fromEmail = ReplaceTemplateVariables(emailParams.FromEmail ?? settings.DefaultFromEmail, templateData.Variables, templateData.Values);
//        var replyToEmail = ReplaceTemplateVariables(emailParams.ReplyToEmail ?? fromEmail, templateData.Variables, templateData.Values);
//        var subject = ReplaceTemplateVariables(emailParams.Subject, templateData.Variables, templateData.Values);
//        var content = ReplaceTemplateVariables(emailParams.Content, templateData.Variables, templateData.Values);

//        // اگر امکان ویرایش ورودی فعال است و برای کاربر ارسال می‌شود، لینک ویرایش را اضافه کنید
//        if (formProperties.FormEntryEditEnable && !emailParams.TargetIsAdmin && !formProperties.FormEntryEditHideEditlink)
//        {
//            content += emailParams.AsPlainText ? "\n\n{edit_link}" : "<br/><br/>{edit_link}";
//        }

//        // جایگزینی متغیرها در آدرس ایمیل
//        var emailAddresses = ReplaceTemplateVariables(toEmails, templateData.Variables, templateData.Values)
//            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
//            .Select(e => e.Trim())
//            .ToList();

//        if (emailParams.CheckHookFile)
//        {
//            // این بخش نیاز به پیاده سازی خاص دارد
//            // می‌توانید از یک دیکشنری برای نگهداری ایمیل‌های هوک استفاده کنید
//        }

//        // ایجاد پیام ایمیل
//        var message = new MailMessage
//        {
//            From = new MailAddress(fromEmail, fromName),
//            Subject = subject,
//            Body = emailParams.AsPlainText ? content : $"<div style=\"font-family:Lucida Grande,Tahoma,Arial,Verdana,sans-serif;font-size:12px\">{content}</div>",
//            IsBodyHtml = !emailParams.AsPlainText
//        };

//        foreach (var email in emailAddresses)
//        {
//            message.To.Add(email);
//        }

//        message.ReplyToList.Add(new MailAddress(replyToEmail, fromName));
//        message.Sender = new MailAddress(fromEmail);
//        message.Headers.Add("X-MachForm-Origin", $"{new Uri(settings.BaseUrl).Host} {formId}");

//        // اضافه کردن BCC
//        if (!string.IsNullOrEmpty(emailParams.BccEmails))
//        {
//            foreach (var bcc in emailParams.BccEmails.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
//            {
//                message.Bcc.Add(bcc.Trim());
//            }
//        }

//        // پیوست فایل‌ها
//        if (emailParams.TargetIsAdmin)
//        {
//            var entryDetails = await MfGetEntryDetails(formId, entryId, new EntryOptions
//            {
//                StripDownloadLink = true,
//                MachformPath = emailParams.MachformBasePath
//            });

//            foreach (var data in entryDetails.Where(d => d.ElementType == "file" && d.FileData != null))
//            {
//                foreach (var fileInfo in data.FileData)
//                {
//                    if (settings.StoreFilesAsBlob)
//                    {
//                        var fileContent = await MfReadFormFilesBlob(formId, fileInfo.FilenamePath);
//                        if (fileContent != null)
//                        {
//                            message.Attachments.Add(new Attachment(new MemoryStream(fileContent), fileInfo.FilenameValue));
//                        }
//                    }
//                    else
//                    {
//                        if (File.Exists(fileInfo.FilenamePath))
//                        {
//                            message.Attachments.Add(new Attachment(fileInfo.FilenamePath, fileInfo.FilenameValue));
//                        }
//                    }
//                }
//            }
//        }

//        // پیوست PDF
//        if (emailParams.PdfEnable)
//        {
//            var pdfContent = emailParams.PdfContent ?? "{entry_data}";
//            if (!pdfContent.Contains("<html>"))
//            {
//                pdfContent = $"<html><body>{pdfContent}</body></html>";
//            }

//            var pdfTemplateData = await MfGetTemplateVariables(formId, entryId, new TemplateDataOptions
//            {
//                AsPlainText = false,
//                TargetIsAdmin = emailParams.TargetIsAdmin,
//                MachformPath = emailParams.MachformBasePath,
//                ShowImagePreview = true,
//                UseListLayout = true,
//                StripDownloadLink = settings.DisablePdfLink
//            });

//            pdfContent = ReplaceTemplateVariables(pdfContent, pdfTemplateData.Variables, pdfTemplateData.Values);

//            // تولید PDF
//            var tempDir = Path.Combine(settings.UploadDir, "temp");
//            if (!Directory.Exists(tempDir))
//            {
//                Directory.CreateDirectory(tempDir);
//                File.WriteAllText(Path.Combine(tempDir, "index.html"), " ");
//            }

//            var fileToken = Guid.NewGuid().ToString("N");
//            var pdfFilename = Path.Combine(tempDir, $"data_{formId}_{entryId}_{fileToken}.pdf");

//            using (var writer = new PdfWriter(pdfFilename))
//            {
//                using (var pdf = new PdfDocument(writer))
//                {
//                    var converter = new ConverterProperties();
//                    HtmlConverter.ConvertToPdf(pdfContent, pdf, converter);
//                }
//            }

//            if (File.Exists(pdfFilename))
//            {
//                var pdfAttachmentName = new string(subject.Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray());
//                message.Attachments.Add(new Attachment(pdfFilename, $"{pdfAttachmentName}.pdf"));
//            }
//        }

//        // ارسال ایمیل
//        using (var smtpClient = new SmtpClient())
//        {
//            if (settings.SmtpEnable)
//            {
//                smtpClient.Host = settings.SmtpHost;
//                smtpClient.Port = settings.SmtpPort;
//                smtpClient.EnableSsl = settings.SmtpSecure;

//                if (settings.SmtpAuth)
//                {
//                    smtpClient.Credentials = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword);
//                }
//            }

//            await smtpClient.SendMailAsync(message);
//        }

//        // حذف فایل موقت PDF
//        if (emailParams.PdfEnable && File.Exists(pdfFilename))
//        {
//            File.Delete(pdfFilename);
//        }
//    }

//    // توابع کمکی
//    private string ReplaceTemplateVariables(string input, string[] variables, string[] values)
//    {
//        if (string.IsNullOrEmpty(input))
//            return input;

//        for (int i = 0; i < variables.Length; i++)
//        {
//            input = input.Replace(variables[i], values[i]);
//        }

//        return input.Replace("&nbsp;", "").Trim();
//    }

//    private async Task<TemplateData> MfGetTemplateVariables(int formId, int entryId, TemplateDataOptions options)
//    {
//        // پیاده سازی دریافت متغیرهای تمپلیت از دیتابیس
//        return new TemplateData();
//    }

//    private async Task<List<EntryDetail>> MfGetEntryDetails(int formId, int entryId, EntryOptions options)
//    {
//        // پیاده سازی دریافت جزئیات ورودی از دیتابیس
//        return new List<EntryDetail>();
//    }

//    private async Task<byte[]> MfReadFormFilesBlob(int formId, string filePath)
//    {
//        // پیاده سازی خواندن فایل‌های باینری از دیتابیس
//        return null;
//    }
//}

//// کلاس‌های مدل
//public class ApplicationDbContext : DbContext
//{
//    public DbSet<WebhookOption> WebhookOptions { get; set; }
//    public DbSet<WebhookParameter> WebhookParameters { get; set; }
//    public DbSet<Setting> Settings { get; set; }
//    public DbSet<Form> Forms { get; set; }
//    // سایر DbSet ها
//}

//public class WebhookOption
//{
//    public int Id { get; set; }
//    public int FormId { get; set; }
//    public int RuleId { get; set; }
//    public string WebhookUrl { get; set; }
//    public string WebhookMethod { get; set; }
//    public string WebhookFormat { get; set; }
//    public string WebhookRawData { get; set; }
//    public bool EnableHttpAuth { get; set; }
//    public string HttpUsername { get; set; }
//    public string HttpPassword { get; set; }
//    public bool EnableCustomHttpHeaders { get; set; }
//    public string CustomHttpHeaders { get; set; }
//}

//public class WebhookParameter
//{
//    public int AwpId { get; set; }
//    public int FormId { get; set; }
//    public int RuleId { get; set; }
//    public string ParamName { get; set; }
//    public string ParamValue { get; set; }
//}

//public class Setting
//{
//    public int Id { get; set; }
//    public string BaseUrl { get; set; }
//    public string DefaultFromName { get; set; }
//    public string DefaultFromEmail { get; set; }
//    public string UploadDir { get; set; }
//    public bool SmtpEnable { get; set; }
//    public string SmtpHost { get; set; }
//    public int SmtpPort { get; set; }
//    public bool SmtpSecure { get; set; }
//    public bool SmtpAuth { get; set; }
//    public string SmtpUsername { get; set; }
//    public string SmtpPassword { get; set; }
//    public bool StoreFilesAsBlob { get; set; }
//    public bool DisablePdfLink { get; set; }
//}

//public class Form
//{
//    public int FormId { get; set; }
//    public string FormEmail { get; set; }
//    public bool FormEntryEditEnable { get; set; }
//    public bool FormEntryEditHideEditlink { get; set; }
//    // سایر خصوصیات فرم
//}

//public class TemplateData
//{
//    public string[] Variables { get; set; }
//    public string[] Values { get; set; }
//}

//public class TemplateDataOptions
//{
//    public bool AsPlainText { get; set; }
//    public bool TargetIsAdmin { get; set; }
//    public string MachformPath { get; set; }
//    public bool StripDownloadLink { get; set; }
//    public bool ShowImagePreview { get; set; }
//    public bool UseListLayout { get; set; }
//}

//public class EntryDetail
//{
//    public string ElementType { get; set; }
//    public List<FileInfo> FileData { get; set; }
//}

//public class FileInfo
//{
//    public string FilenamePath { get; set; }
//    public string FilenameValue { get; set; }
//}

//public class EntryOptions
//{
//    public bool StripDownloadLink { get; set; }
//    public string MachformPath { get; set; }
//}

//public class EmailParameters
//{
//    public string FromName { get; set; }
//    public string FromEmail { get; set; }
//    public string ReplyToEmail { get; set; }
//    public string BccEmails { get; set; }
//    public string Subject { get; set; }
//    public string Content { get; set; }
//    public bool AsPlainText { get; set; }
//    public bool TargetIsAdmin { get; set; }
//    public bool CheckHookFile { get; set; }
//    public bool PdfEnable { get; set; }
//    public string PdfContent { get; set; }
//    public string MachformBasePath { get; set; }
//}






//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Data.Entity;
//using System.IO;
//using System.Net;
//using System.Web;
//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;
//using System.Net.Mail;

//namespace MachForm.NetCore.Services.Forms;

//public class FormService
//{
//    private readonly AppDbContext _dbContext;
//    private readonly AppSettings _mfSettings;
//    private readonly LanguageResources _mfLang;

//    public FormService(AppDbContext dbContext, AppSettings mfSettings, LanguageResources mfLang)
//    {
//        _dbContext = dbContext;
//        _mfSettings = mfSettings;
//        _mfLang = mfLang;
//    }

//    // معادل تابع mf_get_form_properties
//    public Dictionary<string, object> GetFormProperties(int formId, string[] columns = null)
//    {
//        var formProperties = new Dictionary<string, object>();

//        if (columns != null && columns.Length > 0)
//        {
//            var columnList = string.Join(",", columns);
//            var query = $"SELECT {columnList} FROM {MFTablePrefix}forms WHERE form_id = @formId";

//            var form = _dbContext.Database.SqlQuery<dynamic>(query, new SqlParameter("@formId", formId)).FirstOrDefault();

//            foreach (var column in columns)
//            {
//                if (form != null && form.GetType().GetProperty(column) != null)
//                {
//                    formProperties[column] = form.GetType().GetProperty(column).GetValue(form, null);
//                }
//            }
//        }
//        else
//        {
//            // اگر آرایه ستون‌ها مشخص نشده باشد، همه ستون‌های جدول را می‌گیریم
//            var form = _dbContext.Forms.FirstOrDefault(f => f.form_id == formId);
//            if (form != null)
//            {
//                var properties = form.GetType().GetProperties();
//                foreach (var prop in properties)
//                {
//                    if (prop.Name != "form_id" && prop.Name != "form_name")
//                    {
//                        formProperties[prop.Name] = prop.GetValue(form, null);
//                    }
//                }
//            }
//        }

//        return formProperties;
//    }

//    // معادل تابع mf_get_template_variables
//    public TemplateData GetTemplateVariables(int formId, int entryId, TemplateOptions options = null)
//    {
//        options = options ?? new TemplateOptions();

//        var entryOptions = new EntryOptions
//        {
//            StripDownloadLink = options.StripDownloadLink,
//            StripCheckboxImage = true,
//            MachformPath = options.MachformPath ?? "",
//            ShowImagePreview = options.ShowImagePreview,
//            TargetIsAdmin = options.TargetIsAdmin,
//            HideEncryptedData = options.HideEncryptedData,
//            HidePasswordData = options.HidePasswordData,
//            EncryptionPrivateKey = options.EncryptionPrivateKey
//        };

//        // دریافت جزئیات ورودی
//        var entryDetails = GetEntryDetails(formId, entryId, entryOptions);

//        // دریافت اطلاعات پرداخت
//        var form = _dbContext.Forms.FirstOrDefault(f => f.form_id == formId);
//        if (form != null)
//        {
//            var paymentEnabled = form.payment_enable_merchant == 1;
//            var paymentAmount = (double)form.payment_price_amount;
//            var paymentMerchantType = form.payment_merchant_type;
//            var paymentPriceType = form.payment_price_type;
//            var paymentCurrency = form.payment_currency?.ToUpper();
//            var askBilling = form.payment_ask_billing == 1;
//            var askShipping = form.payment_ask_shipping == 1;
//            var enableTax = form.payment_enable_tax == 1;
//            var taxRate = (float)form.payment_tax_rate;
//            var enableDiscount = form.payment_enable_discount == 1;
//            var discountType = form.payment_discount_type;
//            var discountAmount = (float)form.payment_discount_amount;
//            var discountElementId = form.payment_discount_element_id;
//            var pageTotal = form.form_page_total;
//            var logicEnabled = form.logic_field_enable == 1;
//            var resumeEnabled = form.form_resume_enable == 1;
//            var approvalEnabled = form.form_approval_enable == 1;
//            var editEnabled = form.form_entry_edit_enable == 1;

//            // بررسی تخفیف
//            var isDiscountApplicable = false;
//            if (enableDiscount && paymentEnabled && discountElementId > 0)
//            {
//                var couponValue = _dbContext.Database.SqlQuery<string>(
//                    $"SELECT element_{discountElementId} FROM {MFTablePrefix}form_{formId} WHERE id = @entryId AND status = 1",
//                    new SqlParameter("@entryId", entryId)).FirstOrDefault();

//                isDiscountApplicable = !string.IsNullOrEmpty(couponValue);
//            }

//            if (paymentEnabled)
//            {
//                var payment = _dbContext.FormPayments
//                    .Where(p => p.form_id == formId && p.record_id == entryId && p.status == 1)
//                    .OrderByDescending(p => p.payment_date)
//                    .FirstOrDefault();

//                if (payment != null)
//                {
//                    var paymentId = payment.payment_id;
//                    var paymentDate = payment.payment_date.ToString("dd MMM yyyy - hh:mm tt");
//                    var paymentStatus = payment.payment_status;
//                    var paymentFullname = payment.payment_fullname;
//                    var paymentAmount = (double)payment.payment_amount;
//                    var paymentCurrency = payment.payment_currency?.ToUpper();
//                    var paymentTestMode = payment.payment_test_mode;
//                    var paymentMerchantType = payment.payment_merchant_type;

//                    var billingStreet = WebUtility.HtmlEncode(payment.billing_street?.Trim());
//                    var billingCity = WebUtility.HtmlEncode(payment.billing_city?.Trim());
//                    var billingState = WebUtility.HtmlEncode(payment.billing_state?.Trim());
//                    var billingZipcode = WebUtility.HtmlEncode(payment.billing_zipcode?.Trim());
//                    var billingCountry = WebUtility.HtmlEncode(payment.billing_country?.Trim());

//                    string shippingStreet, shippingCity, shippingState, shippingZipcode, shippingCountry;

//                    if (payment.same_shipping_address == 1)
//                    {
//                        shippingStreet = billingStreet;
//                        shippingCity = billingCity;
//                        shippingState = billingState;
//                        shippingZipcode = billingZipcode;
//                        shippingCountry = billingCountry;
//                    }
//                    else
//                    {
//                        shippingStreet = WebUtility.HtmlEncode(payment.shipping_street?.Trim());
//                        shippingCity = WebUtility.HtmlEncode(payment.shipping_city?.Trim());
//                        shippingState = WebUtility.HtmlEncode(payment.shipping_state?.Trim());
//                        shippingZipcode = WebUtility.HtmlEncode(payment.shipping_zipcode?.Trim());
//                        shippingCountry = WebUtility.HtmlEncode(payment.shipping_country?.Trim());
//                    }

//                    string billingAddress = null, shippingAddress = null;

//                    if (!string.IsNullOrEmpty(billingStreet) || !string.IsNullOrEmpty(billingCity) ||
//                        !string.IsNullOrEmpty(billingState) || !string.IsNullOrEmpty(billingZipcode) ||
//                        !string.IsNullOrEmpty(billingCountry))
//                    {
//                        billingAddress = $"{billingStreet}<br />{billingCity}, {billingState} {billingZipcode}<br />{billingCountry}";
//                    }

//                    if (!string.IsNullOrEmpty(shippingStreet) || !string.IsNullOrEmpty(shippingCity) ||
//                        !string.IsNullOrEmpty(shippingState) || !string.IsNullOrEmpty(shippingZipcode) ||
//                        !string.IsNullOrEmpty(shippingCountry))
//                    {
//                        shippingAddress = $"{shippingStreet}<br />{shippingCity}, {shippingState} {shippingZipcode}<br />{shippingCountry}";
//                    }
//                }
//                else
//                {
//                    // محاسبه مبلغ پرداخت اگر رکوردی وجود نداشته باشد
//                    paymentStatus = "unpaid";

//                    if (paymentPriceType == "variable")
//                    {
//                        paymentAmount = GetPaymentTotal(formId, entryId, 0, "live");
//                    }
//                    else if (paymentPriceType == "fixed")
//                    {
//                        paymentAmount = paymentAmount;
//                    }

//                    // محاسبه تخفیف اگر قابل اعمال باشد
//                    if (isDiscountApplicable)
//                    {
//                        var calculatedDiscount = 0.0;

//                        if (discountType == "percent_off")
//                        {
//                            calculatedDiscount = (discountAmount / 100) * paymentAmount;
//                            calculatedDiscount = Math.Round(calculatedDiscount, 2);
//                        }
//                        else
//                        {
//                            calculatedDiscount = Math.Round(discountAmount, 2);
//                        }

//                        paymentAmount -= calculatedDiscount;
//                    }

//                    // محاسبه مالیات اگر فعال باشد
//                    if (enableTax && taxRate > 0)
//                    {
//                        var taxAmount = (taxRate / 100) * paymentAmount;
//                        taxAmount = Math.Round(taxAmount, 2);
//                        paymentAmount += taxAmount;
//                    }

//                    paymentCurrency = paymentCurrency;
//                }

//                // تعیین نماد ارز
//                var currencySymbol = GetCurrencySymbol(paymentCurrency, options.AsPlainText);

//                // قالب‌بندی مبلغ پرداخت
//                paymentAmount = Math.Round(paymentAmount, 2);
//                var totalPaymentAmount = $"{currencySymbol}{paymentAmount} {paymentCurrency}";

//                // افزودن اطلاعات پرداخت به جزئیات ورودی
//                var totalEntryDetails = entryDetails.Count;

//                // افزودن ردیف خالی به عنوان جداکننده
//                entryDetails.Add(new EntryDetail
//                {
//                    Value = options.AsPlainText ? "" : "&nbsp;&nbsp;",
//                    Label = options.AsPlainText ? "" : "&nbsp;&nbsp;"
//                });

//                // افزودن مبلغ کل
//                entryDetails.Add(new EntryDetail
//                {
//                    Value = totalPaymentAmount,
//                    Label = _mfLang["payment_total"]
//                });

//                // افزودن وضعیت پرداخت (اگر وضعیت "unpaid" نباشد)
//                if (paymentStatus != "unpaid")
//                {
//                    entryDetails.Add(new EntryDetail
//                    {
//                        Value = paymentTestMode == 1 ? $"{paymentStatus.ToUpper()} (TEST mode)" : paymentStatus.ToUpper(),
//                        Label = _mfLang["payment_status"]
//                    });
//                }
//            }
//        }

//        // ایجاد متغیرهای قالب
//        var templateVariables = new List<string>();
//        var templateValues = new List<string>();
//        var fileUrl = "";

//        for (int i = 0; i < entryDetails.Count; i++)
//        {
//            var data = entryDetails[i];

//            if (data.ElementId == 0)
//                continue;

//            if (options.AsPlainText)
//            {
//                data.Value = WebUtility.HtmlDecode(data.Value);
//                data.Value = data.Value.Replace("<br />", "\n");

//                if (data.ElementType == "file")
//                {
//                    data.Value = data.Value.Replace("<br/>", ",").Trim(',');

//                    // استخراج URL فایل‌ها
//                    var fileUrls = data.Value.Split(',')
//                        .Select(fileTag =>
//                        {
//                            var match = Regex.Match(fileTag, @"^<a.*?href=([""'])(.*?)\1.*$");
//                            return match.Success ? match.Groups[2].Value : "";
//                        })
//                        .Where(url => !string.IsNullOrEmpty(url));

//                    fileUrl = string.Join(",", fileUrls);
//                }
//                else if (data.ElementType == "money")
//                {
//                    var moneySymbols = new Dictionary<string, string>
//                    {
//                        {"&#165;", "¥"}, {"&#163;", "£"}, {"&#8364;", "€"}, {"&#3647;", "฿"},
//                        {"&#75;&#269;", "Kč"}, {"&#122;&#322;", "zł"}, {"&#65020;", "﷼"}
//                    };

//                    foreach (var symbol in moneySymbols)
//                    {
//                        data.Value = data.Value.Replace(symbol.Key, symbol.Value);
//                    }

//                    data.Value = data.Value.Replace("&nbsp;", "").Trim();
//                    data.Value = StripHtmlTags(data.Value);
//                }
//                else
//                {
//                    data.Value = data.Value.Replace("&nbsp;", "").Trim();
//                    data.Value = StripHtmlTags(data.Value);
//                }
//            }

//            templateVariables.Add($"{{element_{data.ElementId}}}");
//            templateValues.Add(data.Value);

//            if (data.ElementType == "textarea" && !options.AsPlainText)
//            {
//                templateValues[i] = data.Value.Replace("\n", "<br />");
//            }
//            else if (data.ElementType == "file")
//            {
//                if (!options.AsPlainText)
//                {
//                    templateValues[i] = StripHtmlTags(data.Value, "<a><br><img>");
//                }
//                else
//                {
//                    templateValues[i] = StripHtmlTags(data.Value);
//                    templateValues[i] = templateValues[i].Replace("&nbsp;", "\n- ");
//                }

//                // افزودن متغیر URL مستقیم
//                templateVariables.Add($"{{element_{data.ElementId}_url}}");
//                templateValues.Add(fileUrl);
//            }
//            else if (data.ElementType == "signature")
//            {
//                if (string.IsNullOrEmpty(data.Value))
//                {
//                    templateVariables.Add($"{{element_{data.ElementId}}}");
//                    templateValues.Add("");
//                }
//                else
//                {
//                    continue;
//                }
//            }
//        }

//        // دریافت مقادیر ورودی
//        var entryValues = GetEntryValues(formId, entryId);

//        // دریافت متغیرهای قالب برای فیلدهای پیچیده (نام، آدرس، تاریخ)
//        var complexElements = _dbContext.FormElements
//            .Where(e => e.form_id == formId &&
//                        e.element_type != "section" &&
//                        e.element_type != "media" &&
//                        e.element_status == 1 &&
//                        new[] { "simple_name", "simple_name_wmiddle", "name", "name_wmiddle", "address", "date", "europe_date" }.Contains(e.element_type))
//            .OrderBy(e => e.element_position)
//            .ToList();

//        foreach (var element in complexElements)
//        {
//            var elementId = element.element_id;
//            var elementType = element.element_type;

//            if (elementType == "simple_name")
//            {
//                for (int j = 1; j <= 2; j++)
//                {
//                    templateVariables.Add($"{{element_{elementId}_{j}}}");
//                    templateValues.Add(entryValues[$"element_{elementId}_{j}"]?.DefaultValue ?? "");
//                }
//            }
//            else if (elementType == "simple_name_wmiddle")
//            {
//                for (int j = 1; j <= 3; j++)
//                {
//                    templateVariables.Add($"{{element_{elementId}_{j}}}");
//                    templateValues.Add(entryValues[$"element_{elementId}_{j}"]?.DefaultValue ?? "");
//                }
//            }
//            else if (elementType == "name")
//            {
//                for (int j = 1; j <= 4; j++)
//                {
//                    templateVariables.Add($"{{element_{elementId}_{j}}}");
//                    templateValues.Add(entryValues[$"element_{elementId}_{j}"]?.DefaultValue ?? "");
//                }
//            }
//            else if (elementType == "name_wmiddle")
//            {
//                for (int j = 1; j <= 5; j++)
//                {
//                    templateVariables.Add($"{{element_{elementId}_{j}}}");
//                    templateValues.Add(entryValues[$"element_{elementId}_{j}"]?.DefaultValue ?? "");
//                }
//            }
//            else if (elementType == "address")
//            {
//                for (int j = 1; j <= 6; j++)
//                {
//                    templateVariables.Add($"{{element_{elementId}_{j}}}");
//                    templateValues.Add(entryValues[$"element_{elementId}_{j}"]?.DefaultValue ?? "");
//                }
//            }
//            else if (elementType == "date" || elementType == "europe_date")
//            {
//                if (elementType == "date") // mm-dd-yyyy
//                {
//                    templateVariables.Add($"{{element_{elementId}_mm}}");
//                    templateValues.Add(entryValues[$"element_{elementId}_1"]?.DefaultValue ?? "");

//                    templateVariables.Add($"{{element_{elementId}_dd}}");
//                    templateValues.Add(entryValues[$"element_{elementId}_2"]?.DefaultValue ?? "");

//                    templateVariables.Add($"{{element_{elementId}_yyyy}}");
//                    templateValues.Add(entryValues[$"element_{elementId}_3"]?.DefaultValue ?? "");
//                }
//                else if (elementType == "europe_date") // dd-mm-yyyy
//                {
//                    templateVariables.Add($"{{element_{elementId}_dd}}");
//                    templateValues.Add(entryValues[$"element_{elementId}_1"]?.DefaultValue ?? "");

//                    templateVariables.Add($"{{element_{elementId}_mm}}");
//                    templateValues.Add(entryValues[$"element_{elementId}_2"]?.DefaultValue ?? "");

//                    templateVariables.Add($"{{element_{elementId}_yyyy}}");
//                    templateValues.Add(entryValues[$"element_{elementId}_3"]?.DefaultValue ?? "");
//                }
//            }
//        }

//        // دریافت اطلاعات زمان ایجاد ورودی و آدرس IP
//        var entry = _dbContext.Database.SqlQuery<dynamic>(
//            $"SELECT date_created, ip_address FROM {MFTablePrefix}form_{formId} WHERE id = @entryId",
//            new SqlParameter("@entryId", entryId)).FirstOrDefault();

//        var dateCreated = entry?.date_created.ToString();
//        var ipAddress = entry?.ip_address;

//        // دریافت نام فرم
//        var formName = _dbContext.Forms
//            .Where(f => f.form_id == formId)
//            .Select(f => f.form_name)
//            .FirstOrDefault();

//        // افزودن متغیرهای عمومی
//        templateVariables.Add("{date_created}");
//        templateValues.Add(dateCreated ?? "");

//        templateVariables.Add("{ip_address}");
//        templateValues.Add(ipAddress ?? "");

//        templateVariables.Add("{form_name}");
//        templateValues.Add(formName ?? "");

//        templateVariables.Add("{entry_no}");
//        templateValues.Add(entryId.ToString());

//        templateVariables.Add("{form_id}");
//        templateValues.Add(formId.ToString());

//        // ایجاد محتوای ایمیل
//        var emailBody = ComposeEmailBody(entryDetails, options);

//        // افزودن متغیر entry_data
//        templateVariables.Add("{entry_data}");
//        templateValues.Add(emailBody);

//        return new TemplateData
//        {
//            Variables = templateVariables.ToArray(),
//            Values = templateValues.ToArray(),
//            FilesToAttach = GetFilesToAttach(entryDetails)
//        };
//    }

//    // معادل تابع mf_parse_template_variables
//    public string ParseTemplateVariables(int formId, int entryId, string templateContent)
//    {
//        var options = new TemplateOptions
//        {
//            StripDownloadLink = false,
//            AsPlainText = true,
//            TargetIsAdmin = true,
//            MachformPath = _mfSettings.BaseUrl
//        };

//        var templateData = GetTemplateVariables(formId, entryId, options);

//        for (int i = 0; i < templateData.Variables.Length; i++)
//        {
//            templateContent = templateContent.Replace(templateData.Variables[i], templateData.Values[i]);
//        }

//        return templateContent;
//    }

//    // معادل تابع mf_get_country_code
//    public string GetCountryCode(string countryName)
//    {
//        var countries = new Dictionary<string, string>
//        {
//            {"United States", "US"},
//            {"United Kingdom", "GB"},
//            {"Canada", "CA"},
//            {"Australia", "AU"},
//            // ... سایر کشورها
//        };

//        return countries.TryGetValue(countryName, out var code) ? code : null;
//    }

//    // معادل تابع mf_trim_max_length
//    public string TrimMaxLength(string text, int maxLength)
//    {
//        if (string.IsNullOrEmpty(text)) return text;

//        return text.Length > maxLength + 3 ?
//            text.Substring(0, maxLength) + "..." :
//            text;
//    }

//    // معادل تابع mf_array_insert
//    public void ArrayInsert<T>(ref T[] array, int position, T[] insertArray)
//    {
//        var firstPart = array.Take(position).ToArray();
//        var lastPart = array.Skip(position).ToArray();

//        array = firstPart.Concat(insertArray).Concat(lastPart).ToArray();
//    }

//    // معادل تابع mf_is_whitelisted_ip_address
//    public bool IsWhitelistedIpAddress(string clientIpAddress)
//    {
//        if (string.IsNullOrEmpty(_mfSettings.IpWhitelist))
//            return true;

//        var whitelist = _mfSettings.IpWhitelist.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

//        foreach (var allowedIp in whitelist)
//        {
//            var trimmedIp = allowedIp.Trim();

//            if (!trimmedIp.Contains("*"))
//            {
//                if (clientIpAddress == trimmedIp)
//                    return true;
//            }
//            else
//            {
//                var allowedParts = trimmedIp.Split('.');
//                var clientParts = clientIpAddress.Split('.');

//                if (allowedParts.Length != clientParts.Length)
//                    continue;

//                bool match = true;
//                for (int i = 0; i < allowedParts.Length; i++)
//                {
//                    if (allowedParts[i] != "*" && allowedParts[i] != clientParts[i])
//                    {
//                        match = false;
//                        break;
//                    }
//                }

//                if (match)
//                    return true;
//            }
//        }

//        return false;
//    }

//    // متدهای کمکی
//    private string StripHtmlTags(string input, string allowedTags = "")
//    {
//        if (string.IsNullOrEmpty(input))
//            return input;

//        // پیاده‌سازی ساده برای حذف تگ‌های HTML
//        if (string.IsNullOrEmpty(allowedTags))
//        {
//            return Regex.Replace(input, "<.*?>", string.Empty);
//        }
//        else
//        {
//            // پیاده‌سازی پیچیده‌تر برای حفظ تگ‌های مجاز
//            // این یک پیاده‌سازی ساده است و ممکن است نیاز به بهبود داشته باشد
//            var pattern = $@"<(?!({allowedTags})\b)[^>]*>";
//            return Regex.Replace(input, pattern, string.Empty, RegexOptions.IgnoreCase);
//        }
//    }

//    private string GetCurrencySymbol(string currencyCode, bool asPlainText)
//    {
//        var symbols = new Dictionary<string, string[]>
//        {
//            {"USD", new[] {"&#36;", "$"}},
//            {"EUR", new[] {"&#8364;", "€"}},
//            {"GBP", new[] {"&#163;", "£"}},
//            // ... سایر ارزها
//        };

//        if (symbols.TryGetValue(currencyCode, out var symbol))
//        {
//            return asPlainText ? symbol[1] : symbol[0];
//        }

//        return "";
//    }
//}

//// کلاس‌های مدل
//public class AppDbContext : DbContext
//{
//    public DbSet<Form> Forms { get; set; }
//    public DbSet<FormElement> FormElements { get; set; }
//    public DbSet<FormPayment> FormPayments { get; set; }
//    // ... سایر DbSetها
//}

//public class Form
//{
//    public int form_id { get; set; }
//    public string form_name { get; set; }
//    public int payment_enable_merchant { get; set; }
//    public string payment_merchant_type { get; set; }
//    public string payment_price_type { get; set; }
//    public decimal payment_price_amount { get; set; }
//    public string payment_currency { get; set; }
//    // ... سایر خصوصیات
//}

//public class FormElement
//{
//    public int element_id { get; set; }
//    public int form_id { get; set; }
//    public string element_type { get; set; }
//    public int element_status { get; set; }
//    public int element_position { get; set; }
//    // ... سایر خصوصیات
//}

//public class FormPayment
//{
//    public string payment_id { get; set; }
//    public DateTime payment_date { get; set; }
//    public string payment_status { get; set; }
//    public string payment_fullname { get; set; }
//    public decimal payment_amount { get; set; }
//    public string payment_currency { get; set; }
//    // ... سایر خصوصیات
//}

//// کلاس‌های کمکی
//public class TemplateData
//{
//    public string[] Variables { get; set; }
//    public string[] Values { get; set; }
//    public List<FileAttachment> FilesToAttach { get; set; }
//}

//public class TemplateOptions
//{
//    public bool AsPlainText { get; set; } = false;
//    public bool TargetIsAdmin { get; set; } = false;
//    public bool UseListLayout { get; set; } = false;
//    public bool StripDownloadLink { get; set; } = false;
//    public string MachformPath { get; set; } = "";
//    public bool ShowImagePreview { get; set; } = false;
//    public bool HideEncryptedData { get; set; } = false;
//    public bool HidePasswordData { get; set; } = false;
//    public string EncryptionPrivateKey { get; set; } = "";
//}

//public class EntryOptions
//{
//    public bool StripDownloadLink { get; set; }
//    public bool StripCheckboxImage { get; set; }
//    public string MachformPath { get; set; }
//    public bool ShowImagePreview { get; set; }
//    public bool TargetIsAdmin { get; set; }
//    public bool HideEncryptedData { get; set; }
//    public bool HidePasswordData { get; set; }
//    public string EncryptionPrivateKey { get; set; }
//}

//public class EntryDetail
//{
//    public int ElementId { get; set; }
//    public string ElementType { get; set; }
//    public string Label { get; set; }
//    public string Value { get; set; }
//    public List<FileInfo> Filedata { get; set; }
//    // ... سایر خصوصیات
//}

//public class FileAttachment
//{
//    public string FilePath { get; set; }
//    public string FileName { get; set; }
//}

//public class AppSettings
//{
//    public string BaseUrl { get; set; }
//    public string IpWhitelist { get; set; }
//    // ... سایر تنظیمات
//}

//public class LanguageResources : Dictionary<string, string>
//{
//    // منابع زبانی
//}











//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Mail;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Web;
//using System.Data.Entity;

//public class FormService
//{
//    private readonly AppDbContext _dbContext;
//    private readonly AppSettings _mfSettings;
//    private readonly LanguageResources _mfLang;

//    public FormService(AppDbContext dbContext, AppSettings mfSettings, LanguageResources mfLang)
//    {
//        _dbContext = dbContext;
//        _mfSettings = mfSettings;
//        _mfLang = mfLang;
//    }

//    // معادل تابع mf_send_resume_link
//    public void SendResumeLink(int formId, int entryId, string resumeUrl, string resumeEmail)
//    {
//        // دریافت تنظیمات فرم
//        var formProperties = GetFormProperties(formId, new[] {
//            "form_name", "form_language", "form_resume_subject",
//            "form_resume_content", "form_resume_from_name", "form_resume_from_email_address"
//        });

//        // تنظیم زبان
//        SetLanguage(formProperties["form_language"]);

//        // تنظیم مقادیر پیش‌فرض در صورت خالی بودن تنظیمات
//        if (string.IsNullOrEmpty(formProperties["form_resume_subject"]))
//            formProperties["form_resume_subject"] = _mfLang["resume_email_subject"];

//        if (string.IsNullOrEmpty(formProperties["form_resume_content"]))
//            formProperties["form_resume_content"] = _mfLang["resume_email_content"];

//        if (string.IsNullOrEmpty(formProperties["form_resume_from_name"]))
//            formProperties["form_resume_from_name"] = WebUtility.HtmlDecode(_mfSettings.DefaultFromName);

//        if (string.IsNullOrEmpty(formProperties["form_resume_from_email_address"]))
//            formProperties["form_resume_from_email_address"] = _mfSettings.DefaultFromEmail;

//        // تجزیه متغیرهای قالب
//        formProperties["form_resume_subject"] = ParseTemplateVariables(formId, entryId, formProperties["form_resume_subject"]);
//        formProperties["form_resume_content"] = ParseTemplateVariables(formId, entryId, formProperties["form_resume_content"]);

//        // جایگزینی {resume_url}
//        formProperties["form_resume_content"] = formProperties["form_resume_content"].Replace("{resume_url}", resumeUrl);

//        // ارسال ایمیل
//        if (!string.IsNullOrEmpty(resumeEmail) && !string.IsNullOrEmpty(resumeUrl))
//        {
//            SendEmail(
//                formProperties["form_resume_from_email_address"],
//                formProperties["form_resume_from_name"],
//                resumeEmail,
//                formProperties["form_resume_subject"],
//                formProperties["form_resume_content"],
//                isHtml: true
//            );
//        }
//    }

//    // معادل تابع mf_send_login_info
//    public void SendLoginInfo(int userId, string password)
//    {
//        // دریافت اطلاعات کاربر
//        var user = _dbContext.Users
//            .Where(u => u.UserId == userId && u.Status == 1)
//            .Select(u => new { u.UserFullname, u.UserEmail })
//            .FirstOrDefault();

//        if (user == null) return;

//        var subject = "Your MachForm login information";
//        var emailTemplate = @"Hello {0},

//You can login to MachForm panel using the following information:

//<b>URL:</b> {1}

//<b>Email:</b> {2}
//<b>Password:</b> {3}

//Thank you.";

//        var emailContent = string.Format(emailTemplate,
//            user.UserFullname,
//            _mfSettings.BaseUrl,
//            user.UserEmail,
//            password);

//        emailContent = emailContent.Replace("\n", "<br />");

//        SendEmail(
//            _mfSettings.DefaultFromEmail,
//            WebUtility.HtmlDecode(_mfSettings.DefaultFromName),
//            user.UserEmail,
//            subject,
//            emailContent,
//            isHtml: true
//        );
//    }

//    // معادل تابع mf_get_ssl_suffix
//    public string GetSslSuffix()
//    {
//        if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTPS"]) &&
//            HttpContext.Current.Request.ServerVariables["HTTPS"].ToLower() != "off")
//        {
//            return "s";
//        }
//        else if (HttpContext.Current.Request.Headers["X-Forwarded-Proto"]?.ToLower() == "https")
//        {
//            return "s";
//        }
//        else if (HttpContext.Current.Request.Headers["Front-End-Https"]?.ToLower() == "on")
//        {
//            return "s";
//        }

//        return "";
//    }

//    // معادل تابع mf_get_dirname
//    public string GetDirname(string path)
//    {
//        var currentDir = System.IO.Path.GetDirectoryName(path);
//        return (currentDir == "/" || currentDir == "\\") ? "" : currentDir;
//    }

//    // معادل تابع mf_get_settings
//    public AppSettings GetSettings()
//    {
//        return _dbContext.Settings.FirstOrDefault();
//    }

//    // معادل تابع mf_format_bytes
//    public string FormatBytes(long bytes)
//    {
//        if (bytes < 1024) return $"{bytes} B";
//        else if (bytes < 1048576) return $"{Math.Round(bytes / 1024.0, 2)} KB";
//        else if (bytes < 1073741824) return $"{Math.Round(bytes / 1048576.0, 2)} MB";
//        else if (bytes < 1099511627776) return $"{Math.Round(bytes / 1073741824.0, 2)} GB";
//        else return $"{Math.Round(bytes / 1099511627776.0, 2)} TB";
//    }

//    // معادل تابع mf_trim_value
//    public void TrimValue(ref string value)
//    {
//        value = value?.Trim();
//    }

//    // معادل تابع mf_strtolower_value
//    public void StrToLowerValue(ref string value)
//    {
//        value = value?.ToLower();
//    }

//    // متدهای کمکی برای ارسال ایمیل
//    private void SendEmail(string fromEmail, string fromName, string toEmail, string subject, string body, bool isHtml = false)
//    {
//        var message = new MailMessage
//        {
//            From = new MailAddress(fromEmail, fromName),
//            Subject = subject,
//            Body = body,
//            IsBodyHtml = isHtml,
//            BodyEncoding = Encoding.UTF8,
//            SubjectEncoding = Encoding.UTF8
//        };
//        message.To.Add(toEmail);

//        if (_mfSettings.SmtpEnable)
//        {
//            using (var smtpClient = new SmtpClient(_mfSettings.SmtpHost, _mfSettings.SmtpPort))
//            {
//                if (!string.IsNullOrEmpty(_mfSettings.SmtpSecure))
//                {
//                    smtpClient.EnableSsl = true;
//                }

//                if (!string.IsNullOrEmpty(_mfSettings.SmtpAuth))
//                {
//                    smtpClient.Credentials = new NetworkCredential(
//                        _mfSettings.SmtpUsername,
//                        _mfSettings.SmtpPassword
//                    );
//                }

//                smtpClient.Send(message);
//            }
//        }
//        else
//        {
//            using (var smtpClient = new SmtpClient())
//            {
//                smtpClient.Send(message);
//            }
//        }
//    }

//    // معادل تابع mf_get_logic_javascript
//    public string GetLogicJavascript(int formId, int pageNumber)
//    {
//        // دریافت عناصر هدف برای صفحه جاری
//        var logicElements = _dbContext.FieldLogicElements
//            .Where(a => a.FormId == formId)
//            .Join(_dbContext.FormElements.Where(b => b.ElementStatus == 1 && b.ElementPageNumber == pageNumber),
//                a => new { a.FormId, a.ElementId },
//                b => new { b.FormId, b.ElementId },
//                (a, b) => new {
//                    a.ElementId,
//                    a.RuleShowHide,
//                    a.RuleAllAny,
//                    b.ElementTitle,
//                    b.ElementPageNumber
//                })
//            .OrderBy(x => x.ElementPageNumber)
//            .ToList();

//        var logicElementsArray = new Dictionary<int, dynamic>();
//        foreach (var item in logicElements)
//        {
//            logicElementsArray[item.ElementId] = new
//            {
//                ElementTitle = item.ElementTitle,
//                RuleShowHide = item.RuleShowHide,
//                RuleAllAny = item.RuleAllAny
//            };
//        }

//        // دریافت شرایط منطق
//        var logicConditions = _dbContext.FieldLogicConditions
//            .Where(a => a.FormId == formId)
//            .OrderBy(a => a.AlcId)
//            .ToList();

//        var logicConditionsArray = new Dictionary<int, List<dynamic>>();
//        int prevElementId = 0;
//        int i = 0;

//        foreach (var item in logicConditions)
//        {
//            if (item.TargetElementId != prevElementId)
//            {
//                i = 0;
//            }

//            if (!logicConditionsArray.ContainsKey(item.TargetElementId))
//            {
//                logicConditionsArray[item.TargetElementId] = new List<dynamic>();
//            }

//            logicConditionsArray[item.TargetElementId].Add(new
//            {
//                ElementName = item.ElementName,
//                ElementType = item.ElementType,
//                ElementPageNumber = item.ElementPageNumber,
//                RuleCondition = item.RuleCondition,
//                RuleKeyword = item.RuleKeyword
//            });

//            prevElementId = item.TargetElementId;
//            i++;
//        }

//        // ساخت کد جاوااسکریپت
//        var mfHandlerCode = new StringBuilder();
//        var mfBindCode = new StringBuilder();
//        var mfInitializeCode = new StringBuilder();

//        foreach (var element in logicElementsArray)
//        {
//            var elementId = element.Key;
//            var ruleShowHide = element.Value.RuleShowHide;
//            var ruleAllAny = element.Value.RuleAllAny;

//            mfHandlerCode.AppendLine($"function mf_handler_{elementId}(){{");

//            // کدهای مربوط به شرایط
//            // ... (پیاده‌سازی مشابه کد PHP)

//            mfHandlerCode.AppendLine("}");

//            mfInitializeCode.AppendLine($"mf_handler_{elementId}();");
//        }

//        var javascriptCode = $@"
//<script type='text/javascript'>
//$(function(){{

//{mfHandlerCode}
//{mfBindCode}
//{mfInitializeCode}

//}});
//</script>";

//        return string.IsNullOrWhiteSpace(mfHandlerCode.ToString()) &&
//               string.IsNullOrWhiteSpace(mfBindCode.ToString()) &&
//               string.IsNullOrWhiteSpace(mfInitializeCode.ToString())
//            ? ""
//            : javascriptCode;
//    }

//    // معادل تابع mf_get_condition_javascript
//    private string GetConditionJavascript(int formId, dynamic conditionParams)
//    {
//        // پیاده‌سازی مشابه کد PHP
//        // ... (تبدیل شرایط مختلف به جاوااسکریپت)
//        return "";
//    }

//    // معادل تابع mf_get_condition_status_from_table
//    public bool GetConditionStatusFromTable(int formId, dynamic conditionParams, bool useMainTable = false, int? entryId = null)
//    {
//        // پیاده‌سازی مشابه کد PHP
//        // ... (بررسی شرایط از روی داده‌های ذخیره شده در دیتابیس)
//        return false;
//    }

//    // معادل تابع mf_get_condition_status_from_input
//    public bool GetConditionStatusFromInput(int formId, dynamic conditionParams, Dictionary<string, string> userInput)
//    {
//        // پیاده‌سازی مشابه کد PHP
//        // ... (بررسی شرایط از روی ورودی کاربر)
//        return false;
//    }
//}

//// کلاس‌های مدل
//public class AppDbContext : DbContext
//{
//    public DbSet<Form> Forms { get; set; }
//    public DbSet<FormElement> FormElements { get; set; }
//    public DbSet<FieldLogicElement> FieldLogicElements { get; set; }
//    public DbSet<FieldLogicCondition> FieldLogicConditions { get; set; }
//    public DbSet<ElementOption> ElementOptions { get; set; }
//    public DbSet<User> Users { get; set; }
//    public DbSet<Setting> Settings { get; set; }
//}

//public class Form
//{
//    public int FormId { get; set; }
//    public string FormName { get; set; }
//    public string FormLanguage { get; set; }
//    public string FormResumeSubject { get; set; }
//    public string FormResumeContent { get; set; }
//    public string FormResumeFromName { get; set; }
//    public string FormResumeFromEmailAddress { get; set; }
//    // ... سایر خصوصیات
//}

//public class FormElement
//{
//    public int FormId { get; set; }
//    public int ElementId { get; set; }
//    public string ElementType { get; set; }
//    public int ElementStatus { get; set; }
//    public int ElementPosition { get; set; }
//    public int ElementPageNumber { get; set; }
//    public string ElementTitle { get; set; }
//    // ... سایر خصوصیات
//}

//public class FieldLogicElement
//{
//    public int FormId { get; set; }
//    public int ElementId { get; set; }
//    public string RuleShowHide { get; set; }
//    public string RuleAllAny { get; set; }
//    // ... سایر خصوصیات
//}

//public class FieldLogicCondition
//{
//    public int AlcId { get; set; }
//    public int FormId { get; set; }
//    public int TargetElementId { get; set; }
//    public string ElementName { get; set; }
//    public string RuleCondition { get; set; }
//    public string RuleKeyword { get; set; }
//    // ... سایر خصوصیات
//}

//public class User
//{
//    public int UserId { get; set; }
//    public string UserFullname { get; set; }
//    public string UserEmail { get; set; }
//    public int Status { get; set; }
//    // ... سایر خصوصیات
//}

//public class Setting
//{
//    public string DefaultFromName { get; set; }
//    public string DefaultFromEmail { get; set; }
//    public string BaseUrl { get; set; }
//    public bool SmtpEnable { get; set; }
//    public string SmtpHost { get; set; }
//    public int SmtpPort { get; set; }
//    public string SmtpSecure { get; set; }
//    public bool SmtpAuth { get; set; }
//    public string SmtpUsername { get; set; }
//    public string SmtpPassword { get; set; }
//    // ... سایر تنظیمات
//}

//public class LanguageResources : Dictionary<string, string>
//{
//    // منابع زبانی
//}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Globalization;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Cryptography;
//using System.Text;

//public static class MachFormHelper
//{
//    // تبدیل مقدار به float با حذف نمادهای ارزی و جداکننده‌ها
//    public static float MfToFloat(string num)
//    {
//        int dotPos = num.LastIndexOf('.');
//        int commaPos = num.LastIndexOf(',');
//        int sep = (dotPos > commaPos && dotPos >= 0) ? dotPos :
//                  ((commaPos > dotPos && commaPos >= 0) ? commaPos : -1);

//        if (sep == -1)
//        {
//            return float.Parse(Regex.Replace(num, "[^0-9]", ""));
//        }

//        string beforeSep = Regex.Replace(num.Substring(0, sep), "[^0-9]", "");
//        string afterSep = Regex.Replace(num.Substring(sep + 1), "[^0-9]", "");

//        return float.Parse($"{beforeSep}.{afterSep}");
//    }

//    // ذخیره لاگ فعالیت فرم
//    public static void MfLogFormActivity(MachFormDbContext dbContext, int formId, int entryId, string logMessage)
//    {
//        var logTime = DateTime.Now;
//        var userId = HttpContext.Current.Session["mf_user_id"] as int?;

//        string logUser;
//        string userEmail = "";
//        string userFullname = "";

//        if (userId.HasValue)
//        {
//            var user = dbContext.Users.FirstOrDefault(u => u.UserId == userId.Value);
//            if (user != null)
//            {
//                userEmail = user.UserEmail;
//                userFullname = user.UserFullname;
//                logUser = $"{userFullname} - {userEmail} - ID:{userId}";
//            }
//            else
//            {
//                logUser = "Form User";
//            }
//        }
//        else
//        {
//            logUser = "Form User";
//        }

//        string logOrigin = $"{HttpContext.Current.Request.UserHostAddress} - {HttpContext.Current.Request.UserAgent}";

//        // ذخیره در جدول لاگ
//        var formLog = new FormLog
//        {
//            RecordId = entryId,
//            LogTime = logTime,
//            LogUser = logUser,
//            LogOrigin = logOrigin,
//            LogMessage = logMessage
//        };

//        dbContext.FormLogs.Add(formLog);
//        dbContext.SaveChanges();
//    }
//}

//// کلاس تولید رشته تصادفی
//public class RandomStringGenerator
//{
//    protected string Alphabet;
//    protected int AlphabetLength;

//    public RandomStringGenerator(string alphabet = "")
//    {
//        if (!string.IsNullOrEmpty(alphabet))
//        {
//            SetAlphabet(alphabet);
//        }
//        else
//        {
//            SetAlphabet(
//                string.Join("", Enumerable.Range('a', 26).Select(c => (char)c)) +
//                string.Join("", Enumerable.Range('A', 26).Select(c => (char)c)) +
//                string.Join("", Enumerable.Range('0', 10).Select(c => (char)c))
//            );
//        }
//    }

//    public void SetAlphabet(string alphabet)
//    {
//        Alphabet = alphabet;
//        AlphabetLength = alphabet.Length;
//    }

//    public string Generate(int length)
//    {
//        var token = new StringBuilder();
//        for (int i = 0; i < length; i++)
//        {
//            int randomKey = GetRandomInteger(0, AlphabetLength);
//            token.Append(Alphabet[randomKey]);
//        }
//        return token.ToString();
//    }

//    protected int GetRandomInteger(int min, int max)
//    {
//        int range = max - min;
//        if (range < 0)
//        {
//            return min;
//        }

//        int bits = (int)Math.Log(range, 2) + 1;
//        int bytes = (bits + 7) / 8;
//        int filter = (1 << bits) - 1;

//        byte[] randomBytes = new byte[bytes];
//        using (var rng = RandomNumberGenerator.Create())
//        {
//            int rnd;
//            do
//            {
//                rng.GetBytes(randomBytes);
//                rnd = BitConverter.ToInt32(randomBytes, 0) & filter;
//            } while (rnd >= range);

//            return min + rnd;
//        }
//    }
//}

//// کلاس‌های مدل Entity Framework
//public class MachFormDbContext : DbContext
//{
//    public DbSet<User> Users { get; set; }
//    public DbSet<Form> Forms { get; set; }
//    public DbSet<FormLog> FormLogs { get; set; }
//    public DbSet<FormStats> FormStats { get; set; }
//    public DbSet<ApprovalSetting> ApprovalSettings { get; set; }
//    public DbSet<Approver> Approvers { get; set; }
//    public DbSet<ApproverCondition> ApproverConditions { get; set; }
//    public DbSet<FormApproval> FormApprovals { get; set; }
//    public DbSet<FormPayment> FormPayments { get; set; }
//    public DbSet<Integration> Integrations { get; set; }
//    public DbSet<FormElement> FormElements { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//        optionsBuilder.UseSqlServer("YourConnectionString");
//    }
//}

//public class User
//{
//    public int UserId { get; set; }
//    public string UserEmail { get; set; }
//    public string UserFullname { get; set; }
//}

//public class Form
//{
//    public int FormId { get; set; }
//    public string FormName { get; set; }
//    public bool FormActive { get; set; }
//    public string FormTags { get; set; }
//    public bool FormApprovalEnable { get; set; }
//    public bool FormResumeEnable { get; set; }
//    public string PaymentMerchantType { get; set; }
//    public bool PaymentEnableMerchant { get; set; }
//    public string FormApprovalEmailSubject { get; set; }
//    public string FormApprovalEmailContent { get; set; }
//    public bool LogicEmailEnable { get; set; }
//    public bool LogicWebhookEnable { get; set; }
//}

//public class FormLog
//{
//    public int Id { get; set; }
//    public int RecordId { get; set; }
//    public DateTime LogTime { get; set; }
//    public string LogUser { get; set; }
//    public string LogOrigin { get; set; }
//    public string LogMessage { get; set; }
//}

//public class FormStats
//{
//    public int Id { get; set; }
//    public int FormId { get; set; }
//    public int TotalEntries { get; set; }
//    public int TodayEntries { get; set; }
//    public DateTime? LastEntryDate { get; set; }
//}

//public class ApprovalSetting
//{
//    public int Id { get; set; }
//    public int FormId { get; set; }
//    public string WorkflowType { get; set; } // 'serial' or 'parallel'
//    public string ParallelWorkflow { get; set; } // 'any' or 'all'
//}

//public class Approver
//{
//    public int Id { get; set; }
//    public int FormId { get; set; }
//    public int UserId { get; set; }
//    public string RuleAllAny { get; set; } // 'all' or 'any'
//    public int UserPosition { get; set; }
//}

//public class ApproverCondition
//{
//    public int Id { get; set; }
//    public int FormId { get; set; }
//    public int TargetUserId { get; set; }
//    public string ElementName { get; set; }
//    public string RuleCondition { get; set; }
//    public string RuleKeyword { get; set; }
//}

//public class FormApproval
//{
//    public int Id { get; set; }
//    public int FormId { get; set; }
//    public int RecordId { get; set; }
//    public DateTime DateCreated { get; set; }
//    public int UserId { get; set; }
//    public string IpAddress { get; set; }
//    public string ApprovalState { get; set; } // 'approved' or 'denied'
//    public string ApprovalNote { get; set; }
//}

//public class FormPayment
//{
//    public int Id { get; set; }
//    public int FormId { get; set; }
//    public int RecordId { get; set; }
//    public string PaymentStatus { get; set; }
//    public decimal PaymentAmount { get; set; }
//    public string PaymentId { get; set; }
//}

//public class Integration
//{
//    public int Id { get; set; }
//    public int FormId { get; set; }
//    public bool GsheetIntegrationStatus { get; set; }
//    public string GsheetRefreshToken { get; set; }
//    public string GsheetAccessToken { get; set; }
//    public string GsheetSpreadsheetId { get; set; }
//    public bool GsheetCreateNewSheet { get; set; }
//    public string GsheetElements { get; set; }
//    public bool GsheetDelayNotificationUntilPaid { get; set; }
//    public bool GsheetDelayNotificationUntilApproved { get; set; }
//    public bool GcalIntegrationStatus { get; set; }
//    public string GcalRefreshToken { get; set; }
//    public string GcalAccessToken { get; set; }
//    public string GcalCalendarId { get; set; }
//    public string GcalEventTitle { get; set; }
//    public string GcalEventDesc { get; set; }
//    public string GcalEventLocation { get; set; }
//    public bool GcalEventAllday { get; set; }
//    public string GcalStartDatetime { get; set; }
//    public int GcalStartDateElement { get; set; }
//    public int GcalStartTimeElement { get; set; }
//    public string GcalStartDateType { get; set; }
//    public string GcalStartTimeType { get; set; }
//    public string GcalEndDatetime { get; set; }
//    public int GcalEndDateElement { get; set; }
//    public int GcalEndTimeElement { get; set; }
//    public string GcalEndDateType { get; set; }
//    public string GcalEndTimeType { get; set; }
//    public string GcalDurationType { get; set; }
//    public int GcalDurationPeriodLength { get; set; }
//    public string GcalDurationPeriodUnit { get; set; }
//    public string GcalAttendeeEmail { get; set; }
//    public bool GcalDelayNotificationUntilPaid { get; set; }
//    public bool GcalDelayNotificationUntilApproved { get; set; }
//}

//public class FormElement
//{
//    public int ElementId { get; set; }
//    public int FormId { get; set; }
//    public string ElementName { get; set; }
//    public string ElementType { get; set; }
//}



//public static class ApprovalWorkflow
//{
//    // شروع فرآیند تایید
//    public static void CreateApprovalProcess(MachFormDbContext dbContext, int formId, int entryId)
//    {
//        var approverUserIds = GetApprovalQueueUserIds(dbContext, formId, entryId);

//        if (approverUserIds.Any())
//        {
//            string approvalQueueUserId;
//            if (approverUserIds.Count > 1)
//            {
//                approvalQueueUserId = $"|{string.Join("|", approverUserIds)}|";
//            }
//            else
//            {
//                approvalQueueUserId = approverUserIds.First().ToString();
//            }

//            // به‌روزرسانی رکورد با وضعیت در انتظار تایید
//            var formRecord = dbContext.FormRecords.FirstOrDefault(f => f.FormId == formId && f.Id == entryId);
//            if (formRecord != null)
//            {
//                formRecord.ApprovalStatus = "pending";
//                formRecord.ApprovalQueueUserId = approvalQueueUserId;
//                dbContext.SaveChanges();
//            }

//            // ارسال اعلان به تاییدکنندگان
//            NotifyApprovers(dbContext, formId, entryId, approverUserIds);
//        }
//    }

//    // دریافت لیست کاربران تاییدکننده بعدی
//    public static List<int> GetApprovalQueueUserIds(MachFormDbContext dbContext, int formId, int entryId)
//    {
//        var approvalSettings = dbContext.ApprovalSettings.FirstOrDefault(a => a.FormId == formId);
//        if (approvalSettings == null)
//            return new List<int>();

//        string workflowType = approvalSettings.WorkflowType;
//        string parallelWorkflow = approvalSettings.ParallelWorkflow;

//        var formRecord = dbContext.FormRecords.FirstOrDefault(f => f.FormId == formId && f.Id == entryId);
//        bool isFirstApprovalStep = string.IsNullOrEmpty(formRecord?.ApprovalQueueUserId);

//        var approvers = dbContext.Approvers
//            .Where(a => a.FormId == formId)
//            .OrderBy(a => a.UserPosition)
//            .ToList();

//        var approvalQueueUserIds = new List<int>();

//        if (!approvers.Any())
//            return approvalQueueUserIds;

//        if (isFirstApprovalStep)
//        {
//            foreach (var approver in approvers)
//            {
//                var conditions = dbContext.ApproverConditions
//                    .Where(c => c.FormId == formId && c.TargetUserId == approver.UserId)
//                    .ToList();

//                bool allConditionsStatus = CheckConditions(dbContext, formId, entryId, conditions, approver.RuleAllAny);

//                if (allConditionsStatus)
//                {
//                    approvalQueueUserIds.Add(approver.UserId);

//                    if (workflowType == "serial")
//                    {
//                        break;
//                    }
//                }
//            }
//        }
//        else
//        {
//            if (workflowType == "serial")
//            {
//                var lastApproval = dbContext.FormApprovals
//                    .Where(a => a.FormId == formId && a.RecordId == entryId)
//                    .OrderByDescending(a => a.Id)
//                    .FirstOrDefault();

//                if (lastApproval != null)
//                {
//                    int currentApproverUserId = lastApproval.UserId;
//                    bool isUserCandidate = false;
//                    var nextApproversCandidates = new List<Approver>();

//                    foreach (var approver in approvers)
//                    {
//                        if (isUserCandidate)
//                        {
//                            nextApproversCandidates.Add(approver);
//                        }

//                        if (approver.UserId == currentApproverUserId)
//                        {
//                            isUserCandidate = true;
//                        }
//                    }

//                    foreach (var candidate in nextApproversCandidates)
//                    {
//                        var conditions = dbContext.ApproverConditions
//                            .Where(c => c.FormId == formId && c.TargetUserId == candidate.UserId)
//                            .ToList();

//                        bool allConditionsStatus = CheckConditions(dbContext, formId, entryId, conditions, candidate.RuleAllAny);

//                        if (allConditionsStatus)
//                        {
//                            approvalQueueUserIds.Add(candidate.UserId);
//                            break;
//                        }
//                    }
//                }
//            }
//        }

//        return approvalQueueUserIds;
//    }

//    // بررسی شرایط تایید
//    private static bool CheckConditions(MachFormDbContext dbContext, int formId, int entryId, List<ApproverCondition> conditions, string ruleAllAny)
//    {
//        var currentRuleConditionsStatus = new List<bool>();

//        foreach (var condition in conditions)
//        {
//            bool conditionStatus = GetConditionStatusFromTable(dbContext, formId, entryId, condition);
//            currentRuleConditionsStatus.Add(conditionStatus);
//        }

//        if (ruleAllAny == "all")
//        {
//            return !currentRuleConditionsStatus.Contains(false);
//        }
//        else if (ruleAllAny == "any")
//        {
//            return currentRuleConditionsStatus.Contains(true);
//        }

//        return false;
//    }

//    // امضای تایید
//    public static string SignApproval(MachFormDbContext dbContext, int formId, int entryId, ApprovalSignatureParams signatureParams)
//    {
//        var approvalResult = "pending";
//        var settings = GetSettings(dbContext);
//        var formProperties = GetFormProperties(dbContext, formId);

//        // بررسی وضعیت پرداخت
//        bool paymentCompleted = dbContext.FormPayments
//            .Any(p => p.FormId == formId && p.RecordId == entryId && p.PaymentStatus == "paid");

//        var approvalSettings = dbContext.ApprovalSettings.FirstOrDefault(a => a.FormId == formId);
//        if (approvalSettings == null)
//            return approvalResult;

//        // ذخیره تایید در جدول approvals
//        var approval = new FormApproval
//        {
//            FormId = formId,
//            RecordId = entryId,
//            DateCreated = DateTime.Now,
//            UserId = signatureParams.UserId,
//            IpAddress = signatureParams.IpAddress,
//            ApprovalState = signatureParams.ApprovalState,
//            ApprovalNote = signatureParams.ApprovalNote
//        };

//        dbContext.FormApprovals.Add(approval);
//        dbContext.SaveChanges();

//        // دریافت تاییدکنندگان بعدی
//        var nextApproverUserIds = GetApprovalQueueUserIds(dbContext, formId, entryId);

//        if (nextApproverUserIds.Any())
//        {
//            if (signatureParams.ApprovalState == "approved")
//            {
//                string approvalQueueUserId = nextApproverUserIds.Count > 1 ?
//                    $"|{string.Join("|", nextApproverUserIds)}|" :
//                    nextApproverUserIds.First().ToString();

//                var formRecord = dbContext.FormRecords.FirstOrDefault(f => f.FormId == formId && f.Id == entryId);
//                if (formRecord != null)
//                {
//                    formRecord.ApprovalQueueUserId = approvalQueueUserId;
//                    dbContext.SaveChanges();
//                }

//                NotifyApprovers(dbContext, formId, entryId, nextApproverUserIds);
//            }
//            else if (signatureParams.ApprovalState == "denied")
//            {
//                var formRecord = dbContext.FormRecords.FirstOrDefault(f => f.FormId == formId && f.Id == entryId);
//                if (formRecord != null)
//                {
//                    formRecord.ApprovalQueueUserId = "";
//                    formRecord.ApprovalStatus = signatureParams.ApprovalState;
//                    dbContext.SaveChanges();
//                }

//                // اجرای منطق "رد شده است"
//                if (formProperties.LogicEmailEnable)
//                {
//                    SendLogicNotifications(dbContext, formId, entryId, new LogicEmailParams
//                    {
//                        MachformBasePath = settings.BaseUrl,
//                        ApprovalStatusIsDenied = true,
//                        PaymentCompleted = paymentCompleted
//                    });
//                }

//                if (formProperties.LogicWebhookEnable)
//                {
//                    SendLogicWebhookNotifications(dbContext, formId, entryId, new LogicWebhookParams
//                    {
//                        ApprovalStatusIsDenied = true,
//                        PaymentCompleted = paymentCompleted
//                    });
//                }

//                RunIntegrations(dbContext, formId, entryId, new IntegrationParams
//                {
//                    PaymentCompleted = paymentCompleted,
//                    ApprovalStatusIsDenied = true
//                });

//                approvalResult = signatureParams.ApprovalState;
//            }
//        }
//        else
//        {
//            // این آخرین مرحله تایید است
//            if (approvalSettings.WorkflowType == "serial" ||
//                (approvalSettings.WorkflowType == "parallel" && approvalSettings.ParallelWorkflow == "any"))
//            {
//                var formRecord = dbContext.FormRecords.FirstOrDefault(f => f.FormId == formId && f.Id == entryId);
//                if (formRecord != null)
//                {
//                    formRecord.ApprovalQueueUserId = "";
//                    formRecord.ApprovalStatus = signatureParams.ApprovalState;
//                    dbContext.SaveChanges();
//                }

//                approvalResult = signatureParams.ApprovalState;

//                // اجرای منطق مربوطه
//                if (formProperties.LogicEmailEnable)
//                {
//                    var emailParams = new LogicEmailParams
//                    {
//                        MachformBasePath = settings.BaseUrl,
//                        PaymentCompleted = paymentCompleted
//                    };

//                    if (approvalResult == "approved")
//                        emailParams.ApprovalStatusIsApproved = true;
//                    else if (approvalResult == "denied")
//                        emailParams.ApprovalStatusIsDenied = true;

//                    SendLogicNotifications(dbContext, formId, entryId, emailParams);
//                }

//                if (formProperties.LogicWebhookEnable)
//                {
//                    var webhookParams = new LogicWebhookParams
//                    {
//                        PaymentCompleted = paymentCompleted
//                    };

//                    if (approvalResult == "approved")
//                        webhookParams.ApprovalStatusIsApproved = true;
//                    else if (approvalResult == "denied")
//                        webhookParams.ApprovalStatusIsDenied = true;

//                    SendLogicWebhookNotifications(dbContext, formId, entryId, webhookParams);
//                }

//                RunIntegrations(dbContext, formId, entryId, new IntegrationParams
//                {
//                    PaymentCompleted = paymentCompleted,
//                    ApprovalStatusIsApproved = approvalResult == "approved",
//                    ApprovalStatusIsDenied = approvalResult == "denied"
//                });
//            }
//            else if (approvalSettings.WorkflowType == "parallel" && approvalSettings.ParallelWorkflow == "all")
//            {
//                // بررسی امضاهای دیگر تاییدکنندگان
//                var formRecord = dbContext.FormRecords.FirstOrDefault(f => f.FormId == formId && f.Id == entryId);
//                if (formRecord == null)
//                    return approvalResult;

//                var approvalQueueUserIds = formRecord.ApprovalQueueUserId?
//                    .Trim('|')
//                    .Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
//                    .Select(int.Parse)
//                    .ToList() ?? new List<int>();

//                int totalApprovers = approvalQueueUserIds.Count;

//                var approvals = dbContext.FormApprovals
//                    .Where(a => a.FormId == formId &&
//                                a.RecordId == entryId &&
//                                approvalQueueUserIds.Contains(a.UserId))
//                    .ToList();

//                int currentApprovalNumber = approvals.Count;
//                bool currentApprovalAllApproved = approvals.All(a => a.ApprovalState == "approved");

//                if (currentApprovalNumber == totalApprovers || !currentApprovalAllApproved)
//                {
//                    formRecord.ApprovalQueueUserId = "";
//                    formRecord.ApprovalStatus = currentApprovalAllApproved ? "approved" : "denied";
//                    dbContext.SaveChanges();

//                    approvalResult = formRecord.ApprovalStatus;

//                    // اجرای منطق مربوطه
//                    if (formProperties.LogicEmailEnable)
//                    {
//                        var emailParams = new LogicEmailParams
//                        {
//                            MachformBasePath = settings.BaseUrl,
//                            PaymentCompleted = paymentCompleted
//                        };

//                        if (approvalResult == "approved")
//                            emailParams.ApprovalStatusIsApproved = true;
//                        else if (approvalResult == "denied")
//                            emailParams.ApprovalStatusIsDenied = true;

//                        SendLogicNotifications(dbContext, formId, entryId, emailParams);
//                    }

//                    if (formProperties.LogicWebhookEnable)
//                    {
//                        var webhookParams = new LogicWebhookParams
//                        {
//                            PaymentCompleted = paymentCompleted
//                        };

//                        if (approvalResult == "approved")
//                            webhookParams.ApprovalStatusIsApproved = true;
//                        else if (approvalResult == "denied")
//                            webhookParams.ApprovalStatusIsDenied = true;

//                        SendLogicWebhookNotifications(dbContext, formId, entryId, webhookParams);
//                    }

//                    RunIntegrations(dbContext, formId, entryId, new IntegrationParams
//                    {
//                        PaymentCompleted = paymentCompleted,
//                        ApprovalStatusIsApproved = approvalResult == "approved",
//                        ApprovalStatusIsDenied = approvalResult == "denied"
//                    });
//                }
//                else
//                {
//                    // حذف کاربر فعلی از صف
//                    approvalQueueUserIds.Remove(signatureParams.UserId);
//                    formRecord.ApprovalQueueUserId = $"|{string.Join("|", approvalQueueUserIds)}|";
//                    dbContext.SaveChanges();
//                }
//            }
//        }

//        return approvalResult;
//    }

//    // ارسال اعلان به تاییدکنندگان
//    public static void NotifyApprovers(MachFormDbContext dbContext, int formId, int entryId, List<int> approverUserIds)
//    {
//        var settings = GetSettings(dbContext);
//        var formProperties = GetFormProperties(dbContext, formId);

//        var approvers = dbContext.Users
//            .Where(u => approverUserIds.Contains(u.UserId))
//            .Select(u => new { u.UserId, u.UserEmail })
//            .ToList();

//        var approverEmails = approvers.Select(a => a.UserEmail).ToList();

//        // آماده‌سازی محتوای ایمیل
//        string viewEntryLink = $"<a href=\"{settings.BaseUrl}view_entry.php?form_id={formId}&entry_id={entryId}\">VIEW ENTRY</a>";

//        string subjectTemplate = formProperties.FormApprovalEmailSubject ?? "Approval Required - {form_name} [#{entry_no}]";
//        string contentTemplate = formProperties.FormApprovalEmailContent ??
//            "This entry needs your approval.<br/><br/>Please approve or deny by using the link below:<br/><strong>{view_entry_link}</strong><br/><br/><hr style=\"width: 60%;margin-top: 20px;margin-bottom: 20px\"><br/>{entry_data}";

//        var templateData = GetTemplateVariables(dbContext, formId, entryId, new TemplateOptions
//        {
//            AsPlainText = false,
//            TargetIsAdmin = true,
//            MachformPath = settings.BaseUrl,
//            ShowImagePreview = true,
//            UseListLayout = false
//        });

//        // جایگزینی متغیرها در موضوع و محتوا
//        string subject = ReplaceTemplateVariables(subjectTemplate, templateData.Variables, templateData.Values);
//        subject = subject.Replace("&nbsp;", "");
//        subject = HttpUtility.HtmlDecode(subject);

//        string content = ReplaceTemplateVariables(contentTemplate, templateData.Variables, templateData.Values);

//        // ارسال ایمیل
//        if (approverEmails.Any())
//        {
//            SendEmail(new EmailMessage
//            {
//                FromEmail = settings.DefaultFromEmail ?? $"no-reply@{HttpContext.Current.Request.Url.Host.Replace("www.", "")}",
//                FromName = settings.DefaultFromName ?? "MachForm",
//                ToEmails = approverEmails,
//                Subject = subject,
//                Body = content,
//                IsBodyHtml = true
//            });
//        }
//    }
//}


//public static class HelperMethods
//{
//    // دریافت لیست زمان‌بندی‌های پشتیبانی شده
//    public static List<TimezoneInfo> GetTimezoneList()
//    {
//        var timezones = TimeZoneInfo.GetSystemTimeZones();
//        var continents = new[] { "America/Indiana", "Africa", "America", "Antarctica", "Arctic", "Asia", "Atlantic", "Australia", "Europe", "Indian", "Pacific" };

//        var timezoneData = new List<TimezoneInfo>();

//        foreach (var tz in timezones)
//        {
//            var simpleName = tz.Id;
//            foreach (var continent in continents)
//            {
//                if (simpleName.StartsWith(continent))
//                {
//                    simpleName = simpleName.Substring(continent.Length).Trim('/');
//                    break;
//                }
//            }

//            // جایگزینی نام‌های خاص
//            switch (simpleName)
//            {
//                case "New_York": simpleName = "Eastern Time (New York)"; break;
//                case "Chicago": simpleName = "Central Time (Chicago)"; break;
//                case "Denver": simpleName = "Mountain Time (Denver)"; break;
//                case "Phoenix": simpleName = "Mountain Time - no DST (Phoenix)"; break;
//                case "Los_Angeles": simpleName = "Pacific Time (Los Angeles)"; break;
//                case "Anchorage": simpleName = "Alaska (Anchorage)"; break;
//                case "Adak": simpleName = "Hawaii (Adak)"; break;
//                case "Honolulu": simpleName = "Hawaii - no DST (Honolulu)"; break;
//            }

//            simpleName = simpleName.Replace("_", " ");

//            var offset = tz.BaseUtcOffset;
//            string gmtOffset = offset < TimeSpan.Zero ?
//                $"GMT-{offset.ToString("hh\\:mm").Substring(1)}" :
//                $"GMT+{offset.ToString("hh\\:mm")}";

//            timezoneData.Add(new TimezoneInfo
//            {
//                FullName = tz.Id,
//                SimpleName = simpleName,
//                RawOffset = (int)offset.TotalSeconds,
//                GmtOffset = gmtOffset
//            });
//        }

//        // مرتب‌سازی بر اساس offset
//        timezoneData.Sort((a, b) => a.RawOffset.CompareTo(b.RawOffset));

//        return timezoneData;
//    }

//    // تنظیم زمان‌بندی سیستم بر اساس ترجیح کاربر
//    public static void SetSystemTimezone(MachFormDbContext dbContext)
//    {
//        var settings = GetSettings(dbContext);
//        if (!string.IsNullOrEmpty(settings.Timezone))
//        {
//            TimeZoneInfo timezone;
//            try
//            {
//                timezone = TimeZoneInfo.FindSystemTimeZoneById(settings.Timezone);
//                TimeZoneInfo.Local = timezone;
//            }
//            catch
//            {
//                // در صورت خطا، از زمان‌بندی پیش‌فرض سیستم استفاده می‌شود
//            }
//        }
//    }

//    // اجرای وظایف زمان‌بندی شده
//    public static void RunCronJobs(MachFormDbContext dbContext, bool executeNow = false)
//    {
//        if (!executeNow && DateTime.Now.Day != 1)
//        {
//            return;
//        }

//        var settings = GetSettings(dbContext);
//        bool enableDataRetention = settings.EnableDataRetention;
//        int dataRetentionPeriod = settings.DataRetentionPeriod;

//        if (enableDataRetention && dataRetentionPeriod > 0)
//        {
//            var forms = dbContext.Forms
//                .Where(f => (f.FormActive == 0 || f.FormActive == 1) &&
//                           (f.FormTags == null || !f.FormTags.Contains("skipdataretention")))
//                .Select(f => f.FormId)
//                .ToList();

//            foreach (var formId in forms)
//            {
//                var cutoffDate = DateTime.Now.AddMonths(-dataRetentionPeriod);
//                var oldRecords = dbContext.FormRecords
//                    .Where(f => f.FormId == formId && f.DateCreated < cutoffDate)
//                    .ToList();

//                dbContext.FormRecords.RemoveRange(oldRecords);
//                dbContext.SaveChanges();
//            }
//        }
//    }

//    // اجرای تمامی یکپارچه‌سازی‌ها
//    public static void RunIntegrations(MachFormDbContext dbContext, int formId, int entryId, IntegrationParams options)
//    {
//        bool isPaymentCompleted = options.PaymentCompleted;
//        bool approvalStatusIsDenied = options.ApprovalStatusIsDenied;
//        bool approvalStatusIsApproved = options.ApprovalStatusIsApproved;

//        var integration = dbContext.Integrations.FirstOrDefault(i => i.FormId == formId);
//        if (integration == null)
//            return;

//        var formProperties = GetFormProperties(dbContext, formId);
//        bool formApprovalEnable = formProperties.FormApprovalEnable;
//        bool formPaymentEnable = formProperties.PaymentEnableMerchant && formProperties.PaymentMerchantType != "check";

//        // یکپارچه‌سازی Google Sheets
//        if (integration.GsheetIntegrationStatus)
//        {
//            bool runGsheetIntegration = false;

//            if (!integration.GsheetDelayNotificationUntilPaid && !integration.GsheetDelayNotificationUntilApproved)
//            {
//                runGsheetIntegration = !approvalStatusIsApproved && !approvalStatusIsDenied && !isPaymentCompleted;
//            }
//            else if (integration.GsheetDelayNotificationUntilPaid && !integration.GsheetDelayNotificationUntilApproved)
//            {
//                runGsheetIntegration = isPaymentCompleted;
//            }
//            else if (!integration.GsheetDelayNotificationUntilPaid && integration.GsheetDelayNotificationUntilApproved)
//            {
//                runGsheetIntegration = approvalStatusIsApproved;
//            }
//            else if (integration.GsheetDelayNotificationUntilPaid && integration.GsheetDelayNotificationUntilApproved)
//            {
//                if (!isPaymentCompleted)
//                {
//                    isPaymentCompleted = dbContext.FormPayments
//                        .Any(p => p.FormId == formId && p.RecordId == entryId && p.PaymentStatus == "paid");
//                }

//                if (!approvalStatusIsApproved)
//                {
//                    var record = dbContext.FormRecords.FirstOrDefault(f => f.FormId == formId && f.Id == entryId);
//                    approvalStatusIsApproved = record?.ApprovalStatus == "approved";
//                }

//                runGsheetIntegration = isPaymentCompleted && approvalStatusIsApproved;
//            }

//            if (runGsheetIntegration)
//            {
//                SaveEntryToGoogleSheet(dbContext, formId, entryId);
//            }
//        }

//        // یکپارچه‌سازی Google Calendar
//        if (integration.GcalIntegrationStatus)
//        {
//            bool runGcalIntegration = false;

//            if (!integration.GcalDelayNotificationUntilPaid && !integration.GcalDelayNotificationUntilApproved)
//            {
//                runGcalIntegration = !approvalStatusIsApproved && !approvalStatusIsDenied && !isPaymentCompleted;
//            }
//            else if (integration.GcalDelayNotificationUntilPaid && !integration.GcalDelayNotificationUntilApproved)
//            {
//                runGcalIntegration = isPaymentCompleted;
//            }
//            else if (!integration.GcalDelayNotificationUntilPaid && integration.GcalDelayNotificationUntilApproved)
//            {
//                runGcalIntegration = approvalStatusIsApproved;
//            }
//            else if (integration.GcalDelayNotificationUntilPaid && integration.GcalDelayNotificationUntilApproved)
//            {
//                if (!isPaymentCompleted)
//                {
//                    isPaymentCompleted = dbContext.FormPayments
//                        .Any(p => p.FormId == formId && p.RecordId == entryId && p.PaymentStatus == "paid");
//                }

//                if (!approvalStatusIsApproved)
//                {
//                    var record = dbContext.FormRecords.FirstOrDefault(f => f.FormId == formId && f.Id == entryId);
//                    approvalStatusIsApproved = record?.ApprovalStatus == "approved";
//                }

//                runGcalIntegration = isPaymentCompleted && approvalStatusIsApproved;
//            }

//            if (runGcalIntegration)
//            {
//                SaveEntryToGoogleCalendar(dbContext, formId, entryId);
//            }
//        }
//    }

//    // ذخیره ورودی در Google Sheets
//    private static void SaveEntryToGoogleSheet(MachFormDbContext dbContext, int formId, int entryId)
//    {
//        // پیاده‌سازی مشابه کد PHP با استفاده از Google Sheets API
//        // نیاز به تنظیمات و احراز هویت مناسب دارد
//    }

//    // ذخیره رویداد در Google Calendar
//    private static void SaveEntryToGoogleCalendar(MachFormDbContext dbContext, int formId, int entryId)
//    {
//        // پیاده‌سازی مشابه کد PHP با استفاده از Google Calendar API
//        // نیاز به تنظیمات و احراز هویت مناسب دارد
//    }

//    // به‌روزرسانی آمار فرم
//    public static void RefreshFormStats(MachFormDbContext dbContext, int formId)
//    {
//        var stats = new FormStats();

//        // تعداد کل ورودی‌ها
//        stats.TotalEntries = dbContext.FormRecords
//            .Count(f => f.FormId == formId && f.Status == 1);

//        // تعداد ورودی‌های امروز
//        var today = DateTime.Today;
//        stats.TodayEntries = dbContext.FormRecords
//            .Count(f => f.FormId == formId && f.DateCreated >= today);

//        // تاریخ آخرین ورودی
//        var lastEntry = dbContext.FormRecords
//            .Where(f => f.FormId == formId && f.Status == 1 && f.ResumeKey == null)
//            .OrderByDescending(f => f.Id)
//            .Select(f => f.DateCreated)
//            .FirstOrDefault();

//        stats.LastEntryDate = lastEntry;

//        // ذخیره آمار
//        var existingStats = dbContext.FormStats.FirstOrDefault(s => s.FormId == formId);
//        if (existingStats != null)
//        {
//            dbContext.FormStats.Remove(existingStats);
//        }

//        stats.FormId = formId;
//        dbContext.FormStats.Add(stats);
//        dbContext.SaveChanges();
//    }

//    using System.Text;

//public static class RatingHelper
//{
//    // تبدیل عدد به HTML markup رتبه‌بندی
//    public static string NumericToRating(int ratingValue, int ratingMax, string ratingType)
//    {
//        ratingValue = Math.Max(0, ratingValue);
//        ratingMax = Math.Max(0, ratingMax);

//        var ratingMarkup = new StringBuilder();

//        for (int i = 1; i <= ratingMax; i++)
//        {
//            string ratingColor = i <= ratingValue ? "#ffba0a" : "#caccdc";
//            string iconClass = GetIconClass(ratingType);

//            ratingMarkup.Append($"<span class=\"{iconClass}\" style=\"color: {ratingColor};padding-right: 2px\"></span>");
//        }

//        return ratingMarkup.ToString();
//    }

//    private static string GetIconClass(string ratingType)
//    {
//        return ratingType switch
//        {
//            "star" => "icon-star-full2",
//            "circle" => "icon-star",
//            "love" => "icon-heart5",
//            "thumb" => "icon-thumbs-up3",
//            _ => "icon-star-full2"
//        };
//    }
//}


//using System.Security.Cryptography;
//using System.Web;

//public static class SecurityHelper
//{
//    // بررسی اعتبار CSRF Token
//    public static void VerifyCsrfToken(string receivedCsrfToken)
//    {
//        var sessionToken = HttpContext.Current.Session["mf_csrf_token"] as string;

//        if (!string.IsNullOrEmpty(sessionToken))
//        {
//            receivedCsrfToken = receivedCsrfToken ?? string.Empty;

//            if (!SecureEquals(sessionToken, receivedCsrfToken))
//            {
//                HttpContext.Current.Response.Write("Error. Invalid CSRF token.");
//                HttpContext.Current.Response.End();
//            }
//        }
//    }

//    // مقایسه امن رشته‌ها برای جلوگیری از حملات زمان‌بندی
//    private static bool SecureEquals(string a, string b)
//    {
//        if (a.Length != b.Length)
//        {
//            return false;
//        }

//        int result = 0;
//        for (int i = 0; i < a.Length; i++)
//        {
//            result |= a[i] ^ b[i];
//        }
//        return result == 0;
//    }
//}


//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//public class Rating
//{
//    [Key]
//    public int Id { get; set; }

//    [Required]
//    public int RatingValue { get; set; }

//    [Required]
//    public int RatingMax { get; set; }

//    [Required]
//    [StringLength(20)]
//    public string RatingType { get; set; } // 'star', 'circle', 'love', 'thumb'

//    [Required]
//    public DateTime CreatedAt { get; set; }

//    [ForeignKey("User")]
//    public int UserId { get; set; }
//    public virtual User User { get; set; }
//}

//public class CsrfToken
//{
//    [Key]
//    public int Id { get; set; }

//    [Required]
//    [StringLength(100)]
//    public string Token { get; set; }

//    [Required]
//    public DateTime ExpiresAt { get; set; }

//    [ForeignKey("User")]
//    public int UserId { get; set; }
//    public virtual User User { get; set; }
//}

//public class User
//{
//    [Key]
//    public int Id { get; set; }

//    [Required]
//    [StringLength(100)]
//    public string Username { get; set; }

//    [Required]
//    [StringLength(255)]
//    public string Email { get; set; }

//    public virtual ICollection<Rating> Ratings { get; set; }
//    public virtual ICollection<CsrfToken> CsrfTokens { get; set; }
//}



