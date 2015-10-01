using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Customer.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Customer")]
    public class PaymentController : Controller
    {
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }
    }
}