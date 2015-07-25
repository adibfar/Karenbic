using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Admin.Controllers
{
    public class PortfolioTypeController : Controller
    {
        private DataAccess.Context _context;

        public PortfolioTypeController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            JsonResult result = new JsonResult();

            List<DomainClasses.PortfolioType> list = _context.PortfolioTypes
                .OrderByDescending(x => x.Priority)
                .ToList();

            result.Data = list.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Priority = x.Priority
            }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}