using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MachForm.NetCore.Models.Forms;

public class FormDto
{
    [Key]
    public int FormId { get; set; }
    public string FormName { get; set; }
    public string FormDescription { get; set; }
    public int FormNameHide { get; set; } = 0;
    [AllowNull]
    public string FormTags { get; set; } = null;
    public string FormEmail { get; set; }
    public string FormRedirect { get; set; }
    public int FormRedirectEnable { get; set; } = 0;
    public string FormSuccessMessage { get; set; }
    public string FormDisabledMessage { get; set; }
    [AllowNull]
    public string FormPassword { get; set; } = null;
    public int FormUniqueIp { get; set; } = 0;
    public int FormUniqueIpMaxcount { get; set; } = 5;
    public FormUniqueIpPeriod FormUniqueIpPeriod { get; set; } = (FormUniqueIpPeriod)2;
    [AllowNull]
    public int FormFrameHeight { get; set; } = default(int);
    public int FormHasCss { get; set; } = 0;
    public int FormCaptcha { get; set; } = 0;
    public string FormCaptchaType { get; set; } = "n";
    public int FormActive { get; set; } = 1;
    public int FormThemeId { get; set; } = 0;
    public int FormReview { get; set; } = 0;
    public int FormResumeEnable { get; set; } = 0;
    public string FormResumeSubject { get; set; }
    public string FormResumeContent { get; set; }
    public string FormResumeFromName { get; set; }
    public string FormResumeFromEmailAddress { get; set; } = "";
    public int FormCustomScriptEnable { get; set; } = 0;
    public string FormCustomScriptUrl { get; set; }
    public int FormLimitEnable { get; set; } = 0;
    public int FormLimit { get; set; } = 0;
    public string FormLabelAlignment { get; set; } = "top_label";
    [AllowNull]
    public string FormLanguage { get; set; } = null;
    public int FormPageTotal { get; set; } = 1;
    [AllowNull]
    public string FormLastpageTitle { get; set; } = null;
    public string FormSubmitPrimaryText { get; set; } = "Submit";
    public string FormSubmitSecondaryText { get; set; } = "Previous";
    [AllowNull]
    public string FormSubmitPrimaryImg { get; set; } = null;
    [AllowNull]
    public string FormSubmitSecondaryImg { get; set; } = null;
    public int FormSubmitUseImage { get; set; } = 0;
    public string FormReviewPrimaryText { get; set; } = "Submit";
    public string FormReviewSecondaryText { get; set; } = "Previous";
    [AllowNull]
    public string FormReviewPrimaryImg { get; set; } = null;
    [AllowNull]
    public string FormReviewSecondaryImg { get; set; } = null;
    public int FormReviewUseImage { get; set; } = 0;
    public string FormReviewTitle { get; set; }
    public string FormReviewDescription { get; set; }
    public string FormPaginationType { get; set; } = "steps";
    public int FormScheduleEnable { get; set; } = 0;
    [AllowNull]
    public DateTime FormScheduleStartDate { get; set; } = default(DateTime);
    [AllowNull]
    public DateTime FormScheduleEndDate { get; set; } = default(DateTime);
    [AllowNull]
    public DateTime FormScheduleStartHour { get; set; } = default(DateTime);
    [AllowNull]
    public DateTime FormScheduleEndHour { get; set; } = default(DateTime);
    public int FormApprovalEnable { get; set; } = 0;
    public string FormApprovalEmailSubject { get; set; }
    public string FormApprovalEmailContent { get; set; }
    [AllowNull]
    public DateTime FormCreatedDate { get; set; } = default(DateTime);
    [AllowNull]
    public int FormCreatedBy { get; set; } = default(int);
    public int EslEnable { get; set; } = 0;
    public string EslFromName { get; set; }
    [AllowNull]
    public string EslFromEmailAddress { get; set; } = null;
    public string EslBccEmailAddress { get; set; }
    [AllowNull]
    public string EslReplytoEmailAddress { get; set; } = null;
    public string EslSubject { get; set; }
    public string EslContent { get; set; }
    public int EslPlainText { get; set; } = 0;
    public int EslPdfEnable { get; set; } = 0;
    public string EslPdfContent { get; set; }
    public int EsrEnable { get; set; } = 0;
    public string EsrEmailAddress { get; set; }
    public string EsrFromName { get; set; }
    [AllowNull]
    public string EsrFromEmailAddress { get; set; } = null;
    public string EsrBccEmailAddress { get; set; }
    [AllowNull]
    public string EsrReplytoEmailAddress { get; set; } = null;
    public string EsrSubject { get; set; }
    public string EsrContent { get; set; }
    public int EsrPlainText { get; set; } = 0;
    public int EsrPdfEnable { get; set; } = 0;
    public string EsrPdfContent { get; set; }
    public int PaymentEnableMerchant { get; set; } = 0;
    public string PaymentMerchantType { get; set; } = "paypal_standard";
    [AllowNull]
    public string PaymentPaypalEmail { get; set; } = null;
    public string PaymentPaypalLanguage { get; set; } = "US";
    public string PaymentCurrency { get; set; } = "USD";
    public int PaymentShowTotal { get; set; } = 0;
    public string PaymentTotalLocation { get; set; } = "top";
    public int PaymentEnableRecurring { get; set; } = 0;
    public int PaymentRecurringCycle { get; set; } = 1;
    public string PaymentRecurringUnit { get; set; } = "month";
    public int PaymentEnableTrial { get; set; } = 0;
    public int PaymentTrialPeriod { get; set; } = 1;
    public string PaymentTrialUnit { get; set; } = "month";
    public decimal PaymentTrialAmount { get; set; } = decimal.Parse("0.00");
    public int PaymentEnableSetupfee { get; set; } = 0;
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
    public int PaymentStripeEnableTestMode { get; set; } = 0;
    public int PaymentStripeEnableReceipt { get; set; } = 0;
    [AllowNull]
    public int PaymentStripeReceiptElementId { get; set; } = default(int);
    public int PaymentStripeEnablePaymentRequestButton { get; set; } = 0;
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
    public int PaymentAuthorizenetEnableTestMode { get; set; } = 0;
    public int PaymentAuthorizenetSaveCcData { get; set; } = 0;
    public int PaymentAuthorizenetEnableEmail { get; set; } = 0;
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
    public int PaymentBraintreeEnableTestMode { get; set; } = 0;
    public int PaymentPaypalEnableTestMode { get; set; } = 0;
    public int PaymentEnableInvoice { get; set; } = 0;
    [AllowNull]
    public string PaymentInvoiceEmail { get; set; } = null;
    public int PaymentDelayNotifications { get; set; } = 1;
    public int PaymentAskBilling { get; set; } = 0;
    public int PaymentAskShipping { get; set; } = 0;
    public int PaymentEnableTax { get; set; } = 0;
    public decimal PaymentTaxRate { get; set; } = decimal.Parse("0.00");
    public int PaymentEnableDiscount { get; set; } = 0;
    public string PaymentDiscountType { get; set; } = "percent_off";
    public decimal PaymentDiscountAmount { get; set; } = decimal.Parse("0.00");
    public string PaymentDiscountCode { get; set; }
    [AllowNull]
    public int PaymentDiscountElementId { get; set; } = default(int);
    public int PaymentDiscountMaxUsage { get; set; } = 0;
    [AllowNull]
    public DateTime PaymentDiscountExpiryDate { get; set; } = default(DateTime);
    public int LogicFieldEnable { get; set; } = 0;
    public int LogicPageEnable { get; set; } = 0;
    public int LogicEmailEnable { get; set; } = 0;
    public int LogicWebhookEnable { get; set; } = 0;
    public int LogicSuccessEnable { get; set; } = 0;
    public int WebhookEnable { get; set; } = 0;
    public string WebhookUrl { get; set; }
    public string WebhookMethod { get; set; } = "post";
    public int FormEncryptionEnable { get; set; } = 0;
    public string FormEncryptionPublicKey { get; set; }
    public int FormEntryEditEnable { get; set; } = 0;
    public int FormEntryEditResendNotifications { get; set; } = 0;
    public int FormEntryEditRerunLogics { get; set; } = 0;
    public int FormEntryEditAutoDisable { get; set; } = 0;
    public int FormEntryEditAutoDisablePeriod { get; set; } = 1;
    public FormEntryEditAutoDisableUnit FormEntryEditAutoDisableUnit { get; set; } = (FormEntryEditAutoDisableUnit)1;
    public int FormEntryEditHideEditlink { get; set; } = 0;
    public int FormKeywordBlockingEnable { get; set; } = 0;
    public string FormKeywordBlockingList { get; set; }
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

