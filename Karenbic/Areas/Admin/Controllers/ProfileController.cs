﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Karenbic.UserInfrastructure;

namespace Karenbic.Areas.Admin.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Admin")]
    public class ProfileController : Controller
    {
        private DataAccess.Context _context;

        public ProfileController(DataAccess.Context context)
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

        [HttpGet]
        public ActionResult ChangeMobileNumber()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetMobileNumber()
        {
            DomainClasses.Setting setting = _context.Setting.Find(1);

            return Json(setting.AdminMobile, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangeMobileNumber(string number)
        {
            DomainClasses.Setting setting = _context.Setting.Find(1);
            setting.AdminMobile = number;
            _context.SaveChanges();

            return Json(number, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string newPassword, string reNewPassword)
        {
            bool result = false;
            if (newPassword != reNewPassword) return Content(result.ToString());

            ApplicationUser user = UserManager.FindByName(User.Identity.Name);

            UserManager.RemovePassword(user.Id);
            UserManager.AddPassword(user.Id, newPassword);
            result = true;

            return Content(result.ToString());
        }
    }
}