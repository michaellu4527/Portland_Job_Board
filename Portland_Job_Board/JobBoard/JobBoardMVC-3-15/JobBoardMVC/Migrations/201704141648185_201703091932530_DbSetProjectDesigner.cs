namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201703091932530_DbSetProjectDesigner : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.TestCompanies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TestCompanies",
                c => new
                    {
                        rowId = c.Int(nullable: false, identity: true),
                        testCompany = c.String(),
                    })
                .PrimaryKey(t => t.rowId);
            
        }
    }
}
