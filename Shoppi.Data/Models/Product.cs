using System;

namespace Shoppi.Data.Models
{
    public class Product
    {
        private string _name;
        private Category _category;

        public Product(string name, Category category, int quantity = 0)
        {
            Name = name;
            Quantity = quantity;
            Category = category;
        }

        private Product()
        {
        }

        public int Id { get; protected set; }

        public Category Category
        {
            get => _category;
            set
            {
                ValidateCategory(value);
                _category = value;
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                ValidateName(value);
                _name = value;
            }
        }

        public int Quantity { get; protected set; }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid name.");
            }
        }

        private void ValidateCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException("Category can't be null.");
            }
        }
    }
}