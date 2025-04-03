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
