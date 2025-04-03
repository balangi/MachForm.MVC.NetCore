using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using MachForm.NetCore.Models.Forms;

namespace MachForm.NetCore.Models.Elements;

public class FormElementDto
{
    [Key]
    public int Id { get; set; }
    public int FormId { get; set; } = 0;
    public string Title { get; set; }
    public string Guidelines { get; set; }
    public string Size { get; set; } = "medium";
    public int IsRequired { get; set; } = 0;
    public int IsUnique { get; set; } = 0;
    public int IsReadonly { get; set; } = 0;
    public IsPrivate IsPrivate { get; set; } = (IsPrivate)0;
    public int IsEncrypted { get; set; } = 0;
    [AllowNull]
    public string Type { get; set; } = null;
    public int Position { get; set; } = 0;
    public string DefaultValue { get; set; }
    public int EnablePlaceholder { get; set; } = 0;
    [AllowNull]
    public string Constraint { get; set; } = null;
    public int TotalChild { get; set; } = 0;
    public string ChoiceLimitRule { get; set; } = "atleast";
    public int ChoiceLimitQty { get; set; } = 1;
    public int ChoiceLimitRangeMin { get; set; } = 1;
    public int ChoiceLimitRangeMax { get; set; } = 1;
    public int ChoiceMaxEntry { get; set; } = 0;
    public int DateDisableDayofweek { get; set; } = 0;
    [AllowNull]
    public string DateDisabledDayofweekList { get; set; } = null;
    public int NumberEnableQuantity { get; set; } = 0;
    [AllowNull]
    public string NumberQuantityLink { get; set; } = null;
    public int SectionDisplayInEmail { get; set; } = 1;
    public int SectionEnableScroll { get; set; } = 0;




    public string TextDefaultType { get; set; } = "static";
    public int TextDefaultLength { get; set; } = 10;
    public string TextDefaultRandomType { get; set; } = "letter";
    [AllowNull]
    public string TextDefaultPrefix { get; set; } = null;
    public string TextDefaultCase { get; set; } = "u";
    public int EmailEnableConfirmation { get; set; } = 0;
    [AllowNull]
    public string EmailConfirmFieldLabel { get; set; } = null;
    public string MediaType { get; set; } = "image";
    public string MediaImageSrc { get; set; }
    [AllowNull]
    public int MediaImageWidth { get; set; } = default(int);
    [AllowNull]
    public int MediaImageHeight { get; set; } = default(int);
    public string MediaImageAlignment { get; set; } = "l";
    public string MediaImageAlt { get; set; }
    public string MediaImageHref { get; set; }
    public int MediaDisplayInEmail { get; set; } = 0;
    public string MediaVideoSrc { get; set; }
    public string MediaVideoSize { get; set; } = "large";
    public int MediaVideoMuted { get; set; } = 0;
    public string MediaVideoCaptionFile { get; set; }
    public string MediaPdfSrc { get; set; }
    public string RatingStyle { get; set; } = "star";
    public int RatingMax { get; set; } = 5;
    public int RatingDefault { get; set; } 
    public int RatingEnableLabel { get; set; } = 0;
    public string RatingLabelHigh { get; set; } = "Best";
    public string RatingLabelLow { get; set; } = "Worst";
    public string AddressSubfieldsVisibility { get; set; }
    public string AddressSubfieldsLabels { get; set; }
    [AllowNull]
    public string AddressDefaultState { get; set; } = null;



    public string CssClass { get; set; } = "";
    public long RangeMin { get; set; } = '0';
    public long RangeMax { get; set; } = '0';
    public string RangeLimitBy { get; set; }
    public int Status { get; set; } = 2;
    public int ChoiceColumns { get; set; } = 1;
    public int ChoiceHasOther { get; set; } = 0;
    public string ChoiceOtherLabel { get; set; }
    public int TimeShowsecond { get; set; } = 0;
    public int Time24hour { get; set; } = 0;
    public int AddressHideline2 { get; set; } = 0;
    public int AddressUsOnly { get; set; } = 0;
    public int DateEnableRange { get; set; } = 0;
    [AllowNull]
    public DateTime DateRangeMin { get; set; } = default(DateTime);
    [AllowNull]
    public DateTime DateRangeMax { get; set; } = default(DateTime);
    public int DateEnableSelectionLimit { get; set; } = 0;
    public int DateSelectionMax { get; set; } = 1;
    public string DatePastFuture { get; set; } = "p";
    public bool DateDisablePastFuture { get; set; }
    public bool DateDisableWeekend { get; set; }
    public bool DateDisableSpecific { get; set; } 
    public string DateDisabledList { get; set; }
    public int FileEnableTypeLimit { get; set; } = 1;
    public string FileBlockOrAllow { get; set; } = "b";
    [AllowNull]
    public string FileTypeList { get; set; } = null;
    public int FileAsAttachment { get; set; } = 0;
    public int FileEnableAdvance { get; set; } = 0;
    public int FileAutoUpload { get; set; } = 0;
    public int FileEnableMultiUpload { get; set; } = 0;
    public int FileMaxSelection { get; set; } = 5;
    public int FileEnableSizeLimit { get; set; } = 0;
    [AllowNull]
    public int FileSizeMax { get; set; } = default(int);
    public int MatrixAllowMultiselect { get; set; } = 0;
    public int MatrixParentId { get; set; } = 0;
    public int SubmitUseImage { get; set; } = 0;
    public string SubmitPrimaryText { get; set; } = "Continue";
    public string SubmitSecondaryText { get; set; } = "Previous";
    [AllowNull]
    public string SubmitPrimaryImg { get; set; } = null;
    [AllowNull]
    public string SubmitSecondaryImg { get; set; } = null;
    [AllowNull]
    public string PageTitle { get; set; } = null;
    public int PageNumber { get; set; } = 1;


    [InverseProperty(nameof(FormDto.FormElementInfo))]
    public ICollection<FormDto> Forms { get; set; }
}

public enum IsPrivate
{
    Public = 0,
    AdminOnly = 1,
    Hidden = 2,
}

