using System;
using System.Collections.Generic;

namespace Shoppi.Data.Models
{
    public class Category
    {
        public Category(string name, Category parentCategory = null)
        {
            SubCategories = new List<Category>();
            ValidateName(name);
            Name = name;

            if (parentCategory != null)
            {
                parentCategory.SubCategories.Add(this);
            }
        }

        public int Id { get; protected set; }
        public List<Category> SubCategories { get; protected set; }

        public string Name { get; protected set; }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid name.");
            }
        }
    }
}