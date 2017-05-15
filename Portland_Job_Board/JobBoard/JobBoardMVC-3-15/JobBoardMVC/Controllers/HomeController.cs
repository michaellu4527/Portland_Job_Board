using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobBoardMVC.Models;
using System.Threading.Tasks;
using System.Data.Entity;
using JobBoardMVC.CustomFilters;

namespace JobBoardMVC.Controllers
{
    public class HomeController : Controller
    {
        private JobBoardMvcContext context = new JobBoardMvcContext();

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page";

            return View();
        }












        // GET: Admin
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Admin(string jobTitleString, string companyString, int selectedLocationId = 0)
        {
            // Include LINQ queries to allow filters
            var jobs = from j in context.Jobs
                       select j;
            var places = from p in context.Places
                         select p;


            // Job title search form field
            if (!String.IsNullOrEmpty(jobTitleString))
            {
                jobs = jobs.Where(j => j.JobTitle.Contains(jobTitleString));
            }

            // Location search dropdown form field
            if (selectedLocationId > 0)
            {
                // use the selectedLocationID we received from the View dropdown form field to parse desired location to display
                var selectedLocation = places.Where(sl => sl.ID == selectedLocationId);
                // select from the intersection with the jobs variable
                jobs = from j in jobs
                       join sl in selectedLocation on j.Location equals sl.Location
                       select j;
            }

            // company search form field
            if (!String.IsNullOrEmpty(companyString))
            {
                jobs = jobs.Where(j => j.Company.Contains(companyString));
            }

            // grab a count of the number of jobs currently inside the jobs variable.
            int count = jobs.Count();
            // store it in viewbag for the View to display
            ViewBag.Counts = count;

            //instantiate the view model and serve it to the view

            var jobLocationVM = new JobLocationViewModel();
            jobLocationVM.jobs = await jobs.ToListAsync();
            jobLocationVM.places = await places.ToListAsync();

            return View(jobLocationVM);

        }
    }
}