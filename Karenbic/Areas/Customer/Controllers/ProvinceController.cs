using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Customer.Controllers
{
    public class ProvinceController : Controller
    {
        [HttpGet]
        public ActionResult Get()
        {
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                result.Data = context.Province
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