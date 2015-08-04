using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

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
            return View();
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            DomainClasses.PublicPriceCategory model = _context.PublicPriceCategories
                .Include(x => x.Prices)
                .Single(x => x.Id == id);

            return View(model);
        }
    }
}