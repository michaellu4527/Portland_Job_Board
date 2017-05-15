using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobBoardMVC.CustomFilters
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If they are authorized, handle accordingly
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                // Otherwise redirect custom unauthorized page
                filterContext.Result = new RedirectResult("~/Roles/AuthorizeFailed");
            }
        }
    }
}