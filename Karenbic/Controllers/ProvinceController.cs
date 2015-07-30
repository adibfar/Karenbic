using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Controllers
{
    public class ProvinceController : Controller
    {
        private DataAccess.Context _context;

        public ProvinceController(DataAccess.Context context)
        {
            _context = context;
        }

        public ActionResult GetOption(int? id)
        {
            ViewBag.Id = id;

            List<DomainClasses.Province> list = _context.Province.ToList();

            return View(list);
        }
    }
}