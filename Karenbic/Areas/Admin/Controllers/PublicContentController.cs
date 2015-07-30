using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Admin.Controllers
{
    public class PublicContentController : Controller
    {
        private DataAccess.Context _context { get; set; }

        public PublicContentController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AboutUsText()
        {
            string text = string.Empty;

            text = _context.Setting.Find(1).AboutUsText;

            return Json(text, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AboutUsText(string text)
        {
            DomainClasses.Setting setting = _context.Setting.Find(1);
            setting.AboutUsText = text;
            _context.SaveChanges();

            return Json(text, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ContactUsText()
        {
            string text = string.Empty;

            text = _context.Setting.Find(1).ContactUsText;

            return Json(text, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ContactUsText(string text)
        {
            DomainClasses.Setting setting = _context.Setting.Find(1);
            setting.ContactUsText = text;
            _context.SaveChanges();

            return Json(text, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PublicHelp()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PublicHelpText()
        {
            string text = string.Empty;

            text = _context.Setting.Find(1).PublicHelpText;

            return Json(text, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PublicHelpText(string text)
        {
            DomainClasses.Setting setting = _context.Setting.Find(1);
            setting.PublicHelpText = text;
            _context.SaveChanges();

            return Json(text, JsonRequestBehavior.AllowGet);
        }
    }
}