namespace LicenseGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedProduct : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.vw_lg_Product", "tw_SklepInternet", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.vw_lg_Product", "tw_SklepInternet");
        }
    }
}
