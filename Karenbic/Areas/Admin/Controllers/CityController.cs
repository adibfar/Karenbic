using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Admin.Controllers
{
    public class CityController : Controller
    {
        private DataAccess.Context _context;

        public CityController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Get(int provinceId)
        {
            JsonResult result = new JsonResult();

            result.Data = _context.Cities
                    .Where(x => x.Province.Id == provinceId)
                    .OrderBy(x => x.Name)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}