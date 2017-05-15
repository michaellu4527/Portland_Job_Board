namespace JobBoardMVC.Migrations
{
    using Models;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using JobBoardMVC.Models;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<JobBoardMVC.Models.JobBoardMvcContext>
    {

        public Configuration()

        {
            //AutomaticMigrationDataLossAllowed = true;
            //AutomaticMigrationsEnabled = true;
            ContextKey = "JobBoardMVC.Models.JobBoardMvcContext";

            //   JobBoardMvcContext context = new JobBoardMvcContext();

            // Seed(context);

        }



        protected override void Seed(JobBoardMvcContext context)
        {
            // Your seed code here...
            if (context.Jobs.Any())
            {
                return;
            }
            else
            {
                var jobsJsonAll = GetEmbeddedResourceAsString("JobBoardMVC.App_Data.Amazon.json");


                JArray jsonValJobs = JArray.Parse(jobsJsonAll) as JArray;
                dynamic jobsData = jsonValJobs;
                foreach (dynamic job in jobsData)
                {

                    context.Jobs.Add(new Job
                    {
                        ApplicationLink = job.ApplicationLink,
                        Company = job.Company,
                        DatePosted = job.DatePosted,
                        Experience = job.Experience,
                        Hours = job.Hours,
                        JobID = job.JobId,
                        JobTitle = job.JobTitle,
                        LanguagesUsed = job.LanguagesUsed,
                        Location = job.Location,
                        Salary = job.Salary

                    });
                }
                // Make sure to have the context save changes so next section can query Job table
                context.SaveChanges();

                // populate the Place DBSet with distinct (enumerable) Location values 
                // this code should migrate for a regular db load from scrapes
                IQueryable<string> query = from j in context.Jobs
                                           orderby j.Location
                                           select j.Location;
                dynamic placeData = query.Distinct().ToList();
                foreach (dynamic place in placeData)
                {
                    // what was the last row in the Place table - throws an exception if more than one rowID item listed
                    var rowNumber = getLastRowID(context).SingleOrDefault();
                    // stage a new row for place table
                    var Loc = new Place { Location = place, ID = rowNumber + 1 };
                    // write the row unless we already have that Location 
                    context.Places.AddOrUpdate(p => p.Location, Loc);
                    // Make sure to have the context save changes so the rowNumber will update next iteration
                    context.SaveChanges();
                }
                // call base in case this overide seed method doesn't apply
                base.Seed(context);
            }   // end of else
        }   // end of Seed method

        private static string GetEmbeddedResourceAsString(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            //var names = assembly.GetManifestResourceNames();

            string result;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))

            using (StreamReader reader = new StreamReader(stream))

                result = reader.ReadToEnd();

            return result;


        }
        private IEnumerable<int> getLastRowID(JobBoardMvcContext context)
        {

            IQueryable<int> rowNum = from p in context.Places.Take(1)
                                     orderby p.ID descending
                                     select p.ID;
            return rowNum;

        }
    }
}
