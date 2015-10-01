using Microsoft.AspNet.Identity;
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

namespace Karenbic.Areas.Customer.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Customer")]
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
        public ActionResult Edit()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetProfile()
        {
            DomainClasses.Customer customer = new DomainClasses.Customer();

            customer = _context.Customers
                .Include(x => x.City)
                .Include(x => x.City.Province)
                .Include(x => x.Group)
                .Single(x => x.Username == User.Identity.Name);

            return Json(new 
            {
                Id = customer.Id,
                Name = customer.Name,
                Surname = customer.Surname,
                Gender = customer.Gender,
                RegisterDate = Api.ConvertDate.JulainToPersian(customer.RegisterDate),
                Phone = customer.Phone,
                Mobile = customer.Mobile,
                Email = customer.Email,
                Address = customer.Address,
                Province = new
                {
                    Id = customer.City.Province.Id,
                    Name = customer.City.Province.Name
                },
                City = new
                {
                    Id = customer.City.Id,
                    Name = customer.City.Name
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(DomainClasses.Customer customer, int cityId)
        {
            DomainClasses.Customer item = new DomainClasses.Customer();

            item = _context.Customers
                .Include(x => x.Group)
                .Include(x => x.City)
                .Include(x => x.City.Province)
                .Single(x => x.Username == User.Identity.Name);

            item.Name = customer.Name;
            item.Surname = customer.Surname;
            item.Gender = customer.Gender;
            item.Phone = customer.Phone;
            item.Mobile = customer.Mobile;
            item.Email = customer.Email;
            item.Address = customer.Address;

            customer.City = _context.Cities.Find(cityId);

            _context.SaveChanges();

            return Json(new
            {
                Id = item.Id,
                Name = item.Name,
                Surname = item.Surname,
                Gender = item.Gender,
                RegisterDate = Api.ConvertDate.JulainToPersian(item.RegisterDate),
                Phone = item.Phone,
                Mobile = item.Mobile,
                Email = item.Email,
                Address = item.Address,
                Province = new
                {
                    Id = item.City.Province.Id,
                    Name = item.City.Province.Name
                },
                City = new
                {
                    Id = item.City.Id,
                    Name = item.City.Name
                }
            });
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