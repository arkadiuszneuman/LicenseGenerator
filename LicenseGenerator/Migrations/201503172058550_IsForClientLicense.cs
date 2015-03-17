namespace LicenseGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsForClientLicense : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneratedLicenses", "IsForClient", c => c.Boolean(nullable: true));
            Sql("UPDATE dbo.GeneratedLicenses SET IsForClient=0");
            AlterColumn("dbo.GeneratedLicenses", "IsForClient", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GeneratedLicenses", "IsForClient");
        }
    }
}
