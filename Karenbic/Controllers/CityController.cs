using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Controllers
{
    public class CityController : Controller
    {
        private DataAccess.Context _context;

        public CityController(DataAccess.Context context)
        {
            _context = context;
        }

        public ViewResult GetOption(int provinceId, int? id)
        {
            List<DomainClasses.City> list = _context.Cities
                        .Where(x => x.Province.Id == provinceId)
                        .OrderBy(x => x.Name).ToList();

            ViewBag.Id = id;

            return View(list);
        }
    }
}