using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Karenbic.Models;
using Karenbic.UserInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Controllers
{
    public class AccountController : Controller
    {
        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
        }

        private ApplicationSignInManager SignInManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = string.IsNullOrEmpty(returnUrl) ? "/Admin/Home" : returnUrl;
            return View(new Models.LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.UserName,
                model.Password,
                model.RememberMe,
                shouldLockout: false);

            if (result == SignInStatus.Success)
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ViewBag.NotAuth = true;
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
	}
}