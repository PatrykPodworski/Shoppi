using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Shoppi.Helpers
{
    public static class ProductHtmlHelperExtensions
    {
        public static MvcHtmlString HiddenForCategories(this HtmlHelper htmlHelper, ICollection<Category> categories)
        {
            var html = new StringBuilder();

            int index = 0;

            foreach (var category in categories)
            {
                html.AppendLine(htmlHelper.Hidden($"Categories[{index}].Id", category.Id).ToString());
                html.AppendLine(htmlHelper.Hidden($"Categories[{index}].Name", category.Name).ToString());
                index++;
            }

            return MvcHtmlString.Create(html.ToString());
        }
    }
}