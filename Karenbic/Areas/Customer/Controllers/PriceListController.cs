using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Customer.Controllers
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
        public ActionResult Get(DomainClasses.Portal portal)
        {
            JsonResult result = new JsonResult();

            List<DomainClasses.PriceList> list = _context.PriceLists
                .Where(x => x.Portal == portal)
                .OrderBy(x => x.Order)
                .ToList();

            result.Data = list.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Order = x.Order,
                PictureFile = x.PictureFile,
                PicturePath = x.PicturePath
            }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}