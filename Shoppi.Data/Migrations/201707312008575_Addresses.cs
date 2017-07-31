namespace Shoppi.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addresses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AddressLine = c.String(),
                        City = c.String(),
                        ZipCode = c.String(),
                        Country = c.String(),
                        ShoppiUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ShoppiUser_Id)
                .Index(t => t.ShoppiUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Addresses", "ShoppiUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Addresses", new[] { "ShoppiUser_Id" });
            DropTable("dbo.Addresses");
        }
    }
}
