namespace Shoppi.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedheadCategoryandremovedlistofsubCategories : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Categories", name: "Category_Id", newName: "HeadCategoryId");
            RenameIndex(table: "dbo.Categories", name: "IX_Category_Id", newName: "IX_HeadCategoryId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Categories", name: "IX_HeadCategoryId", newName: "IX_Category_Id");
            RenameColumn(table: "dbo.Categories", name: "HeadCategoryId", newName: "Category_Id");
        }
    }
}
