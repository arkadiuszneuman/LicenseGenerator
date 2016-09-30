namespace LicenseGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultPrivileges : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.vw_lg_Product", "tw_Pole3", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.vw_lg_Product", "tw_Pole3");
        }
    }
}
