using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Karenbic.Areas.Admin.Controllers
{
    public class HomeSlideShowController : Controller
    {
        private DataAccess.Context _context;

        public HomeSlideShowController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(int priority, HttpPostedFileBase file)
        {
            if(file != null &&
                (file.ContentType == "image/jpg" || 
                file.ContentType == "image/jpeg" || 
                file.ContentType == "image/png"))
            {
                DomainClasses.HomeSlideShow slide = new DomainClasses.HomeSlideShow();

                slide.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(file.FileName));
                file.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("~/Content/HomeSlideShow"), slide.PictureFile));
                slide.Priority = priority;

                _context.HomeSlideShows.Add(slide);
                _context.SaveChanges();

                return Json(new { 
                    Id = slide.Id,
                    Priority = slide.Priority,
                    PictureFile = slide.PictureFile,
                    PicturePath = slide.PicturePath
                });
            }

            throw new Exception();
        }

        [HttpGet]
        public ActionResult Get()
        {
            List<DomainClasses.HomeSlideShow> list = _context.HomeSlideShows
                .OrderByDescending(x => x.Priority).ToList();

            return Json(list.Select(x => new 
            { 
                Id = x.Id,
                Priority = x.Priority,
                PictureFile = x.PictureFile,
                PicturePath = x.PicturePath
            }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            DomainClasses.HomeSlideShow slide = _context.HomeSlideShows.Find(id);

            if (System.IO.File.Exists(string.Format("{0}/{1}", HostingEnvironment.MapPath("~/Content/HomeSlideShow"), slide.PictureFile)))
            {
                System.IO.File.Delete(string.Format("{0}/{1}", HostingEnvironment.MapPath("~/Content/HomeSlideShow"), slide.PictureFile));
            }

            _context.HomeSlideShows.Remove(slide);
            _context.SaveChanges();

            return Content("True");
        }
    }
}