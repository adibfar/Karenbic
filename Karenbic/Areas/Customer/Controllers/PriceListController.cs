using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Customer.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Customer")]
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
        public ActionResult GetCategory()
        {
            List<DomainClasses.PublicPriceCategory> list = _context.PublicPriceCategories
                .OrderByDescending(x => x.Priority).ToList();

            return Json(list.Select(c => new
            {
                Id = c.Id,
                Title = c.Title,
                Priority = c.Priority
            }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int categoryId)
        {
            List<DomainClasses.PublicPrice> list = _context.PublicPrices
                .Where(x => x.Category.Id == categoryId)
                .OrderByDescending(x => x.Priority)
                .ToList();

            return Json(list.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Priority = x.Priority,
                Description = x.Description
            }).ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}