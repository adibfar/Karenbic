using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Customer.Controllers
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
            DomainClasses.Customer customer = _context.Customers.Find(1);
            //DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

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