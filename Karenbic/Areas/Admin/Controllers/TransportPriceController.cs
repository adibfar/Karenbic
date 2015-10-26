using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Admin.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Admin")]
    public class TransportPriceController : Controller
    {
        private DataAccess.Context _context;

        public TransportPriceController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Get()
        {
            DomainClasses.Setting setting = _context.Setting.First();

            return Json(new
            {
                BikeDelivery = setting.TransportPrice_BikeDelivery,
                Tipax = setting.TransportPrice_Tipax,
                Porterage = setting.TransportPrice_Porterage,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(decimal bikeDelivery, decimal tipax, decimal porterage)
        {
            DomainClasses.Setting setting = _context.Setting.First();
            setting.TransportPrice_BikeDelivery = bikeDelivery;
            setting.TransportPrice_Tipax = tipax;
            setting.TransportPrice_Porterage = porterage;
            _context.SaveChanges();

            return Json(new
            {
                BikeDelivery = bikeDelivery,
                Tipax = tipax,
                Porterage = porterage,
            });
        }
    }
}