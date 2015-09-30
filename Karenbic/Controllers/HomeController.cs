using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Controllers
{
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
            ViewBag.Slides = _context.HomeSlideShows
                .OrderByDescending(x => x.Priority).ToList();

            return View();
        }

        [HttpGet]
        public ActionResult AboutUs()
        {
            DomainClasses.Setting setting = _context.Setting.First();
            return View(setting);
        }

        [HttpGet]
        public ActionResult ContactUs()
        {
            DomainClasses.Setting setting = _context.Setting.First();
            return View(setting);
        }

        [HttpGet]
        public ActionResult Help()
        {
            DomainClasses.Setting setting = _context.Setting.First();
            return View(setting);
        }
    }
}