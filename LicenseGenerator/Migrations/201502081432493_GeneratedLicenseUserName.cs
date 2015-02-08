namespace LicenseGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GeneratedLicenseUserName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GeneratedLicenses", "UserName", c => c.String(nullable: true));
            Sql("UPDATE [dbo].[GeneratedLicenses] SET UserName = '' WHERE UserName IS NULL");
            AlterColumn("dbo.GeneratedLicenses", "UserName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GeneratedLicenses", "UserName");
        }
    }
}
