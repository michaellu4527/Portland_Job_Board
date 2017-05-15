using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobBoardMVC.Models
{
    public class JobLocationViewModel
    {
        // type supporting vars from the Jobs table - these are the jobs we will display
        public List<Job> jobs;

        // type supporting vars from the Places table that will populate the dropdown Location filter
        public List<Place> places;

        // type to return selection from dropdown Location filter back to the view
        [Display(Name = "Location:")]
        public int SelectedLocationId { get; set; }

       // type for the Options that populate the dropdown Location filter
        public IEnumerable<SelectListItem> Locations
        {
            get
            {
                var allLocations = places.Select(p => new SelectListItem
                {
                    Value = p.ID.ToString(),
                    Text = p.Location
                });
                return DefaultLocationItem.Concat(allLocations);
            }
        }

        // type for populating a default selection in the dropdown Location filter 
        public IEnumerable<SelectListItem> DefaultLocationItem
        {
            get { return Enumerable.Repeat(new SelectListItem
            {
                Value = "-1",
                Text = "--select location--"
            }, count: 1); }
        }
    }
}