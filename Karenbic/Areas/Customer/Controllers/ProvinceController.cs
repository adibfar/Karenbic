using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Customer.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Customer")]
    public class ProvinceController : Controller
    {
        private DataAccess.Context _context;

        public ProvinceController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            JsonResult result = new JsonResult();

            result.Data = _context.Province
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