namespace LicenseGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProgramNameLongerText : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GeneratedLicenses", "ProgramName", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GeneratedLicenses", "ProgramName", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
