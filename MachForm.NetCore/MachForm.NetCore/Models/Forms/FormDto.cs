using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MachForm.NetCore.Models.Elements;
using MachForm.NetCore.Models.FormStats;

namespace MachForm.NetCore.Models.Forms;

public class FormDto
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool NameHide { get; set; }
    [AllowNull]
    public string Tags { get; set; } = null;
    public string Email { get; set; }
    public string Redirect { get; set; }
    public bool RedirectEnable { get; set; }
    public string SuccessMessage { get; set; }
    public string DisabledMessage { get; set; }
    [AllowNull]
    public string Password { get; set; } = null;
    public bool UniqueIp { get; set; }
    public int UniqueIpMaxcount { get; set; } = 5;
    public FormUniqueIpPeriod UniqueIpPeriod { get; set; } = (FormUniqueIpPeriod)2;
    [AllowNull]
    public int FrameHeight { get; set; } = default(int);
    public bool HasCss { get; set; }
    public bool Captcha { get; set; }
    public string CaptchaType { get; set; } = "n";
    public FormStatus IsActive { get; set; }
    public int ThemeId { get; set; } = 0;
    public int Review { get; set; } = 0;
    public bool ResumeEnable { get; set; }
    public string ResumeSubject { get; set; }
    public string ResumeContent { get; set; }
    public string ResumeFromName { get; set; }
    public string ResumeFromEmailAddress { get; set; } = "";
    public bool CustomScriptEnable { get; set; }
    public string CustomScriptUrl { get; set; }
    public bool LimitEnable { get; set; }
    public bool Limit { get; set; }
    public string LabelAlignment { get; set; } = "top_label";
    [AllowNull]
    public string Language { get; set; } = null;
    public int PageTotal { get; set; } = 1;
    [AllowNull]
    public string LastpageTitle { get; set; } = null;
    public string SubmitPrimaryText { get; set; } = "Submit";
    public string SubmitSecondaryText { get; set; } = "Previous";
    [AllowNull]
    public string SubmitPrimaryImg { get; set; } = null;
    [AllowNull]
    public string SubmitSecondaryImg { get; set; } = null;
    public bool SubmitUseImage { get; set; } 
    public string ReviewPrimaryText { get; set; } = "Submit";
    public string ReviewSecondaryText { get; set; } = "Previous";
    [AllowNull]
    public string ReviewPrimaryImg { get; set; } = null;
    [AllowNull]
    public string ReviewSecondaryImg { get; set; } = null;
    public bool ReviewUseImage { get; set; } 
    public string ReviewTitle { get; set; }
    public string ReviewDescription { get; set; }
    public string PaginationType { get; set; } = "steps";
    public bool ScheduleEnable { get; set; } 
    [AllowNull]
    public DateTime ScheduleStartDate { get; set; } = default(DateTime);
    [AllowNull]
    public DateTime ScheduleEndDate { get; set; } = default(DateTime);
    [AllowNull]
    public DateTime ScheduleStartHour { get; set; } = default(DateTime);
    [AllowNull]
    public DateTime ScheduleEndHour { get; set; } = default(DateTime);
    public bool ApprovalEnable { get; set; }
    public string ApprovalEmailSubject { get; set; }
    public string ApprovalEmailContent { get; set; }
    [AllowNull]
    public DateTime CreatedDate { get; set; } = default(DateTime);
    [AllowNull]
    public int CreatedBy { get; set; } = default(int);
    public bool EslEnable { get; set; }
    public string EslFromName { get; set; }
    [AllowNull]
    public string EslFromEmailAddress { get; set; } = null;
    public string EslBccEmailAddress { get; set; }
    [AllowNull]
    public string EslReplytoEmailAddress { get; set; } = null;
    public string EslSubject { get; set; }
    public string EslContent { get; set; }
    public bool EslPlainText { get; set; }
    public bool EslPdfEnable { get; set; } 
    public string EslPdfContent { get; set; }
    public bool EsrEnable { get; set; } 
    public string EsrEmailAddress { get; set; }
    public string EsrFromName { get; set; }
    [AllowNull]
    public string EsrFromEmailAddress { get; set; } = null;
    public string EsrBccEmailAddress { get; set; }
    [AllowNull]
    public string EsrReplytoEmailAddress { get; set; } = null;
    public string EsrSubject { get; set; }
    public string EsrContent { get; set; }
    public bool EsrPlainText { get; set; } 
    public bool EsrPdfEnable { get; set; } 
    public string EsrPdfContent { get; set; }
    public bool PaymentEnableMerchant { get; set; }
    public string PaymentMerchantType { get; set; } = "paypal_standard";
    [AllowNull]
    public string PaymentPaypalEmail { get; set; } = null;
    public string PaymentPaypalLanguage { get; set; } = "US";
    public string PaymentCurrency { get; set; } = "USD";
    public bool PaymentShowTotal { get; set; } 
    public string PaymentTotalLocation { get; set; } = "top";
    public bool PaymentEnableRecurring { get; set; }
    public int PaymentRecurringCycle { get; set; } = 1;
    public string PaymentRecurringUnit { get; set; } = "month";
    public bool PaymentEnableTrial { get; set; } 
    public int PaymentTrialPeriod { get; set; } = 1;
    public string PaymentTrialUnit { get; set; } = "month";
    public decimal PaymentTrialAmount { get; set; } = decimal.Parse("0.00");
    public bool PaymentEnableSetupfee { get; set; } 
    public decimal PaymentSetupfeeAmount { get; set; } = decimal.Parse("0.00");
    public string PaymentPriceType { get; set; } = "fixed";
    public decimal PaymentPriceAmount { get; set; } = decimal.Parse("0.00");
    [AllowNull]
    public string PaymentPriceName { get; set; } = null;
    [AllowNull]
    public string PaymentStripeLiveSecretKey { get; set; } = null;
    [AllowNull]
    public string PaymentStripeLivePublicKey { get; set; } = null;
    [AllowNull]
    public string PaymentStripeTestSecretKey { get; set; } = null;
    [AllowNull]
    public string PaymentStripeTestPublicKey { get; set; } = null;
    public bool PaymentStripeEnableTestMode { get; set; }
    public bool PaymentStripeEnableReceipt { get; set; }
    [AllowNull]
    public int PaymentStripeReceiptElementId { get; set; } = default(int);
    public bool PaymentStripeEnablePaymentRequestButton { get; set; }
    [AllowNull]
    public string PaymentStripeAccountCountry { get; set; } = null;
    [AllowNull]
    public string PaymentPaypalRestLiveClientid { get; set; } = null;
    [AllowNull]
    public string PaymentPaypalRestLiveSecretKey { get; set; } = null;
    [AllowNull]
    public string PaymentPaypalRestTestClientid { get; set; } = null;
    [AllowNull]
    public string PaymentPaypalRestTestSecretKey { get; set; } = null;
    public int PaymentPaypalRestEnableTestMode { get; set; } = 0;
    [AllowNull]
    public string PaymentAuthorizenetLiveApiloginid { get; set; } = null;
    [AllowNull]
    public string PaymentAuthorizenetLiveTranskey { get; set; } = null;
    [AllowNull]
    public string PaymentAuthorizenetTestApiloginid { get; set; } = null;
    [AllowNull]
    public string PaymentAuthorizenetTestTranskey { get; set; } = null;
    public bool PaymentAuthorizenetEnableTestMode { get; set; } 
    public bool PaymentAuthorizenetSaveCcData { get; set; } 
    public bool PaymentAuthorizenetEnableEmail { get; set; } 
    [AllowNull]
    public int PaymentAuthorizenetEmailElementId { get; set; } = default(int);
    [AllowNull]
    public string PaymentBraintreeLiveMerchantId { get; set; } = null;
    [AllowNull]
    public string PaymentBraintreeLivePublicKey { get; set; } = null;
    [AllowNull]
    public string PaymentBraintreeLivePrivateKey { get; set; } = null;
    public string PaymentBraintreeLiveEncryptionKey { get; set; }
    [AllowNull]
    public string PaymentBraintreeTestMerchantId { get; set; } = null;
    [AllowNull]
    public string PaymentBraintreeTestPublicKey { get; set; } = null;
    [AllowNull]
    public string PaymentBraintreeTestPrivateKey { get; set; } = null;
    public string PaymentBraintreeTestEncryptionKey { get; set; }
    public bool PaymentBraintreeEnableTestMode { get; set; }
    public bool PaymentPaypalEnableTestMode { get; set; }
    public bool PaymentEnableInvoice { get; set; }
    [AllowNull]
    public string PaymentInvoiceEmail { get; set; } = null;
    public int PaymentDelayNotifications { get; set; } = 1;
    public bool PaymentAskBilling { get; set; } 
    public bool PaymentAskShipping { get; set; }
    public bool PaymentEnableTax { get; set; }
    public decimal PaymentTaxRate { get; set; } = decimal.Parse("0.00");
    public bool PaymentEnableDiscount { get; set; } 
    public string PaymentDiscountType { get; set; } = "percent_off";
    public decimal PaymentDiscountAmount { get; set; } = decimal.Parse("0.00");
    public string PaymentDiscountCode { get; set; }
    [AllowNull]
    public int PaymentDiscountElementId { get; set; } = default(int);
    public bool PaymentDiscountMaxUsage { get; set; } 
    [AllowNull]
    public DateTime PaymentDiscountExpiryDate { get; set; } = default(DateTime);
    public bool LogicFieldEnable { get; set; } 
    public bool LogicPageEnable { get; set; } 
    public bool LogicEmailEnable { get; set; }
    public bool LogicWebhookEnable { get; set; }
    public bool LogicSuccessEnable { get; set; }
    public bool WebhookEnable { get; set; }
    public string WebhookUrl { get; set; }
    public string WebhookMethod { get; set; } = "post";
    public bool EncryptionEnable { get; set; } 
    public string EncryptionPublicKey { get; set; }
    public bool EntryEditEnable { get; set; } 
    public bool EntryEditResendNotifications { get; set; }
    public bool EntryEditRerunLogics { get; set; }
    public bool EntryEditAutoDisable { get; set; } 
    public bool EntryEditAutoDisablePeriod { get; set; } = true;
    public FormEntryEditAutoDisableUnit EntryEditAutoDisableUnit { get; set; } = (FormEntryEditAutoDisableUnit)1;
    public bool EntryEditHideEditlink { get; set; }
    public bool KeywordBlockingEnable { get; set; }
    public string KeywordBlockingList { get; set; }
    public string EntriesSortBy { get; set; } = "id-desc";
    public int EntriesEnableFilter { get; set; } = 0;
    public EntriesFilterType EntriesFilterType { get; set; } = (EntriesFilterType)0;


    [InverseProperty("Forms")]
    public virtual FormStatDto FormStatInfo { get; set; }
    [InverseProperty("Forms")]
    public virtual FormElementDto FormElementInfo { get; set; }
}

public enum FormUniqueIpPeriod
{
    H = 1, //hour
    D = 2, //day
    W = 3, //week
    M = 4, //month
    Y = 5, //year
    L = 6, //lifetime 
}
public enum FormEntryEditAutoDisableUnit
{
    R = 1,
    H = 2,
    D = 3,
}
public enum EntriesFilterType
{
    All =0,
    Any = 1,
}

