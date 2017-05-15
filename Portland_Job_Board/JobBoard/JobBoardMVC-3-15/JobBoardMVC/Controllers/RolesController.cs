using JobBoardMVC.CustomFilters;
using JobBoardMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JobBoardMVC.Controllers
{
    public class RolesController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private UserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public UserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Roles
        JobBoardMvcContext context;

        public RolesController()
        {
            context = new JobBoardMvcContext();
        }

        // Get All Roles
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var Roles = context.Roles.ToList();
            return View(Roles);
        }

        // GET: Roles/Create
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        // POST: /Roles/Create
        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Create(IdentityRole Role)
        {
            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("Index", "Roles");
        }

        // GET: /Roles/Remove
        [HttpGet]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Remove()
        {
            return View();
        }

        // POST: Roles/Remove
        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Remove(string RoleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index", "Roles");
        }

        // GET: /Roles/Register
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Register()
        {
            ViewBag.Name = new SelectList(context.Roles.ToList(), "Name", "Name");
            return View();
        }

        // POST: /Roles/Register
        [HttpPost]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { Email = model.Email, UserName = model.Email, Location = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(user.Id, model.Name);

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Jobs");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // GET: Roles/AuthorizeFailed
        public ActionResult AuthorizeFailed()
        {
            return View();
        }
    }
}