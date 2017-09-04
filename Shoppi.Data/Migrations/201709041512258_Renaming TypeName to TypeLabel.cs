namespace Shoppi.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamingTypeNametoTypeLabel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "TypeLabel", c => c.String());
            DropColumn("dbo.Products", "TypeName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "TypeName", c => c.String());
            DropColumn("dbo.Products", "TypeLabel");
        }
    }
}
