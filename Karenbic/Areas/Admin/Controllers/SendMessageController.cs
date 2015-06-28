using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Karenbic.Areas.Admin.Controllers
{
    public class SendMessageController : Controller
    {
        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> New(string title, string text, bool isCustomerGroupFilter, int[] customerGroupsId, 
            bool isCustomerFilter, int[] customersId)
        {
            using (DataAccess.Context context = new DataAccess.Context())
            {
                //Create Main Message
                DomainClasses.AdminMessage_Admin mainMessage = new DomainClasses.AdminMessage_Admin();
                mainMessage.Title = title;
                mainMessage.Text = text;
                mainMessage.IsCustomerGroupFilter = isCustomerGroupFilter;
                mainMessage.IsCustomerFilter = isCustomerFilter;
                if (customerGroupsId != null && customerGroupsId.Length > 0)
                {
                    mainMessage.CustomerGroups = new List<DomainClasses.CustomerGroup>();
                    foreach (int id in customerGroupsId)
                    {
                        mainMessage.CustomerGroups.Add(await context.CustomerGroups.FindAsync(id));
                    }
                }
                if (customersId != null && customersId.Length > 0)
                {
                    mainMessage.Customers = new List<DomainClasses.Customer>();
                    foreach (int id in customersId)
                    {
                        mainMessage.Customers.Add(await context.Customers.FindAsync(customersId));
                    }
                }


                //Create Customer Message
                if (isCustomerFilter && customersId != null && customersId.Length > 0)
                {
                    foreach (int id in customersId)
                    {
                        DomainClasses.Customer customer = await context.Customers.FindAsync(id);

                        DomainClasses.AdminMessage_Customer message = new DomainClasses.AdminMessage_Customer();
                        message.Title = title;
                        message.Text = text;
                        message.SendDate = mainMessage.SendDate;
                        message.Customer = customer;
                        message.AdminMessage_Admin = mainMessage;

                        context.AdminMessages_Customer.Add(message);
                    }
                }
                else if (isCustomerGroupFilter && customerGroupsId != null && customerGroupsId.Length > 0)
                {
                    foreach (int groupId in customerGroupsId)
                    {
                        DomainClasses.CustomerGroup group = await context.CustomerGroups
                            .Include(x => x.Customers)
                            .SingleAsync(x => x.Id == groupId);

                        foreach (DomainClasses.Customer customer in group.Customers)
                        {
                            DomainClasses.AdminMessage_Customer message = new DomainClasses.AdminMessage_Customer();
                            message.Title = title;
                            message.Text = text;
                            message.SendDate = mainMessage.SendDate;
                            message.Customer = customer;
                            message.AdminMessage_Admin = mainMessage;

                            context.AdminMessages_Customer.Add(message);
                        }
                    }
                }
                else if(isCustomerGroupFilter == false && isCustomerFilter == false)
                {
                    List<DomainClasses.Customer> customers = await context.Customers.ToListAsync();
                    foreach (DomainClasses.Customer customer in customers)
                    {
                        DomainClasses.AdminMessage_Customer message = new DomainClasses.AdminMessage_Customer();
                        message.Title = title;
                        message.Text = text;
                        message.SendDate = mainMessage.SendDate;
                        message.Customer = customer;
                        message.AdminMessage_Admin = mainMessage;

                        context.AdminMessages_Customer.Add(message);
                    }
                }

                context.SaveChanges();
            }

            return Json(new
            {
                //Id = message.Id,
                //Title = message.Title,
                //Text = message.Text
            });
        }

        [HttpGet]
        public ActionResult New_GetCustomers(int[] customerGroupId)
        {
            JsonResult result = new JsonResult();

            if (customerGroupId != null && customerGroupId.Length > 0)
            {
                using (DataAccess.Context context = new DataAccess.Context())
                {
                    IQueryable<DomainClasses.Customer> query = context.Customers.AsQueryable();
                    query = query.Where(x => x.Group != null && customerGroupId.Contains(x.Group.Id));

                    List<DomainClasses.Customer> list = query.OrderBy(x => x.Surname)
                        .ThenBy(x => x.Name)
                        .Include(x => x.Group)
                        .Include(x => x.City)
                        .Include(x => x.City.Province).ToList();

                    result.Data = list.Select(x => new
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Surname = x.Surname
                    }).ToArray();
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Get(int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                IQueryable<DomainClasses.AdminMessage_Admin> query = context.AdminMessages_Admin.AsQueryable();
                query = query.Where(x => x.IsShowAdmin);

                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(query.Count()) / Convert.ToDouble(pageSize)));
                int resultCount = query.Count();

                List<DomainClasses.AdminMessage_Admin> list = query
                    .OrderByDescending(x => x.SendDate)
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
                        SendDate = Api.ConvertDate.JulainToPersian(x.SendDate),
                        Time = string.Format("{0:D2}:{1:D2}", x.SendDate.Hour, x.SendDate.Minute),
                        Title = x.Title,
                        Text = x.Text,
                    }).ToArray()
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Remove(Guid id)
        {
            bool result = false;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.AdminMessage_Admin message = context.AdminMessages_Admin.Find(id);
                message.IsShowAdmin = false;
                context.SaveChanges();
                result = true;
            }

            return Content(result.ToString());
        }
    }
}