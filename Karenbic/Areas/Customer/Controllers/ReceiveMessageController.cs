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
    public class ReceiveMessageController : Controller
    {
        private DataAccess.Context _context;

        public ReceiveMessageController(DataAccess.Context context)
        {
            _context = context;
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

            IQueryable<DomainClasses.AdminMessage_Customer> query = _context.AdminMessages_Customer.AsQueryable();

            DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);
            query = query.Where(x => x.Customer.Id == customer.Id);

            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(query.Count()) / Convert.ToDouble(pageSize)));
            int resultCount = query.Count();

            List<DomainClasses.AdminMessage_Customer> list = query
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
                    IsRead = x.IsRead
                }).ToArray()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MarkAsRead(Guid id)
        {
            bool result = false;

            DomainClasses.AdminMessage_Customer message = _context.AdminMessages_Customer.Find(id);
            message.IsRead = true;
            _context.SaveChanges();
            result = true;

            //send notification
            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<Hubs.CustomerNotification>();

            notificationHub.Clients
                .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == User.Identity.Name)
                .Value.ConnectionIds.ToArray<string>()).minusUnReadMessage();

            notificationHub.Clients
                .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == User.Identity.Name)
                .Value.ConnectionIds.ToArray<string>()).minusUnReadAdminMessage();

            return Content(result.ToString());
        }

        [HttpPost]
        public ActionResult Remove(Guid id)
        {
            bool result = false;

            DomainClasses.AdminMessage_Customer message = _context.AdminMessages_Customer.Find(id);
            _context.AdminMessages_Customer.Remove(message);
            _context.SaveChanges();
            result = true;

            return Content(result.ToString());
        }

        [HttpGet]
        public ActionResult GetAllUnReadMessage()
        {
            JsonResult result = new JsonResult();

            IQueryable<DomainClasses.AdminMessage_Customer> query = _context.AdminMessages_Customer.AsQueryable();
            query = query.Where(x => x.IsRead == false);

            DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);
            query = query.Where(x => x.Customer.Id == customer.Id);

            List<DomainClasses.AdminMessage_Customer> list = query
                .OrderByDescending(x => x.SendDate)
                .ToList();

            result.Data = list.Select(x => new
            {
                Id = x.Id,
                SendDate = Api.ConvertDate.JulainToPersian(x.SendDate),
                Time = string.Format("{0:D2}:{1:D2}", x.SendDate.Hour, x.SendDate.Minute),
                Title = x.Title,
                Text = x.Text,
                IsRead = x.IsRead
            }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}