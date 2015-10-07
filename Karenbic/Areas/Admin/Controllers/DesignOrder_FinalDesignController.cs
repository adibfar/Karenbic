using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Admin.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Admin")]
    public class DesignOrder_FinalDesignController : Controller
    {
        private DataAccess.Context _context;
        private SMSService.ISMSService _SMSService;

        public DesignOrder_FinalDesignController(DataAccess.Context context, SMSService.ISMSService SMSService)
        {
            _context = context;
            _SMSService = SMSService;
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
            DomainClasses.DesignOrder order = _context.DesignOrders
                .Include(x => x.Customer)
                .Single(x => x.Id == orderId);
            order.AdminMustSeeIt = false;
            order.CustomerMustSeeIt = true;

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
                var adminNotification = GlobalHost.ConnectionManager.GetHubContext<Hubs.AdminNotification>();
                adminNotification.Clients.All.minusUnCheckedDesignOrders();
                adminNotification.Clients.All.minusUnSendedFinalDesignOfDesignOrders();

                var customerNotification = GlobalHost.ConnectionManager.GetHubContext<Hubs.CustomerNotification>();
                if (Hubs.CustomerNotification.Users.Any(x => x.Key == order.Customer.Username))
                {
                    customerNotification.Clients
                        .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == order.Customer.Username)
                        .Value.ConnectionIds.ToArray<string>()).newUnreviewedDesign();
                }
            }

            //Send SMS
            _SMSService.Send(new string[1] { order.Customer.Mobile },
                string.Format(@"استودیو کارن
{0} عزیز
فایل نهایی سفارش طراحی شماره {1} برای شما ارسال شد. جهت دریافت به بخش نمایش سفارشات مراجعه فرمائید.",
                                order.Customer.Name,
                                order.Code),
                false);

            return Json(designs.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Link = x.Link
            }));
        }
    }
}