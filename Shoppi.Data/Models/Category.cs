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
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int? HeadCategoryId { get; set; }

        public virtual Category HeadCategory { get; set; }
    }
}