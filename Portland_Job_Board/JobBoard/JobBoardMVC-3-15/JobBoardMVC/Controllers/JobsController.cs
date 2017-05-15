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
using System.Data.Entity.Migrations;
using JobBoardMVC.CustomFilters;

namespace JobBoardMVC.Controllers
{
    public class JobsController : Controller
    {
        private JobBoardMvcContext context = new JobBoardMvcContext();

        // GET: Jobs
  
        public async Task<ActionResult> Index(string jobTitleString, string companyString, int selectedLocationId = 0)
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

        // GET: Jobs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await context.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Jobs/Create
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ApplicationLink,Company,DatePosted,Experience,Hours,JobID,JobTitle,LanguagesUsed,Location,Salary")] Job job)
        {
            if (ModelState.IsValid)
            {
                context.Jobs.Add(job);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(job);
        }

        //GET: Jobs/Edit/5
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await context.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ApplicationLink,Company,DatePosted,Experience,Hours,JobID,JobTitle,LanguagesUsed,Location,Salary")] Job job)
        {
            if (ModelState.IsValid)
            {
                context.Entry(job).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(job);
        }

        //GET: Jobs/Delete/5
        [CustomAuthorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await context.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        //public ActionResult Delete()
        //{
        //    return View();
        //}

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Job job = await context.Jobs.FindAsync(id);
            context.Jobs.Remove(job);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
