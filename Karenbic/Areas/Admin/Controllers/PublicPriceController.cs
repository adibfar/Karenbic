using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Admin.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Admin")]
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

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(DomainClasses.PublicPrice model, int categoryId)
        {
            model.Category = _context.PublicPriceCategories.Find(categoryId);

            if (!ModelState.IsValid) throw new Exception();

            _context.PublicPrices.Add(model);
            _context.SaveChanges();

            return Json(new 
            { 
                Id = model.Id,
                Title = model.Title,
                Priority = model.Priority,
                Description = model.Description,
                Category = new
                {
                    Id = model.Category.Id,
                    Title = model.Category.Title
                }
            });
        }

        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(DomainClasses.PublicPrice model, int categoryId)
        {
            DomainClasses.PublicPrice item = _context.PublicPrices.Find(model.Id);
            item.Title = model.Title;
            item.Priority = model.Priority;
            item.Description = model.Description;
            item.Category = _context.PublicPriceCategories.Find(categoryId);

            _context.SaveChanges();

            return Json(new
            {
                Id = item.Id,
                Title = item.Title,
                Priority = item.Priority,
                Description = item.Description,
                Category = new
                {
                    Id = item.Category.Id,
                    Title = item.Category.Title
                }
            });
        }

        [HttpGet]
        public ActionResult Find(int id)
        {
            DomainClasses.PublicPrice model = _context.PublicPrices
                .Include(x => x.Category)
                .Single(x => x.Id == id);

            return Json(new {
                Id = model.Id,
                Title = model.Title,
                Priority = model.Priority,
                Description = model.Description,
                Category = new
                {
                    Id = model.Category.Id,
                    Title = model.Category.Title
                }
            }, JsonRequestBehavior.AllowGet);
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

            _context.PublicPrices.Remove(item);
            _context.SaveChanges();

            result = true;

            return Content(result.ToString());
        }
    }
}