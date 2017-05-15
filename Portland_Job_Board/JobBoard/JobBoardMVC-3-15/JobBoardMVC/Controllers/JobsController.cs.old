using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobBoardMVC.Models;

namespace JobBoardMVC.Controllers
{
    public class JobsController : Controller
    {
        private readonly JobBoardMvcContext _context;

        public JobsController(JobBoardMvcContext context)
        {
            _context = context;
        }
        
        public JobsController()
        {
        }
        // GET: Jobs 
        // Include LINQ query to allow search
        public async Task<ActionResult> Index(string jobTitleString, string jobLocation, string companyString)
        {
            IQueryable<string> locationQuery = from j in _context.Jobs
                                               orderby j.Location
                                               select j.Location;

            var jobs = from j in _context.Jobs
                       select j;

            // Grab a count of the number of jobs in the database to display:
            int count = jobs.Count();
            ViewBag.Counts = count;

            if (!String.IsNullOrEmpty(jobTitleString))
            {
                jobs = jobs.Where(j => j.JobTitle.Contains(jobTitleString));
            }

            if (!String.IsNullOrEmpty(jobLocation))
            {
                jobs = jobs.Where(j => j.Location == jobLocation);
            }

            if (!String.IsNullOrEmpty(companyString))
            {
                jobs = jobs.Where(j => j.Company.Contains(companyString));
            }

            var jobLocationVM = new JobLocationViewModel();
            jobLocationVM.locations = new SelectList(await locationQuery.Distinct().ToListAsync());
            jobLocationVM.jobs = await jobs.ToListAsync();

            return View(jobLocationVM);
        }


        // GET: Jobs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                //return NotFiund();
            }

            var job = await _context.Jobs.SingleOrDefaultAsync(m => m.ID == id);
            if (job == null)
            {
               // return NotFound();
            }

            return View();
        }

       
    }
}
