
using JobBoardMVC.Migrations;
using JobBoardMVC.Models;
using Microsoft.Owin;
using Owin;
using System.Data.Entity;

[assembly: OwinStartupAttribute(typeof(JobBoardMVC.Startup))]
namespace JobBoardMVC
{
    public partial class Startup
    {
        //public object SeedData { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            //JobBoardMvcContext context = new JobBoardMvcContext();

            
        //   Database.SetInitializer(new DropCreateDatabaseAlways<JobBoardMvcContext>());

          Database.SetInitializer(new MigrateDatabaseToLatestVersion<JobBoardMvcContext, Configuration>());

        }

    }
}
