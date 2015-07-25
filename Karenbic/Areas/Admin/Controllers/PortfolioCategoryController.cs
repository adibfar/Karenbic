using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Admin.Controllers
{
    public class PortfolioCategoryController : Controller
    {
        private DataAccess.Context _context;

        public PortfolioCategoryController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string title, int typeId, int priority)
        {
            if (!ModelState.IsValid) throw new Exception();

            DomainClasses.PortfolioCategory model = new DomainClasses.PortfolioCategory();
            model.Title = title;
            model.Type = _context.PortfolioTypes.Find(typeId);
            model.Priority = priority;

            _context.PortfolioCategories.Add(model);
            _context.SaveChanges();

            return Json(new 
            { 
                Id = model.Id,
                Title = model.Title,
                Priority = model.Priority,
                Type = new
                {
                    Id = model.Type.Id,
                    Title = model.Type.Title
                }
            });
        }

        [HttpPost]
        public ActionResult Edit(int id, string title, int typeId, int priority)
        {
            DomainClasses.PortfolioCategory model = _context.PortfolioCategories.Find(id);
            model.Title = title;
            model.Type = _context.PortfolioTypes.Find(typeId);
            model.Priority = priority;

            _context.SaveChanges();

            return Json(new
            {
                Id = model.Id,
                Title = model.Title,
                Priority = model.Priority,
                Type = new
                {
                    Id = model.Type.Id,
                    Title = model.Type.Title
                }
            });
        }

        [HttpGet]
        public ActionResult Get(int? typeId)
        {
            JsonResult result = new JsonResult();

            IQueryable<DomainClasses.PortfolioCategory> query = _context.PortfolioCategories.AsQueryable();

            if (typeId != null)
            {
                query = query.Where(x => x.Type.Id == typeId);
            }

            List<DomainClasses.PortfolioCategory> list = query.Include(x => x.Type)
                .OrderByDescending(x => x.Priority)
                .ToList();

            result.Data = list.Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Priority = x.Priority,
                    Type = new
                    {
                        Id = x.Type.Id,
                        Title = x.Type.Title
                    }
                }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            bool result = false;

            DomainClasses.PortfolioCategory item = _context.PortfolioCategories.Find(id);

            try
            {
                _context.PortfolioCategories.Remove(item);
                _context.SaveChanges();

                result = true;
            }
            catch
            {
                
            }

            return Content(result.ToString());
        }
    }
}