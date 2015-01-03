namespace LicenseGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Licensehistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GeneratedLicenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramName = c.String(nullable: false, maxLength: 20),
                        Company = c.String(nullable: false, maxLength: 255),
                        NIP = c.String(nullable: false, maxLength: 10, fixedLength: true),
                        Privileges = c.String(),
                        LicenseTermDate = c.DateTime(),
                        AddionalInformation = c.String(),
                        NumberOfStands = c.Int(),
                        ProgramVersion = c.String(),
                        PartnerNIP = c.String(maxLength: 10, fixedLength: true),
                        IsEncrypted = c.Boolean(nullable: false),
                        GenerationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GeneratedLicenses");
        }
    }
}
