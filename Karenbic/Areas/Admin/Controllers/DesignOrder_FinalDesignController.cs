using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Admin.Controllers
{
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
            List<DomainClasses.DesignOrder_FinalDesign> list = _context.DesignOrder_FinalDesigns
                .Where(x => x.Order.Id == orderId).ToList();

            return Json(list.Select(x => new 
            { 
                Id = x.Id,
                Title = x.Title,
                Link = x.Link
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Save(DomainClasses.DesignOrder_FinalDesign[] designs, int orderId)
        {
            DomainClasses.DesignOrder order = _context.DesignOrders.Find(orderId);

            bool mustSendNotification = false;

            if (designs.Length > 0 && order.IsSendFinalDesign == false)
            {
                order.IsSendFinalDesign = true;
                mustSendNotification = true;
            }

            List<DomainClasses.DesignOrder_FinalDesign> list = _context.DesignOrder_FinalDesigns
                .Where(x => x.Order.Id == orderId).ToList();

            //remove old link
            foreach (DomainClasses.DesignOrder_FinalDesign design in list)
            {
                _context.DesignOrder_FinalDesigns.Remove(design);
            }

            //add new link
            foreach (DomainClasses.DesignOrder_FinalDesign design in designs)
            {
                design.Order = order;
                _context.DesignOrder_FinalDesigns.Add(design);
            }

            _context.SaveChanges();

            //send notification
            if (mustSendNotification)
            {
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<Hubs.AdminNotification>();

                notificationHub.Clients.All.minusUnCheckedDesignOrders();
                notificationHub.Clients.All.minusUnSendedFinalDesignOfDesignOrders();
            }

            return Json(designs.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Link = x.Link
            }));
        }
    }
}