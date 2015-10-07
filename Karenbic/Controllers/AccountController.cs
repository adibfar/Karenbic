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
using System.Net.Mail;

namespace Karenbic.Controllers
{
    public class AccountController : Controller
    {
        private DataAccess.Context _context;
        private SMSService.ISMSService _SMSService;

        public AccountController(DataAccess.Context context, SMSService.ISMSService SMSService)
        {
            _context = context;
            _SMSService = SMSService;
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
            //RoleManager.Create(new ApplicationRole("Admin"));
            //RoleManager.Create(new ApplicationRole("Customer"));
            //UserManager.Create(new ApplicationUser() 
            //{ 
            //    UserName = "Admin",
            //    Email = "ali.andalibi@outllok.com"
            //}, "123456");
            //UserManager.AddToRole(UserManager.FindByName("Admin").Id, "Admin");

            ViewBag.ReturnUrl = string.IsNullOrEmpty(returnUrl) ? "" : returnUrl;
            return View(new Models.LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl, string captcha)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            if (Session["Captcha_Login"] == null || Session["Captcha_Login"].ToString() != captcha)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.UserName,
                model.Password,
                model.RememberMe,
                shouldLockout: false);

            if (result == SignInStatus.Success)
            {
                ApplicationUser user = UserManager.FindByName(model.UserName);

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else if (UserManager.IsInRole(user.Id, "Admin"))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else if (UserManager.IsInRole(user.Id, "Customer"))
                {
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
                else
                {
                    ViewBag.NotAuth = true;
                    return View(model);
                }
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
        public ActionResult RecoveryPassword()
        {
            return View();
        }


        [HttpPost]
        public virtual ViewResult RecoveryPassword(string username, string captcha)
        {
            if (Session["Captcha_RecoveryPassword"] == null || Session["Captcha_RecoveryPassword"].ToString() != captcha)
            {
                ViewBag.RecoveryPasswordError += "مجموع اشتباه است";
                return View();
            }

            if (_context.Customers.Any(x => x.Username == username))
            {
                DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == username);

                //Recovery Password
                ApplicationUser user = UserManager.FindByName(username);
                string newPassword = new Random().Next(100000, 999999).ToString();

                UserManager.RemovePassword(user.Id);
                UserManager.AddPassword(user.Id, newPassword);

                //Send Email
                try
                {
                    System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                    msg.BodyEncoding = System.Text.Encoding.UTF8;
                    msg.Subject = "بازیابی رمز عبور";
                    msg.Body = string.Format(@"استودیو کارن
{0} عزیز
رمز عبور جدید شما{1} می باشد
                        ", customer.Name, newPassword);
                    msg.From = new System.Net.Mail.MailAddress("support@karenads.com");
                    msg.To.Add(new MailAddress(customer.Email));

                    SmtpClient smtp = new SmtpClient("mail.karenads.com", 25);
                    smtp.Credentials = new System.Net.NetworkCredential("support@karenads.com", "Karen@2340374$");
                    smtp.EnableSsl = false;
                    smtp.Send(msg);
                }
                catch { }

                //Send SMS
                try
                {
                    _SMSService.Send(new string[] { customer.Mobile },
                        string.Format(@"استودیو کارن
{0} عزیز
رمز عبور جدید شما{1} می باشد
                        ", customer.Name, newPassword), false);
                }
                catch { }

                ViewBag.RecoveryPasswordSuccess = true;
            }
            else
            {
                ViewBag.RecoveryPasswordError = "کاربری شما یافت نشد.";
            }

            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new DomainClasses.Customer());
        }

        [HttpPost]
        public async Task<ActionResult> Register(DomainClasses.Customer model, int cityId, 
            string password, string rePassword, string captcha, string captchaPerfix)
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
            if (Session["Captcha_" + captchaPerfix] == null || Session["Captcha_" + captchaPerfix].ToString() != captcha)
            {
                isValid = false;
                errors.Add("مجموع اشتباه است");
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
            UserManager.AddToRole(UserManager.FindByName(model.Username).Id, "Customer");

            var result = await SignInManager.PasswordSignInAsync(model.Username,
                password,
                true,
                shouldLockout: false);

            if (result == SignInStatus.Success)
            {
                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            else
            {
                ViewBag.RegisterSuccessed = true;

                return View(new DomainClasses.Customer());
            }
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