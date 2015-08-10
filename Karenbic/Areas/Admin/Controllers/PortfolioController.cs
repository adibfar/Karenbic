using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Karenbic.Areas.Admin.Controllers
{
    public class PortfolioController : Controller
    {
        private DataAccess.Context _context;

        public PortfolioController(DataAccess.Context context) 
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(int categoryId, int priority, string description, 
            HttpPostedFileBase tumbPicture, HttpPostedFileBase picture)
        {
            if (!ModelState.IsValid) throw new Exception();

            DomainClasses.PortfolioCategory category = _context.PortfolioCategories
                .Include(x => x.Type)
                .Single(x => x.Id == categoryId);

            DomainClasses.Portfolio model = new DomainClasses.Portfolio();
            model.Category = category;
            model.Priority = priority;
            model.Description = description;

            if (tumbPicture != null &&
                (tumbPicture.ContentType == "image/jpg" || tumbPicture.ContentType == "image/jpeg" || tumbPicture.ContentType == "image/png") &&
                tumbPicture.ContentLength <= 250 * 1024)
            {
                model.TumbPictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(tumbPicture.FileName));
                tumbPicture.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Portfolio"), model.TumbPictureFile));
            }

            if (picture != null &&
                (picture.ContentType == "image/jpg" || picture.ContentType == "image/jpeg" || picture.ContentType == "image/png") &&
                picture.ContentLength <= 250 * 1024)
            {
                model.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(picture.FileName));
                picture.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Portfolio"), model.PictureFile));
            }

            _context.Portfolios.Add(model);
            _context.SaveChanges();

            return Json(new
            {
                Id = model.Id,
                Priority = model.Priority,
                TumbPictureFile = model.TumbPictureFile,
                TumbPicturePath = model.TumbPicturePath,
                PictureFile = model.PictureFile,
                PicturePath = model.PicturePath,
                Category = new
                {
                    Id = model.Category.Id,
                    Title = model.Category.Title,
                    Type = new
                    {
                        Id = model.Category.Type.Id,
                        Title = model.Category.Type.Title
                    }
                }
            });
        }

        [HttpPost]
        public ActionResult Edit(int id, int categoryId, int priority, string description,
            HttpPostedFileBase tumbPicture, HttpPostedFileBase picture)
        {
            //if (!ModelState.IsValid) throw new Exception();

            DomainClasses.PortfolioCategory category = _context.PortfolioCategories
                .Include(x => x.Type)
                .Single(x => x.Id == categoryId);

            DomainClasses.Portfolio model = _context.Portfolios.Find(id);
            model.Category = category;
            model.Priority = priority;
            model.Description = description;

            if (tumbPicture != null &&
                (tumbPicture.ContentType == "image/jpg" || tumbPicture.ContentType == "image/jpeg" || tumbPicture.ContentType == "image/png") &&
                tumbPicture.ContentLength <= 250 * 1024)
            {
                string oldFile = model.TumbPictureFile;

                //save new file
                model.TumbPictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(tumbPicture.FileName));
                tumbPicture.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Portfolio"), model.TumbPictureFile));

                //delete old picture
                if (System.IO.File.Exists(string.Format("{0}/{1}",
                    HostingEnvironment.MapPath("/Content/Portfolio"), oldFile)))
                {
                    System.IO.File.Delete(string.Format("{0}/{1}",
                    HostingEnvironment.MapPath("/Content/Portfolio"), oldFile));
                }
            }

            if (picture != null &&
                (picture.ContentType == "image/jpg" || picture.ContentType == "image/jpeg" || picture.ContentType == "image/png") &&
                picture.ContentLength <= 250 * 1024)
            {
                string oldFile = model.PictureFile;

                //save new file
                model.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(picture.FileName));
                picture.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Portfolio"), model.PictureFile));

                //delete old picture
                if (System.IO.File.Exists(string.Format("{0}/{1}",
                    HostingEnvironment.MapPath("/Content/Portfolio"), oldFile)))
                {
                    System.IO.File.Delete(string.Format("{0}/{1}",
                    HostingEnvironment.MapPath("/Content/Portfolio"), oldFile));
                }
            }

            _context.SaveChanges();

            return Json(new
            {
                Id = model.Id,
                Priority = model.Priority,
                TumbPictureFile = model.TumbPictureFile,
                TumbPicturePath = model.TumbPicturePath,
                PictureFile = model.PictureFile,
                PicturePath = model.PicturePath,
                Category = new
                {
                    Id = model.Category.Id,
                    Title = model.Category.Title,
                    Type = new
                    {
                        Id = model.Category.Type.Id,
                        Title = model.Category.Type.Title
                    }
                }
            });
        }

        [HttpGet]
        public async Task<ActionResult> Get(int? categoryId, int? typeId, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;

            JsonResult result = new JsonResult();

            IQueryable<DomainClasses.Portfolio> query = _context.Portfolios.AsQueryable();

            if (typeId != null)
            {
                query = query.Where(x => x.Category.Type.Id == typeId);
            }

            if (categoryId != null)
            {
                query = query.Where(x => x.Category.Id == categoryId);
            }

            int resultCount = await query.CountAsync();
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(resultCount) / Convert.ToDouble(pageSize)));

            List<DomainClasses.Portfolio> list = query.Include(x => x.Category)
                .Include(x => x.Category.Type)
                .OrderByDescending(x => x.Priority)
                .ToList();

            result.Data = new
            {
                ResultCount = resultCount,
                PageCount = pageCount,
                PageIndex = pageIndex,
                List = list.Select(x => new
                {
                    Id = x.Id,
                    Priority = x.Priority,
                    TumbPictureFile = x.TumbPictureFile,
                    TumbPicturePath = x.TumbPicturePath,
                    PictureFile = x.PictureFile,
                    PicturePath = x.PicturePath,
                    Description = x.Description,
                    Category = new
                    {
                        Id = x.Category.Id,
                        Title = x.Category.Title,
                        Type = new
                        {
                            Id = x.Category.Type.Id,
                            Title = x.Category.Type.Title
                        }
                    }
                }).ToArray()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            bool result = false;

            DomainClasses.Portfolio item = _context.Portfolios.Find(id);

            if (System.IO.File.Exists(string.Format("{0}/{1}",
                HostingEnvironment.MapPath("/Content/Portfolio"), item.PictureFile)))
            {
                System.IO.File.Delete(string.Format("{0}/{1}",
                HostingEnvironment.MapPath("/Content/Portfolio"), item.PictureFile));
            }

            _context.Portfolios.Remove(item);
            _context.SaveChanges();

            result = true;

            return Content(result.ToString());
        }
    }
}