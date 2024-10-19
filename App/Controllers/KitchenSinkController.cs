using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace HtmxDotnet.Controllers
{
    public class KitchenSinkController : Controller
    {
        // GET: KitchenSinkController
        public ActionResult Index()
        {
            return View();
        }
        public IActionResult TypeAheadSearchColor(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Ok();
            }
            var data = new List<string>
            {
                "Red",
                "Blue",
                "Green",
                "Yellow",
                "Orange",
                "Purple",
                "Pink",
                "Brown",
                "Black",
                "White",
                "Gray",
                "Violet",
                "Indigo",
                "Magenta",
                "Cyan",
                "Turquoise",
                "Teal",
                "Olive",
                "Maroon",
                "Navy Blue",
                "Sky Blue",
                "Peach",
                "Salmon",
                "Lavender",
                "Mint Green",
                "Lime Green",
                "Beige",
                "Cream",
                "Coral",
                "Amber",
                "Gold",
                "Silver",
                "Bronze",
                "Burgundy",
                "Rose",
                "Sea Green",
                "Forest Green",
                "Slate Gray",
                "Charcoal",
                "Ivory",
                "Mustard",
                "Periwinkle",
                "Plum",
                "Copper",
                "Emerald",
                "Jade",
                "Ruby",
                "Sapphire",
                "Topaz",
                "Aquamarine"
            };

            var highlightedResults = ProcessTypeAhead(data, query);

            return Content(highlightedResults);
        }

        public IActionResult TypeAheadSearch(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Ok();
            }
            var data = new List<string>
            {
                "Banana Bread",
                "Banana Pudding",
                "Apple Pie",
                "Blueberry Muffin",
                "Chocolate Cake",
                "Cheesecake",
                "Pineapple Upside Down Cake",
                "Strawberry Shortcake",
                "Lemon Meringue Pie",
                "Peach Cobbler",
                "Cherry Pie",
                "Pumpkin Pie",
                "Carrot Cake",
                "Red Velvet Cake",
                "Brownies",
                "Tiramisu",
                "Macarons",
                "Cupcakes",
                "Donuts",
                "Cookies",
                "Muffins",
                "Waffles",
                "Pancakes",
                "French Toast",
                "Crepes",
                "Oatmeal Raisin Cookies",
                "Gingerbread Cookies",
                "Sugar Cookies",
                "Snickerdoodles",
                "Raisin Bread",
                "Zucchini Bread",
                "Lemon Bars",
                "Rice Pudding",
                "Bread Pudding",
                "Pecan Pie",
                "Mango Sorbet",
                "Raspberry Cheesecake",
                "Blueberry Pie",
                "Apple Crisp",
                "Cinnamon Rolls",
                "Doughnuts",
                "Lemon Cake",
                "Almond Cake",
                "Churros",
                "Fruit Tart",
                "Peach Melba",
                "Chocolate Chip Cookies",
                "Granola Bars",
                "Apple Fritters",
                "Coconut Macaroons"
            };

            var highlightedResults = ProcessTypeAhead(data, query);

            return Content(highlightedResults);
        }

        private string ProcessTypeAhead(List<string> data, string query)
        {

            var results = data
                .Where(d => d.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(d => d.IndexOf(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var highlightedResults = HighlightMatch(results, query);

            //return Content(highlightedResults);

            return highlightedResults;
        }

        private string HighlightMatch(List<string> results, string query)
        {
            var stringBuilder = new StringBuilder();

            foreach (var result in results)
            {
                // Start by appending the opening span tag with the correct class
                stringBuilder.Append($"<span class=\"suggestion\">");

                int startIndex = result.IndexOf(query, StringComparison.OrdinalIgnoreCase);
                int currentIndex = 0;

                while (startIndex >= 0)
                {
                    // Append text before the match
                    stringBuilder.Append(result.AsSpan(currentIndex, startIndex - currentIndex));

                    // Append the matched part with span and highlight class
                    stringBuilder.Append("<span class=\"highlight\">");
                    stringBuilder.Append(result.AsSpan(startIndex, query.Length));
                    stringBuilder.Append("</span>");

                    // Move the currentIndex past the end of the current match
                    currentIndex = startIndex + query.Length;

                    // Search for the next occurrence starting from the new position
                    startIndex = result.IndexOf(query, currentIndex, StringComparison.OrdinalIgnoreCase);
                }

                // Append the remaining part of the text after the last match
                stringBuilder.Append(result.AsSpan(currentIndex));

                // Close the span tag for the suggestion
                stringBuilder.Append("</span>");

                // Add a separator between results (e.g., comma)
                stringBuilder.Append(' ');
            }

            // Remove the trailing comma and space if needed
            if (stringBuilder.Length > 1)
            {
                stringBuilder.Length -= 1;
            }

            return stringBuilder.ToString();
        }

    }
}
