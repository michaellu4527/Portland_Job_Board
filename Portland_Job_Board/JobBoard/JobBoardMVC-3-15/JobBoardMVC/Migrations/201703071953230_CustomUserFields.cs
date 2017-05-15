namespace JobBoardMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomUserFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Location", c => c.String());
            AddColumn("dbo.AspNetUsers", "Languages", c => c.String());
            AddColumn("dbo.AspNetUsers", "Experience", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Experience");
            DropColumn("dbo.AspNetUsers", "Languages");
            DropColumn("dbo.AspNetUsers", "Location");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
