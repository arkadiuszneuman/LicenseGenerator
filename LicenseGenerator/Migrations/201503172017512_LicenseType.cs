namespace LicenseGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LicenseType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneratedLicenses", "LicenseType", c => c.Int());
            Sql("UPDATE dbo.GeneratedLicenses SET LicenseType = 1 WHERE AddionalInformation LIKE 'Licencja testowa wa¿na do%'");
            Sql("UPDATE dbo.GeneratedLicenses SET LicenseType = 2 WHERE AddionalInformation = 'Licencja bezterminowa'");
            Sql("UPDATE dbo.GeneratedLicenses SET LicenseType = 3 WHERE AddionalInformation LIKE 'Licencja z abonamentem wa¿nym do%'");
        }
        
        public override void Down()
        {
            DropColumn("dbo.GeneratedLicenses", "LicenseType");
        }
    }
}
