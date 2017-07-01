using System;
using System.Collections.Generic;

namespace Shoppi.Data.Models
{
    public class Category
    {
        public Category(string name)
        {
            SubCategories = new List<Category>();
            ValidateName(name);
            Name = name;
        }

        public List<Category> SubCategories { get; set; }

        public string Name { get; private set; }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid name.");
            }
        }
    }
}