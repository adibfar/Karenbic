﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Karenbic.Areas.Admin.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Admin")]
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
        public ActionResult Add(string title, int categoryId, int priority, string description, 
            HttpPostedFileBase mainFile, HttpPostedFileBase[] pictures)
        {
            if (!ModelState.IsValid) throw new Exception();

            DomainClasses.PortfolioCategory category = _context.PortfolioCategories
                .Include(x => x.Type)
                .Single(x => x.Id == categoryId);

            DomainClasses.Portfolio model = new DomainClasses.Portfolio();
            model.Title = title;
            model.Category = category;
            model.Priority = priority;
            model.Description = description;

            if (mainFile != null &&
                (mainFile.ContentType == "image/jpg" || mainFile.ContentType == "image/jpeg" || mainFile.ContentType == "image/png") &&
                mainFile.ContentLength <= 250 * 1024)
            {
                model.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(mainFile.FileName));
                mainFile.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Portfolio"), model.PictureFile));
            }

            foreach (HttpPostedFileBase picture in pictures)
            {
                if (picture != null &&
                    (picture.ContentType == "image/jpg" || picture.ContentType == "image/jpeg" || picture.ContentType == "image/png") &&
                    picture.ContentLength <= 150 * 1024)
                {
                    DomainClasses.PortfolioPicture pic = new DomainClasses.PortfolioPicture();
                    pic.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(picture.FileName));
                    picture.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Portfolio"), pic.PictureFile));
                    pic.Portfolio = model;
                    _context.PortfolioPictures.Add(pic);
                }
            }

            _context.Portfolios.Add(model);
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
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, string title, int categoryId, int priority, string description,
            HttpPostedFileBase mainFile, HttpPostedFileBase[] newPictures, string removedPictures)
        {
            //if (!ModelState.IsValid) throw new Exception();

            DomainClasses.PortfolioCategory category = _context.PortfolioCategories
                .Include(x => x.Type)
                .Single(x => x.Id == categoryId);

            DomainClasses.Portfolio model = _context.Portfolios.Find(id);
            model.Title = title;
            model.Category = category;
            model.Priority = priority;
            model.Description = description;

            if (mainFile != null &&
                (mainFile.ContentType == "image/jpg" || mainFile.ContentType == "image/jpeg" || mainFile.ContentType == "image/png") &&
                mainFile.ContentLength <= 250 * 1024)
            {
                string oldFile = model.PictureFile;

                //save new file
                model.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(mainFile.FileName));
                mainFile.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Portfolio"), model.PictureFile));

                //delete old picture
                if (System.IO.File.Exists(string.Format("{0}/{1}",
                    HostingEnvironment.MapPath("/Content/Portfolio"), oldFile)))
                {
                    System.IO.File.Delete(string.Format("{0}/{1}",
                    HostingEnvironment.MapPath("/Content/Portfolio"), oldFile));
                }
            }

            if (!string.IsNullOrEmpty(removedPictures))
            {
                string[] strRemovedPictures = removedPictures.Split(',');
                foreach (string pictureId in strRemovedPictures)
                {
                    DomainClasses.PortfolioPicture picture = _context.PortfolioPictures.Find(Convert.ToInt32(pictureId));
                    //remove picture
                    if (System.IO.File.Exists(string.Format("{0}/{1}",
                        HostingEnvironment.MapPath("/Content/Portfolio"), picture.PictureFile)))
                    {
                        System.IO.File.Delete(string.Format("{0}/{1}",
                        HostingEnvironment.MapPath("/Content/Portfolio"), picture.PictureFile));
                    }
                    _context.PortfolioPictures.Remove(picture);
                }
            }

            if (newPictures != null && newPictures.Count() > 0)
            {
                foreach (HttpPostedFileBase picture in newPictures)
                {
                    if (picture != null &&
                        (picture.ContentType == "image/jpg" || picture.ContentType == "image/jpeg" || picture.ContentType == "image/png") &&
                        picture.ContentLength <= 150 * 1024)
                    {
                        DomainClasses.PortfolioPicture pic = new DomainClasses.PortfolioPicture();
                        pic.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(picture.FileName));
                        picture.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Portfolio"), pic.PictureFile));
                        pic.Portfolio = model;
                        _context.PortfolioPictures.Add(pic);
                    }
                }
            }

            _context.SaveChanges();

            return Json(new
            {
                Id = model.Id,
                Priority = model.Priority,
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
        public ActionResult Find(int id)
        {

            DomainClasses.Portfolio product = _context.Portfolios
                .Include(x => x.Category)
                .Include(x => x.Category.Type)
                .Include(x => x.Pictures)
                .Single(x => x.Id == id);

            return Json(new
            {
                Id = product.Id,
                Title = product.Title,
                Priority = product.Priority,
                Description = product.Description,
                PictureFile = product.PictureFile,
                PicturePath = product.PicturePath,
                Type = new
                {
                    Id = product.Category.Type.Id,
                    Title = product.Category.Type.Title
                },
                Category = new
                {
                    Id = product.Category.Id,
                    Title = product.Category.Title
                },
                Pictures = product.Pictures.Select(x => new
                {
                    Id = x.Id,
                    PictureFile = x.PictureFile,
                    PicturePath = x.PicturePath
                })
            }, JsonRequestBehavior.AllowGet);
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
                    Title = x.Title,
                    Priority = x.Priority,
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

            DomainClasses.Portfolio item = _context.Portfolios
                .Single(x => x.Id == id);

            foreach (DomainClasses.PortfolioPicture picture in item.Pictures.ToList())
            {
                if (System.IO.File.Exists(string.Format("{0}/{1}",
                    HostingEnvironment.MapPath("~/Content/Portfolio"), picture.PictureFile)))
                {
                    System.IO.File.Delete(string.Format("{0}/{1}",
                        HostingEnvironment.MapPath("~/Content/Portfolio"), picture.PictureFile));
                }

                _context.PortfolioPictures.Remove(picture);
            }

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