namespace Shoppi.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Simplifiedcategory : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Products", name: "Category_Id", newName: "CategoryId");
            RenameIndex(table: "dbo.Products", name: "IX_Category_Id", newName: "IX_CategoryId");
            AlterColumn("dbo.Categories", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "Name", c => c.String());
            RenameIndex(table: "dbo.Products", name: "IX_CategoryId", newName: "IX_Category_Id");
            RenameColumn(table: "dbo.Products", name: "CategoryId", newName: "Category_Id");
        }
    }
}
