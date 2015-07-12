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

namespace Karenbic.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        private DataAccess.Context _context;

        public CustomerController(DataAccess.Context context)
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

        [HttpPost]
        public ActionResult Edit(DomainClasses.Customer customer, int cityId, int? customerGroupId)
        {
            DomainClasses.Customer item = new DomainClasses.Customer();

            item = _context.Customers
                .Include(x => x.Group)
                .Include(x => x.City)
                .Include(x => x.City.Province)
                .Single(x => x.Id == customer.Id);

            item.Name = customer.Name;
            item.Surname = customer.Surname;
            item.Gender = customer.Gender;
            item.Phone = customer.Phone;
            item.Mobile = customer.Mobile;
            item.Email = customer.Email;
            item.Address = customer.Address;

            customer.City = _context.Cities.Find(cityId);

            if (customerGroupId != null)
            {
                item.Group = _context.CustomerGroups.Find(customerGroupId);
            }
            else
            {
                item.Group = null;
            }

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
                Group = item.Group != null ? new
                {
                    Id = item.Group.Id,
                    Title = item.Group.Title
                } : new
                {
                    Id = 0,
                    Title = ""
                },
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
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Get(string customerName, int? customerGroupId,
            int? provinceId, int? cityId, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            IQueryable<DomainClasses.Customer> query = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(customerName))
            {
                query = query.Where(x => x.Name.Contains(customerName) || x.Surname.Contains(customerName));
            }

            if (customerGroupId != null)
            {
                query = query.Where(x => x.Group != null && x.Group.Id == customerGroupId);
            }

            if (provinceId != null)
            {
                query = query.Where(x => x.City.Province.Id == provinceId);
            }

            if (cityId != null)
            {
                query = query.Where(x => x.City.Id == cityId);
            }

            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(query.Count()) / Convert.ToDouble(pageSize)));
            int resultCount = query.Count();

            List<DomainClasses.Customer> list = query.OrderBy(x => x.Surname)
                .ThenBy(x => x.Name)
                .Include(x => x.Group)
                .Include(x => x.City)
                .Include(x => x.City.Province)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            result.Data = new
            {
                ResultCount = resultCount,
                PageCount = pageCount,
                PageIndex = pageIndex,
                List = list.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname,
                    Gender = x.Gender,
                    RegisterDate = Api.ConvertDate.JulainToPersian(x.RegisterDate),
                    Phone = x.Phone,
                    Mobile = x.Mobile,
                    Email = x.Email,
                    Address = x.Address,
                    Group = x.Group != null ? new
                    {
                        Id = x.Group.Id,
                        Title = x.Group.Title
                    } : new
                    {
                        Id = 0,
                        Title = ""
                    },
                    Province = new
                    {
                        Id = x.City.Province.Id,
                        Name = x.City.Province.Name
                    },
                    City = new
                    {
                        Id = x.City.Id,
                        Name = x.City.Name
                    }
                }).ToArray()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangePassword(int customerId, string newPassword)
        {
            bool result = false;

            DomainClasses.Customer customer = _context.Customers.Find(customerId);
            ApplicationUser user = UserManager.FindByName(customer.Username);

            UserManager.RemovePassword(user.Id);
            UserManager.AddPassword(user.Id, newPassword);
            result = true;

            return Content(result.ToString());
        }
    }
}