using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Customer.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Customer")]
    public class HomeController : Controller
    {
        private DataAccess.Context _context;

        public HomeController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PublicHelpText()
        {
            string text = _context.Setting.Find(1).PublicHelpText;

            return Json(text, JsonRequestBehavior.AllowGet);
        }
    }
}