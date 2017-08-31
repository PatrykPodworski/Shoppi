namespace Shoppi.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedtypesforproductandremovedproductquantityitisintypesnow : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            AddColumn("dbo.Products", "TypeName", c => c.String());
            DropColumn("dbo.Products", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Quantity", c => c.Int(nullable: false));
            DropForeignKey("dbo.Types", "ProductId", "dbo.Products");
            DropIndex("dbo.Types", new[] { "ProductId" });
            DropColumn("dbo.Products", "TypeName");
            DropTable("dbo.Types");
        }
    }
}
