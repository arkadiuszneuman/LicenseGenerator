namespace LicenseGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Products : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.vw_lg_Product",
            //    c => new
            //        {
            //            tw_Id = c.Int(nullable: false, identity: true),
            //            tw_Symbol = c.String(),
            //            tw_Nazwa = c.String(),
            //            tw_Pole1 = c.String(),
            //        })
            //    .PrimaryKey(t => t.tw_Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.vw_lg_Product");
        }
    }
}
