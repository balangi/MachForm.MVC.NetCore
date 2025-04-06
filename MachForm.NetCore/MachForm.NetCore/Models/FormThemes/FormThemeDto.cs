using System.ComponentModel.DataAnnotations;

namespace MachForm.NetCore.Models.FormThemes;

public class FormThemeDto
{
    [Key]
    public int ThemeId { get; set; }
    public string UserId { get; set; } 
    public int Status { get; set; } = 1;
    public int ThemeHasCss { get; set; } = 0;
    public string ThemeName { get; set; } = "";
    public int ThemeBuiltIn { get; set; } = 0;
    public int ThemeIsPrivate { get; set; } = 1;
    public LogoType LogoType { get; set; } = (LogoType)1;
    public string LogoCustomImage { get; set; }
    public int LogoCustomHeight { get; set; } = 40;
    public string LogoDefaultImage { get; set; } = "";
    public int LogoDefaultRepeat { get; set; } = 0;
    public WallpaperBgType WallpaperBgType { get; set; } = (WallpaperBgType)1;
    public string WallpaperBgColor { get; set; } = "";
    public string WallpaperBgPattern { get; set; } = "";
    public string WallpaperBgCustom { get; set; }
    public HeaderBgType HeaderBgType { get; set; } = (HeaderBgType)1;
    public string HeaderBgColor { get; set; } = "";
    public string HeaderBgPattern { get; set; } = "";
    public string HeaderBgCustom { get; set; }
    public FormBgType FormBgType { get; set; } = (FormBgType)1;
    public string FormBgColor { get; set; } = "";
    public string FormBgPattern { get; set; } = "";
    public string FormBgCustom { get; set; }
    public HighlightBgType HighlightBgType { get; set; } = (HighlightBgType)1;
    public string HighlightBgColor { get; set; } = "";
    public string HighlightBgPattern { get; set; } = "";
    public string HighlightBgCustom { get; set; }
    public GuidelinesBgType GuidelinesBgType { get; set; } = (GuidelinesBgType)1;
    public string GuidelinesBgColor { get; set; } = "";
    public string GuidelinesBgPattern { get; set; } = "";
    public string GuidelinesBgCustom { get; set; }
    public FieldBgType FieldBgType { get; set; } = (FieldBgType)1;
    public string FieldBgColor { get; set; } = "";
    public string FieldBgPattern { get; set; } = "";
    public string FieldBgCustom { get; set; }
    public string FormTitleFontType { get; set; } = "Lucida";
    public int FormTitleFontWeight { get; set; } = 400;
    public string FormTitleFontStyle { get; set; } = "normal";
    public string FormTitleFontSize { get; set; } = "";
    public string FormTitleFontColor { get; set; } = "";
    public string FormDescFontType { get; set; } = "Lucida";
    public int FormDescFontWeight { get; set; } = 400;
    public string FormDescFontStyle { get; set; } = "normal";
    public string FormDescFontSize { get; set; } = "";
    public string FormDescFontColor { get; set; } = "";
    public string FieldTitleFontType { get; set; } = "Lucida";
    public int FieldTitleFontWeight { get; set; } = 400;
    public string FieldTitleFontStyle { get; set; } = "normal";
    public string FieldTitleFontSize { get; set; } = "";
    public string FieldTitleFontColor { get; set; } = "";
    public string GuidelinesFontType { get; set; } = "Lucida";
    public int GuidelinesFontWeight { get; set; } = 400;
    public string GuidelinesFontStyle { get; set; } = "normal";
    public string GuidelinesFontSize { get; set; } = "";
    public string GuidelinesFontColor { get; set; } = "";
    public string SectionTitleFontType { get; set; } = "Lucida";
    public int SectionTitleFontWeight { get; set; } = 400;
    public string SectionTitleFontStyle { get; set; } = "normal";
    public string SectionTitleFontSize { get; set; } = "";
    public string SectionTitleFontColor { get; set; } = "";
    public string SectionDescFontType { get; set; } = "Lucida";
    public int SectionDescFontWeight { get; set; } = 400;
    public string SectionDescFontStyle { get; set; } = "normal";
    public string SectionDescFontSize { get; set; } = "";
    public string SectionDescFontColor { get; set; } = "";
    public string FieldTextFontType { get; set; } = "Lucida";
    public int FieldTextFontWeight { get; set; } = 400;
    public string FieldTextFontStyle { get; set; } = "normal";
    public string FieldTextFontSize { get; set; } = "";
    public string FieldTextFontColor { get; set; } = "";
    public int BorderFormWidth { get; set; } = 1;
    public string BorderFormStyle { get; set; } = "solid";
    public string BorderFormColor { get; set; } = "";
    public int BorderGuidelinesWidth { get; set; } = 1;
    public string BorderGuidelinesStyle { get; set; } = "solid";
    public string BorderGuidelinesColor { get; set; } = "";
    public int BorderSectionWidth { get; set; } = 1;
    public string BorderSectionStyle { get; set; } = "solid";
    public string BorderSectionColor { get; set; } = "";
    public string FormShadowStyle { get; set; } = "WarpShadow";
    public string FormShadowSize { get; set; } = "large";
    public string FormShadowBrightness { get; set; } = "normal";
    public string FormButtonType { get; set; } = "text";
    public string FormButtonText { get; set; } = "Submit";
    public string FormButtonImage { get; set; }
    public string AdvancedCss { get; set; }
}

public enum LogoType
{
    Default = 1,
    Custom = 2,
    Disabled = 3,
}
public enum WallpaperBgType
{
    Color = 1,
    Pattern = 2,
    Custom = 3,
}
public enum HeaderBgType
{
    Color = 1,
    Pattern = 2,
    Custom = 3,
}
public enum FormBgType
{
    Color = 1,
    Pattern = 2,
    Custom = 3,
}
public enum HighlightBgType
{
    Color = 1,
    Pattern = 2,
    Custom = 3,
}
public enum GuidelinesBgType
{
    Color = 1,
    Pattern = 2,
    Custom = 3,
}
public enum FieldBgType
{
    Color = 1,
    Pattern = 2,
    Custom = 3,
}
