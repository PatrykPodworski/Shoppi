namespace Shoppi.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultAddressIdonceagain : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AspNetUsers", name: "DefaultAddress_Id", newName: "DefaultAddressId");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_DefaultAddress_Id", newName: "IX_DefaultAddressId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_DefaultAddressId", newName: "IX_DefaultAddress_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "DefaultAddressId", newName: "DefaultAddress_Id");
        }
    }
}
