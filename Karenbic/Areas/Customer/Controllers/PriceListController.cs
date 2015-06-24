using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Customer.Controllers
{
    public class PriceListController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Get(DomainClasses.Portal portal)
        {
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                List<DomainClasses.PriceList> list = context.PriceLists
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
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}