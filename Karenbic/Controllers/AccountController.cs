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
        private DataAccess.Context _context;

        public AccountController(DataAccess.Context context)
        {
            _context = context;
        }

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

        [HttpGet]
        public ActionResult Register()
        {
            return View(new DomainClasses.Customer());
        }

        [HttpPost]
        public ActionResult Register(DomainClasses.Customer model, int cityId, string password, string rePassword)
        {
            bool isValid = true;
            List<string> errors = new List<string>();

            if (!ModelState.IsValid)
            {
                isValid = false;
                errors.Add("اطلاعات ورودی صحیح نمی باشد");
            }
            if (_context.Customers.Any(x => x.Email == model.Email))
            {
                isValid = false;
                errors.Add("ایمیل تکراری می باشد");
            }
            if (_context.Customers.Any(x => x.Username == model.Username))
            {
                isValid = false;
                errors.Add("نام کاربری تکراری می باشد");
            }
            if (password != rePassword || password.Length < 6)
            {
                isValid = false;
                errors.Add("پسورد مجاز نمی باشد");
            }

            if (!isValid)
            {
                ViewBag.Errors = errors;
                return View(model);
            }

            model.City = _context.Cities.Find(cityId);
            _context.Customers.Add(model);
            _context.SaveChanges();

            UserManager.Create(new ApplicationUser() { 
                UserName = model.Username,
                Email = model.Email
            }, password);

            ViewBag.RegisterSuccessed = true;

            return View(new DomainClasses.Customer());
        }

        [HttpPost]
        public ActionResult Register_CheckEmail(string email)
        {
            if (_context.Customers.Any(x => x.Email == email))
            {
                return Content("false");
            }

            return Content("true");
        }

        [HttpPost]
        public ActionResult Register_CheckUsername(string username)
        {
            if (_context.Customers.Any(x => x.Username == username))
            {
                return Content("false");
            }

            return Content("true");
        }
	}
}