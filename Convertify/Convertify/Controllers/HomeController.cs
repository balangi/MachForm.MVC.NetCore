using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Convertify.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new PascalCase();
            return View(model);
        }

        //public IActionResult Index()
        //{
        //    string[] testCases = {
        //        "base_url",
        //        "user_id",
        //        "first_name",
        //        "total_amount_usd",
        //        "alreadyPascalCase",
        //        ""
        //    };

        //    var model = new ArrayList();

        //    for (int i = 0; i < testCases.Length; i++)
        //    {
        //        model.Add(new PascalCase { Original = testCases[i], Converted = ConvertToPascalCase(testCases[i]) });
        //    }
        //    return View(model);
        //}

        public static string ConvertToPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            var parts = input.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new StringBuilder();

            foreach (var part in parts)
            {
                if (part.Length > 0)
                {
                    result.Append(textInfo.ToTitleCase(part.ToLower()));
                }
            }

            return result.ToString();
        }
    }
}
