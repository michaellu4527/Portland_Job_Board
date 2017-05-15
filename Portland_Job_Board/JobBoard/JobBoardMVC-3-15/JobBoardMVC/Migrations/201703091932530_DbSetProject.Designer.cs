using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Data.Entity.Migrations;

namespace JobBoardMVC.Migrations
{
    public partial class DbSetProject : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Location = c.String(),
                    PhoneNumber = c.String(),
                    Website = c.String(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Jobs",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    ApplicationLink = c.String(),
                    Company = c.String(),
                    DatePosted = c.String(),
                    Experience = c.String(),
                    Hours = c.String(),
                    JobID = c.String(),
                    JobTitle = c.String(),
                    LanguagesUsed = c.String(),
                    Location = c.String(),
                    Salary = c.String(),
                })
                .PrimaryKey(t => t.ID);

            CreateTable(
                "dbo.Places",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    Location = c.String(),
                })
                .PrimaryKey(t => t.ID);

        }

        public override void Down()
        {
            DropTable("dbo.Places");
            DropTable("dbo.Jobs");
            DropTable("dbo.Companies");
        }
    }
}
