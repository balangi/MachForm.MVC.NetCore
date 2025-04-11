using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace Convertify.Controllers
{
    public class HomeController : Controller
    {
        private static string EnumDefinition = "";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ToPascalCase()
        {
            var model = new ConvertViewModel();
            return View(model);
        }


        [HttpPost]
        public IActionResult ToPascalCase(ConvertViewModel model)
        {
            var model1 = model.Original.Split('\n');
            var str1 = "";

            foreach (var item in model1)
            {
                str1 += ConvertToPascalCase(item);
            }

            model.Converted = str1;
            return View(model);
        }

        public IActionResult ToDataModel()
        {
            string[] testCases = {
                //"`id`  int(11) unsigned NOT NULL AUTO_INCREMENT,",
                //"",
                "`payment_discount_amount` decimal(62,2) NOT NULL DEFAULT '0.00',",
            };
            EnumDefinition = "";

            var model = new ConvertViewModel();

            foreach (var testCase in testCases)
            {
                model.Original += testCase + "\n";
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult ToDataModel(ConvertViewModel model)
        {
            var model1 = model.Original.Split('\n');
            var str1 = "";

            foreach (var item in model1)
            {
                str1 += ConvertToProperty(item) + "\n";
            }

            model.Converted = $"{str1}\n\n{EnumDefinition}";
            return View(model);
        }

        public IActionResult ReverseCharacter()
        {
            string[] testCases = {
                "تيقالخ ىاه کينكت\r\nرپمكسا کينكت\r\n(Modify) ندرك حالصا و نداد رييغت\r\nدوخ دنيآرف اي و تمدخ ،لوصحم رد ار ىزيچ هچ ؛دنتسه اه نيا دينك زكرمت نا ىور دياب هك ىتالاوس ،ىلك روط\r\nايآ ؟مهد رييغت ار لوصحم ىعون ةب مناوت ايآ ؟منك نآ ىور ىرتشيب ة رتمك ديكات اي و منك حالصا مناوت يم\r\nىم\r\n؟مهد رييغت ار نا لكش اي و مرف ،وب Asp Net ،ادص ،تك رح ،گنر ،موهفم مناوت\r\n؛ىليمكت تالاوس\r\n؟منكرت گرزبمتات ﯽمار ﯥباهزيچ هچم\r\n؟درك فذح اي داد شهاك asp.net ناوت ىم ار ىياهزيچ هچ ٠\r\n؟منك ىيامنگ رزب اي قارغا .. و اه هزادنا ،اه گنر ،اه همكد ىريك راك هب رد مناوت ىم ايأ\r\n؟دوش رت ىوق اي و رتهب دناوت ىم ىزيچ هچ م\r\nهباشون ،ىراوس ىاهوردوخ ىاج هب نو ىاهنيشام ،ىلومعم ىازتيپ ىاج هب هداوناخ ىازتيي تخاس ؛زا دنا ترابع تالوصحم ندش رتگرزب زا ىياه هنومن ؛لاثم\r\nنايرتشم زا ىديدج هورگ هدافتسا ىارب تالوصحم ىخرب لباقم رد و ،چيواطع ىاه چيودناس لدمر هرفن ود ىاه چيودناس ،كچوك هباشون ىاج هب هداوناخ\r\nاه هچب ىزاب صوصخم کچوك ىاه نيشام و روتوم تخاس اي و ناكدوك ىارب اه رتچ زياس ندرك كچوك لاثم ىارب ؛دندش کچوك",
            };
            EnumDefinition = "";

            var model = new ConvertViewModel();

            foreach (var testCase in testCases)
            {
                model.Original += testCase + "\n";
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult ReverseCharacter(ConvertViewModel model)
        {
            var splitedOriginal = model.Original.Split("\r\n", StringSplitOptions.None);

            var ar = new ArrayList();
            foreach (var item in splitedOriginal)
            {
                ar.Add(ReverseString(item));
            }

            var converted = string.Join("\r\n", ar.ToArray());
            var converted1 = ExtractAllLatinTerms(converted);
            var converted2 = converted1.Split(' ');

            foreach (var item in converted2) {
                converted = converted.Replace(item, ReverseString(item));
            }

            model.Converted = converted;

            return View(model);
        }

        string ExtractAllLatinTerms(string text)
        {
            // این الگو کلمات لاتین با اعداد و برخی علائم خاص را نیز تشخیص می‌دهد
            Regex regex = new Regex(@"\b[\w\.@-]+\b", RegexOptions.Compiled);

            var matches = regex.Matches(text)
                             .Cast<Match>()
                             .Where(m => Regex.IsMatch(m.Value, "[a-zA-Z]"))
                             .Select(m => m.Value);

            return string.Join(" ", matches);
        }

        public static string ReverseString(string input)
        {
            return new string(input.Reverse().ToArray());
        }

        public static string ConvertToProperty(string dbColumnDefinition)
        {
            if (string.IsNullOrWhiteSpace(dbColumnDefinition))
                return dbColumnDefinition;

            dbColumnDefinition = dbColumnDefinition.Replace("`", "");

            // استخراج نام فیلد با Regex پیشرفته‌تر
            var fieldNameMatch = Regex.Match(dbColumnDefinition.Trim(),
                @"^([a-zA-Z_][a-zA-Z0-9_]*)\s",
                RegexOptions.IgnoreCase);

            if (!fieldNameMatch.Success)
            {
                // اگر نام فیلد استخراج نشد، کل رشته برگردانده شود
                return dbColumnDefinition;
            }

            string columnName = fieldNameMatch.Groups[1].Value;
            string propertyName = ConvertToPascalCase(columnName);

            var attribute1 = Regex.IsMatch(dbColumnDefinition, @"\b(auto_increment)\b", RegexOptions.IgnoreCase)
                ? "[Key] \n"
                : "";

            var attribute2 = Regex.IsMatch(dbColumnDefinition, @"\b(default null)\b", RegexOptions.IgnoreCase)
               ? "[AllowNull] \n"
               : "";

            // تشخیص نوع داده با دقت بیشتر
            string propertyType = DetectPropertyType(dbColumnDefinition);

            // استخراج مقدار پیش‌فرض
            string defaultValue = DetectDefaultValue(dbColumnDefinition, propertyType);

            // استخراج مقادیر enum از کامنت
            var commentMatch = Regex.Match(dbColumnDefinition, @"COMMENT\s+'([^']+)'");
            string enumValues = commentMatch.Success ? commentMatch.Groups[1].Value : string.Empty;

            // تولید enum
            string enumDefinition = GenerateEnum(propertyName, enumValues);

            if (!string.IsNullOrEmpty(enumDefinition))
            {
                EnumDefinition += enumDefinition;
            }

            propertyType = !string.IsNullOrWhiteSpace(commentMatch.ToString())
                //? "int" //
                ? propertyName
                : propertyType;

            // ساخت property
            string property = $"{attribute1}{attribute2}public {propertyType} {propertyName} {{ get; set; }}";

            if (!string.IsNullOrEmpty(defaultValue) && !string.IsNullOrWhiteSpace(commentMatch.ToString()))
                property += $" = {defaultValue.Replace("\"", "")};";
            else if ((!string.IsNullOrEmpty(defaultValue) && propertyType == "int") || (!string.IsNullOrEmpty(defaultValue) && propertyType == "long"))
                property += $" = {defaultValue.Replace("\"", "")};";
            else if ((!string.IsNullOrEmpty(defaultValue) && propertyType == "decimal"))
                property += $" = decimal.Parse({defaultValue.Replace("'", "\"")});";
            else if (!string.IsNullOrEmpty(defaultValue) && string.IsNullOrWhiteSpace(commentMatch.ToString()))
                property += $" = {defaultValue};";

            return property;
        }

        private static string DetectPropertyType(string dbColumnDefinition)
        {
            if (Regex.IsMatch(dbColumnDefinition, @"\b(int|integer|tinyint|smallint|mediumint)\b", RegexOptions.IgnoreCase))
                return "int";
            if (Regex.IsMatch(dbColumnDefinition, @"\b(bigint)\b", RegexOptions.IgnoreCase))
                return "long";
            if (Regex.IsMatch(dbColumnDefinition, @"\b(bool|boolean|bit)\b", RegexOptions.IgnoreCase))
                return "bool";
            if (Regex.IsMatch(dbColumnDefinition, @"\b(datetime|timestamp|date|time)\b", RegexOptions.IgnoreCase))
                return "DateTime";
            if (Regex.IsMatch(dbColumnDefinition, @"\b(decimal|numeric|money)\b", RegexOptions.IgnoreCase))
                return "decimal";
            if (Regex.IsMatch(dbColumnDefinition, @"\b(float|real)\b", RegexOptions.IgnoreCase))
                return "float";
            if (Regex.IsMatch(dbColumnDefinition, @"\b(double)\b", RegexOptions.IgnoreCase))
                return "double";
            //if (Regex.IsMatch(dbColumnDefinition, @"\b(bit)\b", RegexOptions.IgnoreCase))
            //    return "bool";

            // نوع پیش‌فرض برای سایر موارد
            return "string";
        }

        private static string DetectDefaultValue(string dbColumnDefinition, string propertyType)
        {
            var defaultValueMatch = Regex.Match(dbColumnDefinition,
                @"DEFAULT\s+([^\s,]+)",
                RegexOptions.IgnoreCase);

            if (!defaultValueMatch.Success)
                return null;

            string dbDefaultValue = defaultValueMatch.Groups[1].Value;

            // تبدیل مقدار پیش‌فرض به فرمت سی‌شارپ
            if (dbDefaultValue.Equals("NULL", StringComparison.OrdinalIgnoreCase))
            {
                return propertyType == "string" ? "null" :
                       propertyType.EndsWith("?") ? "null" :
                       $"default({propertyType})";
            }

            if (propertyType == "string" || propertyType == "int")
                return $"\"{dbDefaultValue.Replace("'", "")}\"";

            if (propertyType == "bool")
                return dbDefaultValue.Equals("1", StringComparison.Ordinal) ? "true" :
                       dbDefaultValue.Equals("0", StringComparison.Ordinal) ? "false" :
                       dbDefaultValue.ToLower();

            return dbDefaultValue;
        }

        private static string GenerateEnum(string enumName, string values)
        {
            if (string.IsNullOrEmpty(values))
                return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine($"public enum {enumName}");
            sb.AppendLine("{");

            var items = values.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < items.Length; i++)
            {
                string item = items[i].Trim();
                string enumItem = ConvertToPascalCase(item);
                sb.AppendLine($"    {enumItem} = {i + 1},");
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string ConvertToPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var parts = input.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new StringBuilder();

            foreach (var part in parts)
            {
                if (part.Length > 0)
                {
                    result.Append(char.ToUpper(part[0]));
                    if (part.Length > 1)
                        result.Append(part.Substring(1).ToLower());
                }
            }

            return result.ToString();
        }
    }
}
