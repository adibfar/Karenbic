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
        public ActionResult ShowTypes()
        {
            List<DomainClasses.PortfolioType> model = _context.PortfolioTypes
                .OrderByDescending(x => x.Priority)
                .ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult ShowCategories(int typeId)
        {
            DomainClasses.PortfolioType model = _context.PortfolioTypes
                .Include(x => x.Categories)
                .Single(x => x.Id == typeId);

            return View(model);
        }

        [HttpGet]
        public ActionResult ShowPortfolios(int typeId, int categoryId)
        {
            ViewBag.PortfolioType =  _context.PortfolioTypes
                .Include(x => x.Categories)
                .Single(x => x.Id == typeId);

            ViewBag.PortfolioCategory = _context.PortfolioCategories
                .Include(x => x.Portfolios)
                .Single(x => x.Id == categoryId);

            return View();
        }
    }
}