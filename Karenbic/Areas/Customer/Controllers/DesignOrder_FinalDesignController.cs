using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Customer.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Customer")]
    public class DesignOrder_FinalDesignController : Controller
    {
        private DataAccess.Context _context;

        public DesignOrder_FinalDesignController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Get(int orderId)
        {
            DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

            DomainClasses.DesignOrder order = _context.DesignOrders.Find(orderId);
            bool mustSendNotification = order.CustomerMustSeeIt;
            order.CustomerMustSeeIt = false;
            _context.SaveChanges();


            //send notification
            if (mustSendNotification)
            {
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<Hubs.CustomerNotification>();

                notificationHub.Clients
                    .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == User.Identity.Name)
                    .Value.ConnectionIds.ToArray<string>()).minusUnreviewedDesign();
            }

            List<DomainClasses.DesignOrder_FinalDesign> list = _context.DesignOrder_FinalDesigns
                .Where(x => x.Order.Id == orderId && x.Order.Customer.Id == customer.Id).ToList();

            return Json(list.Select(x => new 
            { 
                Id = x.Id,
                Title = x.Title,
                Link = x.Link
            }), JsonRequestBehavior.AllowGet);
        }
    }
}