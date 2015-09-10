using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        DataAccess.Context _context;

        public ProductController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(int categoryId, string title, int priority,
            decimal price, string description, HttpPostedFileBase mainFile, HttpPostedFileBase[] pictures)
        {
            DomainClasses.Product product = new DomainClasses.Product();
            product.Category = _context.ProductCategories.Find(categoryId);
            product.Title = title;
            product.Priority = priority;
            product.Price = price;
            product.Description = description;

            if (mainFile != null &&
                    (mainFile.ContentType == "image/jpg" || mainFile.ContentType == "image/jpeg" || mainFile.ContentType == "image/png") &&
                    mainFile.ContentLength <= 150 * 1024)
            {
                product.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(mainFile.FileName));
                mainFile.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Product"), product.PictureFile));
            }

            foreach (HttpPostedFileBase picture in pictures)
            {
                if (picture != null &&
                    (picture.ContentType == "image/jpg" || picture.ContentType == "image/jpeg" || picture.ContentType == "image/png") &&
                    picture.ContentLength <= 150 * 1024)
                {
                    DomainClasses.ProductPicture pic = new DomainClasses.ProductPicture();
                    pic.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(picture.FileName));
                    picture.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Product"), pic.PictureFile));
                    pic.Product = product;
                    _context.ProductPictures.Add(pic);
                }
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return Json(new 
            {
                Id = product.Id,
                Title = product.Title,
                Priority = product.Priority,
                Price = product.Price,
                Description = product.Description,
                PictureFile = product.PictureFile,
                PicturePath = product.PicturePath,
                Category = new
                {
                    Id = product.Category.Id,
                    Title = product.Category.Title
                }
            });
        }

        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, int categoryId, string title, int priority,
            decimal price, string description, HttpPostedFileBase mainFile, 
            HttpPostedFileBase[] newPictures, string removedPictures)
        {
            DomainClasses.Product product = _context.Products.Find(id);
            product.Category = _context.ProductCategories.Find(categoryId);
            product.Title = title;
            product.Priority = priority;
            product.Price = price;
            product.Description = description;

            if (mainFile != null &&
                    (mainFile.ContentType == "image/jpg" || mainFile.ContentType == "image/jpeg" || mainFile.ContentType == "image/png") &&
                    mainFile.ContentLength <= 150 * 1024)
            {
                //remove old picture
                if (System.IO.File.Exists(string.Format("{0}/{1}",
                    HostingEnvironment.MapPath("/Content/Product"), product.PictureFile)))
                {
                    System.IO.File.Delete(string.Format("{0}/{1}",
                    HostingEnvironment.MapPath("/Content/Product"), product.PictureFile));
                }

                //add new picture
                product.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(mainFile.FileName));
                mainFile.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Product"), product.PictureFile));
            }

            if (!string.IsNullOrEmpty(removedPictures))
            {
                string[] strRemovedPictures = removedPictures.Split(',');
                foreach (string pictureId in strRemovedPictures)
                {
                    DomainClasses.ProductPicture picture = _context.ProductPictures.Find(Convert.ToInt32(pictureId));
                    //remove picture
                    if (System.IO.File.Exists(string.Format("{0}/{1}",
                        HostingEnvironment.MapPath("/Content/Product"), picture.PictureFile)))
                    {
                        System.IO.File.Delete(string.Format("{0}/{1}",
                        HostingEnvironment.MapPath("/Content/Product"), picture.PictureFile));
                    }
                    _context.ProductPictures.Remove(picture);
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
                        DomainClasses.ProductPicture pic = new DomainClasses.ProductPicture();
                        pic.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(picture.FileName));
                        picture.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Product"), pic.PictureFile));
                        pic.Product = product;
                        _context.ProductPictures.Add(pic);
                    }
                }
            }

            _context.SaveChanges();

            return Json(new
            {
                Id = product.Id,
                Title = product.Title,
                Priority = product.Priority,
                Price = product.Price,
                Description = product.Description,
                PictureFile = product.PictureFile,
                PicturePath = product.PicturePath,
                Category = new
                {
                    Id = product.Category.Id,
                    Title = product.Category.Title
                }
            });
        }

        [HttpGet]
        public ActionResult Find(int id)
        {

            DomainClasses.Product product = _context.Products
                .Include(x => x.Category)
                .Include(x => x.Pictures)
                .Single(x => x.Id == id);

            return Json(new
                {
                    Id = product.Id,
                    Title = product.Title,
                    Priority = product.Priority,
                    Price = product.Price,
                    Description = product.Description,
                    PictureFile = product.PictureFile,
                    PicturePath = product.PicturePath,
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
        public async Task<ActionResult> Get(int? categoryId, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;

            JsonResult result = new JsonResult();

            IQueryable<DomainClasses.Product> query = _context.Products.AsQueryable();


            if (categoryId != null)
            {
                query = query.Where(x => x.Category.Id == categoryId);
            }

            int resultCount = await query.CountAsync();
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(resultCount) / Convert.ToDouble(pageSize)));

            List<DomainClasses.Product> list = query.Include(x => x.Category)
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
                        Title = x.Category.Title
                    }
                }).ToArray()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            bool result = true;

            DomainClasses.Product product = _context.Products
                .Include(x => x.Pictures)
                .Single(x => x.Id == id);

            foreach (DomainClasses.ProductPicture picture in product.Pictures.ToList())
            {
                if (System.IO.File.Exists(string.Format("{0}/{1}",
                    HostingEnvironment.MapPath("~/Content/Product"), picture.PictureFile)))
                {
                    System.IO.File.Delete(string.Format("{0}/{1}",
                        HostingEnvironment.MapPath("~/Content/Product"), picture.PictureFile));
                }

                _context.ProductPictures.Remove(picture);
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            result = true;

            return Content(result.ToString());
        }
    }
}