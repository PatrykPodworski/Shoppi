using Shoppi.Data.Models;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Shoppi.Helpers
{
    public static class HtmlHelperExtensions
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

        public static MvcHtmlString HiddenForSelectListItem(this HtmlHelper htmlHelper, ICollection<SelectListItem> items, string collectionName)
        {
            var html = new StringBuilder();

            var index = 0;

            foreach (var item in items)
            {
                html.AppendLine(htmlHelper.Hidden($"{collectionName}[{index}].Value", item.Value).ToString());
                html.AppendLine(htmlHelper.Hidden($"{collectionName}[{index}].Text", item.Text).ToString());
                index++;
            }

            return MvcHtmlString.Create(html.ToString());
        }
    }
}