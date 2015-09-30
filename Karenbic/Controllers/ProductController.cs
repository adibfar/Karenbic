using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Controllers
{
    public class ProductController : Controller
    {
        private DataAccess.Context _context;

        public ProductController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index(int? categoryId, int pageIndex = 1)
        {
            ViewBag.CategoryId = categoryId;

            ViewBag.ProductCategories = _context.ProductCategories
                .OrderByDescending(x => x.Priority)
                .ToList();

            IQueryable<DomainClasses.Product> query = _context.Products.AsQueryable();

            if (categoryId != null)
            {
                query = query.Where(x => x.Category.Id == categoryId);
            }

            ViewBag.Products = query.Include(x => x.Category)
                .OrderByDescending(x => x.Priority)
                .Skip((pageIndex - 1) * 20).Take(20)
                .ToList();

            return View();
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            DomainClasses.Product product = _context.Products
                .Include(x => x.Category)
                .Include(x => x.Pictures)
                .Single(x => x.Id == id);


            ViewBag.ProductCategories = _context.ProductCategories
                .OrderByDescending(x => x.Priority)
                .ToList();

            return View(product);
        }
    }
}