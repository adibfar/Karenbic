using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Controllers
{
    public class PriceListController : Controller
    {
        private DataAccess.Context _context;

        public PriceListController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<DomainClasses.PriceList> list = _context.PriceLists
                .OrderByDescending(x => x.Order)
                .ToList();

            return View(list);
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            List<DomainClasses.PriceList> list = _context.PriceLists
                .OrderByDescending(x => x.Order)
                .ToList();

            ViewBag.selectedItem = _context.PriceLists.Find(id);

            return View(list);
        }
    }
}