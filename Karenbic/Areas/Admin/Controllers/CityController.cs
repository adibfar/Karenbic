using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Admin.Controllers
{
    public class CityController : Controller
    {
        [HttpGet]
        public ActionResult Get(int provinceId)
        {
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                result.Data = context.Cities
                    .Where(x => x.Province.Id == provinceId)
                    .OrderBy(x => x.Name)
                    .Select(x => new
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToArray();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}