namespace Shoppi.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultAddressinShoppiUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Addresses", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Addresses", new[] { "UserId" });
            AddColumn("dbo.AspNetUsers", "DefaultAddress_Id", c => c.Int());
            AlterColumn("dbo.Addresses", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Addresses", "UserId");
            CreateIndex("dbo.AspNetUsers", "DefaultAddress_Id");
            AddForeignKey("dbo.AspNetUsers", "DefaultAddress_Id", "dbo.Addresses", "Id");
            AddForeignKey("dbo.Addresses", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.AspNetUsers", "DefaultAddressId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "DefaultAddressId", c => c.Int());
            DropForeignKey("dbo.Addresses", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DefaultAddress_Id", "dbo.Addresses");
            DropIndex("dbo.AspNetUsers", new[] { "DefaultAddress_Id" });
            DropIndex("dbo.Addresses", new[] { "UserId" });
            AlterColumn("dbo.Addresses", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.AspNetUsers", "DefaultAddress_Id");
            CreateIndex("dbo.Addresses", "UserId");
            AddForeignKey("dbo.Addresses", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
