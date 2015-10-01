using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.SignalR;

namespace Karenbic.Areas.Admin.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Admin")]
    public class FinancialConflictController : Controller
    {
         private DataAccess.Context _context;

         public FinancialConflictController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(DomainClasses.FinancialConflict model, int customerId, DomainClasses.Portal portal)
        {
            model.Customer = _context.Customers.Find(customerId);
            model.Price = model.Items.Sum(x => x.Price);
            model.Portal = portal;
            model.RegisterDate = DateTime.Now;

            if (!ModelState.IsValid) throw new Exception();

            _context.FinancialConflicts.Add(model);
            _context.SaveChanges();

            //send notification
            var customerNotification = GlobalHost.ConnectionManager.GetHubContext<Hubs.CustomerNotification>();
            if (Hubs.CustomerNotification.Users.Any(x => x.Key == model.Customer.Username))
            {
                if (portal == DomainClasses.Portal.Print)
                {
                    customerNotification.Clients
                        .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == model.Customer.Username)
                        .Value.ConnectionIds.ToArray<string>()).newUnpayedPrintFinancialConflict();

                    customerNotification.Clients
                        .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == model.Customer.Username)
                        .Value.ConnectionIds.ToArray<string>()).newUnpayedPrintBilling();
                }
                else
                {
                    customerNotification.Clients
                        .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == model.Customer.Username)
                        .Value.ConnectionIds.ToArray<string>()).newUnpayedDesignFinancialConflict();

                    customerNotification.Clients
                        .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == model.Customer.Username)
                        .Value.ConnectionIds.ToArray<string>()).newUnpayedDesignBilling();
                }
            }

            return Json(new
            {
                Id = model.Id,
                Description = model.Description,
                Price = model.Price,
                RegisterDate = model.RegisterDate,
                PersianRegisterDate = model.PersianRegisterDate,
                Customer = new 
                {
                    Id = model.Customer.Id,
                    Name = model.Customer.Name,
                    Surname = model.Customer.Surname,
                    Username = model.Customer.Username,
                },
                Items = model.Items.Select(x => new 
                {
                    Description = model.Description,
                    Price = model.Price
                })
            });
        }

        [HttpGet]
        public ActionResult Find(int id)
        {
            DomainClasses.FinancialConflict model = _context.FinancialConflicts
                .Include(x => x.Customer)
                .Include(x => x.Items)
                .Single(x => x.Id == id);

            return Json(new
            {
                Id = model.Id,
                Description = model.Description,
                Price = model.Price,
                RegisterDate = model.RegisterDate,
                PersianRegisterDate = model.PersianRegisterDate,
                Customer = new
                {
                    Id = model.Customer.Id,
                    Name = model.Customer.Name,
                    Surname = model.Customer.Surname,
                    Username = model.Customer.Username,
                },
                Items = model.Items.Select(x => new
                {
                    Description = model.Description,
                    Price = model.Price
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Get(DomainClasses.Portal portal, string customerName, string startDate, string endDate, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            IQueryable<DomainClasses.FinancialConflict> query = _context.FinancialConflicts.AsQueryable();
            query = query.Where(x => x.Portal == portal);

            if (!string.IsNullOrEmpty(customerName))
            {
                query = query.Where(x => x.Customer.Name.Contains(customerName) ||
                    x.Customer.Surname.Contains(customerName));
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime julianStartDate = Api.ConvertDate.PersianTOJulian(startDate);
                query = query.Where(x => x.RegisterDate >= julianStartDate);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime tempJulianEndDate = Api.ConvertDate.PersianTOJulian(endDate);
                DateTime julianEndDate = new DateTime(tempJulianEndDate.Year, tempJulianEndDate.Month, tempJulianEndDate.Day, 23, 59, 59, 50);
                query = query.Where(x => x.RegisterDate <= julianEndDate);
            }

            int resultCount = await query.CountAsync();
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(resultCount) / Convert.ToDouble(pageSize)));
            

            List<DomainClasses.FinancialConflict> list = await query
                .Include(x => x.Customer)
                .OrderByDescending(x => x.RegisterDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            result.Data = new
            {
                ResultCount = resultCount,
                PageCount = pageCount,
                PageIndex = pageIndex,
                List = list.Select(x => new
                {
                    Id = x.Id,
                    Description = x.Description,
                    Price = x.Price,
                    RegisterDate = x.RegisterDate,
                    PersianRegisterDate = x.PersianRegisterDate,
                    Time = string.Format("{0:D2}:{1:D2}", x.RegisterDate.Hour, x.RegisterDate.Minute),
                    Customer = new
                    {
                        Id = x.Customer.Id,
                        Name = x.Customer.Name,
                        Surname = x.Customer.Surname,
                        Username = x.Customer.Username,
                    },
                    Items = x.Items.Select(c => new
                    {
                        Description = c.Description,
                        Price = c.Price
                    })
                }).ToArray()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            bool result = false;

            DomainClasses.FinancialConflict item = _context.FinancialConflicts.Find(id);
            if (item.IsPaid == false)
            {
                _context.FinancialConflicts.Remove(item);
                _context.SaveChanges();
                result = true;
            }

            return Content(result.ToString());
        }
    }
}