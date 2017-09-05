using System.Collections.Generic;

namespace Shoppi.Data.Models
{
    public class Category
    {
        public Category(string name, Category headCategory = null)
        {
            Name = name;
            HeadCategory = headCategory;
        }

        public Category()
        {
            SubCategories = new List<Category>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int? HeadCategoryId { get; set; }

        public virtual Category HeadCategory { get; set; }

        public virtual List<Category> SubCategories { get; set; }

        public string GetFullCategoryPathName()
        {
            if (HeadCategory != null)
            {
                return HeadCategory.GetFullCategoryPathName() + ">" + Name;
            }

            return Name;
        }
    }
}