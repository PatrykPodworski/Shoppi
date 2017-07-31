namespace Shoppi.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdinAddress : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Addresses", name: "ShoppiUser_Id", newName: "UserId");
            RenameIndex(table: "dbo.Addresses", name: "IX_ShoppiUser_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Addresses", name: "IX_UserId", newName: "IX_ShoppiUser_Id");
            RenameColumn(table: "dbo.Addresses", name: "UserId", newName: "ShoppiUser_Id");
        }
    }
}
