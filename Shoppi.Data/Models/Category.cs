using System.Collections.Generic;

namespace Shoppi.Data.Models
{
    public class Category
    {
        public Category(string name, Category parentCategory = null)
        {
            SubCategories = new List<Category>();
            Name = name;

            if (parentCategory != null)
            {
                parentCategory.SubCategories.Add(this);
            }
        }

        private Category()
        {
            SubCategories = new List<Category>();
        }

        public int Id { get; protected set; }
        public List<Category> SubCategories { get; protected set; }

        public string Name { get; protected set; }
    }
}