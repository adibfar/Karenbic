using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Customer.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Customer")]
    public class PortalController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}