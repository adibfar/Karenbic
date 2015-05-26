using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Customer.Controllers
{
    public class PortalController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}