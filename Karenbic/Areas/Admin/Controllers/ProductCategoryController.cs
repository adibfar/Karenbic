using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Admin.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Admin")]
    public class ProductCategoryController : Controller
    {
        public DataAccess.Context _context;

        public ProductCategoryController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult Add(string title, int priority)
        {
            DomainClasses.ProductCategory category = new DomainClasses.ProductCategory();
            category.Title = title;
            category.Priority = priority;

            _context.ProductCategories.Add(category);
            _context.SaveChanges();

            return Json(new 
            { 
                Id = category .Id,
                Title = category.Title,
                Priority = category.Priority
            });
        }

        [HttpPost]
        public ActionResult Edit(int id, string title, int priority)
        {
            DomainClasses.ProductCategory category = _context.ProductCategories.Find(id);
            category.Title = title;
            category.Priority = priority;

            _context.SaveChanges();

            return Json(new
            {
                Id = category.Id,
                Title = category.Title,
                Priority = category.Priority
            });
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Get()
        {
            List<DomainClasses.ProductCategory> list = _context.ProductCategories
                .OrderByDescending(x => x.Priority).ToList();

            return Json(list.Select(x => new 
            { 
                Id = x.Id,
                Title = x.Title,
                Priority = x.Priority
            }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            bool result = false;

            DomainClasses.ProductCategory item = _context.ProductCategories.Find(id);
            try
            {
                _context.ProductCategories.Remove(item);
                _context.SaveChanges();
                result = true;
            }
            catch { }

            return Content(result.ToString());
        }
    }
}