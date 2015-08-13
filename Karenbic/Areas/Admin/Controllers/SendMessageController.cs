using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Karenbic.Areas.Admin.Controllers
{
    public class SendMessageController : Controller
    {
        private DataAccess.Context _context;

        public SendMessageController(DataAccess.Context context)
        {
            _context = context;
        }

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
                    mainMessage.CustomerGroups.Add(await _context.CustomerGroups.FindAsync(id));
                }
            }
            if (customersId != null && customersId.Length > 0)
            {
                mainMessage.Customers = new List<DomainClasses.Customer>();
                foreach (int id in customersId)
                {
                    mainMessage.Customers.Add(await _context.Customers.FindAsync(id));
                }
            }


            //Create Customer Message
            if (isCustomerFilter && customersId != null && customersId.Length > 0)
            {
                foreach (int id in customersId)
                {
                    DomainClasses.Customer customer = await _context.Customers.FindAsync(id);

                    DomainClasses.AdminMessage_Customer message = new DomainClasses.AdminMessage_Customer();
                    message.Title = title;
                    message.Text = text;
                    message.SendDate = mainMessage.SendDate;
                    message.Customer = customer;
                    message.AdminMessage_Admin = mainMessage;

                    _context.AdminMessages_Customer.Add(message);
                }
            }
            else if (isCustomerGroupFilter && customerGroupsId != null && customerGroupsId.Length > 0)
            {
                foreach (int groupId in customerGroupsId)
                {
                    DomainClasses.CustomerGroup group = await _context.CustomerGroups
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

                        _context.AdminMessages_Customer.Add(message);
                    }
                }
            }
            else if (isCustomerGroupFilter == false && isCustomerFilter == false)
            {
                List<DomainClasses.Customer> customers = await _context.Customers.ToListAsync();
                foreach (DomainClasses.Customer customer in customers)
                {
                    DomainClasses.AdminMessage_Customer message = new DomainClasses.AdminMessage_Customer();
                    message.Title = title;
                    message.Text = text;
                    message.SendDate = mainMessage.SendDate;
                    message.Customer = customer;
                    message.AdminMessage_Admin = mainMessage;

                    _context.AdminMessages_Customer.Add(message);
                }
            }

            _context.SaveChanges();

            //Send Notification To The Customer
            //new HubConnection
            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<Hubs.CustomerNotification>();

            if (isCustomerFilter && customersId != null && customersId.Length > 0)
            {
                foreach (int id in customersId)
                {
                    DomainClasses.Customer customer = await _context.Customers.FindAsync(id);

                    if (Hubs.CustomerNotification.Users.Any(x => x.Key == customer.Username))
                    {
                        notificationHub.Clients
                            .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == customer.Username)
                            .Value.ConnectionIds.ToArray<string>()).newUnReadMessage();

                        notificationHub.Clients
                            .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == customer.Username)
                            .Value.ConnectionIds.ToArray<string>()).newUnReadAdminMessage();
                    }
                }
            }
            else if (isCustomerGroupFilter && customerGroupsId != null && customerGroupsId.Length > 0)
            {
                foreach (int groupId in customerGroupsId)
                {
                    DomainClasses.CustomerGroup group = await _context.CustomerGroups
                        .Include(x => x.Customers)
                        .SingleAsync(x => x.Id == groupId);

                    foreach (DomainClasses.Customer customer in group.Customers)
                    {
                        if (Hubs.CustomerNotification.Users.Any(x => x.Key == customer.Username))
                        {
                            notificationHub.Clients
                            .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == customer.Username)
                            .Value.ConnectionIds.ToArray<string>()).newUnReadMessage();

                            notificationHub.Clients
                            .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == customer.Username)
                            .Value.ConnectionIds.ToArray<string>()).newUnReadAdminMessage();
                        }
                    }
                }
            }
            else if (isCustomerGroupFilter == false && isCustomerFilter == false)
            {
                List<DomainClasses.Customer> customers = await _context.Customers.ToListAsync();
                foreach (DomainClasses.Customer customer in customers)
                {
                    if (Hubs.CustomerNotification.Users.Any(x => x.Key == customer.Username))
                    {
                        notificationHub.Clients
                            .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == customer.Username)
                            .Value.ConnectionIds.ToArray<string>()).newUnReadMessage();

                        notificationHub.Clients
                            .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == customer.Username)
                            .Value.ConnectionIds.ToArray<string>()).newUnReadAdminMessage();
                    }
                }
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
                IQueryable<DomainClasses.Customer> query = _context.Customers.AsQueryable();
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

            IQueryable<DomainClasses.AdminMessage_Admin> query = _context.AdminMessages_Admin.AsQueryable();
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

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Remove(Guid id)
        {
            bool result = false;

            DomainClasses.AdminMessage_Admin message = _context.AdminMessages_Admin.Find(id);
            message.IsShowAdmin = false;
            _context.SaveChanges();
            result = true;

            return Content(result.ToString());
        }
    }
}