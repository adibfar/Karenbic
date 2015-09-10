using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Controllers
{
    public class PortfolioController : Controller
    {
        private DataAccess.Context _context;

        public PortfolioController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index(int typeId, int? categoryId, int pageIndex = 1)
        {
            //Fetch Portfolio Type
            ViewBag.PortfolioType =  _context.PortfolioTypes
                .Include(x => x.Categories)
                .Single(x => x.Id == typeId);

            //Fetch Portfolio Category
            if (categoryId != null)
            {
                ViewBag.PortfolioCategory = _context.PortfolioCategories.Find(categoryId);
            }

            //Fetch Portfolios
            IQueryable<DomainClasses.Portfolio> query = _context.Portfolios
                .Where(x => x.Category.Type.Id == typeId).AsQueryable();

            if (categoryId != null)
            {
                query = query.Where(x => x.Category.Id == categoryId);
            }

            ViewBag.Portfolios = query.OrderByDescending(x => x.Priority)
                .Include(x => x.Category)
                .Skip((pageIndex - 1) * 15).Take(15).ToList();

            return View();
        }
    }
}