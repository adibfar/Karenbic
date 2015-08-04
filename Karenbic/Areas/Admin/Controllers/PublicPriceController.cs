using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Admin.Controllers
{
    public class PublicPriceController : Controller
    {
        private DataAccess.Context _context;

        public PublicPriceController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string title, int priority, int categoryId, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid) throw new Exception();

            DomainClasses.PublicPrice model = new DomainClasses.PublicPrice();
            model.Title = title;
            model.Priority = priority;
            model.Category = _context.PublicPriceCategories.Find(categoryId);

            if (file != null)
            {
                if (file.ContentType == "image/jpg" || file.ContentType == "image/jpeg" || file.ContentType == "image/png")
                {
                    if (file.ContentLength <= 250 * 1024)
                    {
                        model.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(file.FileName));
                        file.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/PriceList"), model.PictureFile));
                    }
                }
            }

            _context.PublicPrices.Add(model);
            _context.SaveChanges();

            return Json(new 
            { 
                Id = model.Id,
                Title = model.Title,
                Priority = model.Priority,
                PictureFile = model.PictureFile,
                PicturePath = model.PicturePath,
                Category = new
                {
                    Id = model.Category.Id,
                    Title = model.Category.Title
                }
            });
        }

        [HttpPost]
        public ActionResult Edit(int id, string title, int priority, int categoryId, HttpPostedFileBase file)
        {
            DomainClasses.PublicPrice model = _context.PublicPrices.Find(id);
            model.Title = title;
            model.Priority = priority;
            model.Category = _context.PublicPriceCategories.Find(categoryId);

            if (file != null)
            {
                if (file.ContentType == "image/jpg" || file.ContentType == "image/jpeg" || file.ContentType == "image/png")
                {
                    if (file.ContentLength <= 250 * 1024)
                    {
                        string oldFile = model.PictureFile;

                        //save new picture
                        model.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(file.FileName));
                        file.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/PriceList"), model.PictureFile));

                        //delete old picture
                        if (System.IO.File.Exists(string.Format("{0}/{1}",
                            HostingEnvironment.MapPath("/Content/PriceList"), oldFile)))
                        {
                            System.IO.File.Delete(string.Format("{0}/{1}",
                            HostingEnvironment.MapPath("/Content/PriceList"), oldFile));
                        }
                    }
                }
            }

            _context.SaveChanges();

            return Json(new
            {
                Id = model.Id,
                Title = model.Title,
                Priority = model.Priority,
                PictureFile = model.PictureFile,
                PicturePath = model.PicturePath,
                Category = new
                {
                    Id = model.Category.Id,
                    Title = model.Category.Title
                }
            });
        }

        [HttpGet]
        public ActionResult Get(int categoryId)
        {
            JsonResult result = new JsonResult();

            List<DomainClasses.PublicPrice> list = _context.PublicPrices
                .OrderByDescending(x => x.Priority)
                .Include(x => x.Category)
                .Where(x => x.Category.Id == categoryId)
                .ToList();

            result.Data = list.Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Priority = x.Priority,
                    PictureFile = x.PictureFile,
                    PicturePath = x.PicturePath,
                    Category = new
                    {
                        Id = x.Category.Id,
                        Title = x.Category.Title
                    }
                }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            bool result = false;

            DomainClasses.PublicPrice item = _context.PublicPrices.Find(id);

            if (System.IO.File.Exists(string.Format("{0}/{1}",
                HostingEnvironment.MapPath("/Content/PriceList"), item.PictureFile)))
            {
                System.IO.File.Delete(string.Format("{0}/{1}",
                HostingEnvironment.MapPath("/Content/PriceList"), item.PictureFile));
            }

            _context.PublicPrices.Remove(item);
            _context.SaveChanges();

            result = true;

            return Content(result.ToString());
        }
    }
}