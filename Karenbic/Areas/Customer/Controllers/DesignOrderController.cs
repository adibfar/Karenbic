using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Customer.Controllers
{
    public class DesignOrderController : Controller
    {
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
    }
}