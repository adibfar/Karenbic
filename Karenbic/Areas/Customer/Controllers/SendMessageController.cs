using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.SignalR;

namespace Karenbic.Areas.Customer.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Customer")]
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
        public ActionResult New(DomainClasses.CustomerMessage message)
        {
            message.Sender = _context.Customers.Single(x => x.Username == User.Identity.Name);

            _context.CustomerMessages.Add(message);
            _context.SaveChanges();

            //Send Notification To The Admin
            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<Hubs.AdminNotification>();
            notificationHub.Clients.All.newUnReadCustomerMessage();

            return Json(new 
            { 
                Id = message.Id,
                Title = message.Title,
                Text = message.Text
            });
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

            IQueryable<DomainClasses.CustomerMessage> query = _context.CustomerMessages.AsQueryable();

            DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

            query = query.Where(x => x.Sender.Id == customer.Id && x.IsShowCustomer);

            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(query.Count()) / Convert.ToDouble(pageSize)));
            int resultCount = query.Count();

            List<DomainClasses.CustomerMessage> list = query
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
                    IsRead = x.IsReadCustomer,
                    IsAdminReply = x.IsAdminReply,
                    AdminReply = x.AdminReply
                }).ToArray()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MarkAsRead(Guid id)
        {
            bool result = false;

            DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

            DomainClasses.CustomerMessage message = _context.CustomerMessages
                    .Include(x => x.Sender)
                    .Single(x => x.Id == id && x.Sender.Id == customer.Id);

            message.IsReadCustomer = true;
            _context.SaveChanges();
            result = true;

            //send notification
            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<Hubs.CustomerNotification>();

            notificationHub.Clients
                .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == User.Identity.Name)
                .Value.ConnectionIds.ToArray<string>()).minusUnReadMessage();

            notificationHub.Clients
                .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == User.Identity.Name)
                .Value.ConnectionIds.ToArray<string>()).minusUnReadReplyMessage();

            return Content(result.ToString());
        }

        [HttpPost]
        public ActionResult Remove(Guid id)
        {
            bool result = false;

            DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

            DomainClasses.CustomerMessage message = _context.CustomerMessages
                    .Include(x => x.Sender)
                    .Single(x => x.Id == id && x.Sender.Id == customer.Id);

            if (message.IsShowAdmin)
            {
                message.IsShowCustomer = false;
            }
            else
            {
                _context.CustomerMessages.Remove(message);
            }
            _context.SaveChanges();
            result = true;

            return Content(result.ToString());
        }
    }
}