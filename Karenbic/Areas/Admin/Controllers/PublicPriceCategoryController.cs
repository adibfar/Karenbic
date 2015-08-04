using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Admin.Controllers
{
    public class PublicPriceCategoryController : Controller
    {
        DataAccess.Context _context;

        public PublicPriceCategoryController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            List<DomainClasses.PublicPriceCategory> list = _context.PublicPriceCategories
                .OrderByDescending(x => x.Priority)
                .ToList();

            return Json(list.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Priority = x.Priority
            }), JsonRequestBehavior.AllowGet);
        }
    }
}