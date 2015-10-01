using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Admin.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Admin")]
    public class CustomerGroupController : Controller
    {
        private DataAccess.Context _context;

        public CustomerGroupController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(DomainClasses.CustomerGroup model)
        {
            if (!ModelState.IsValid) throw new Exception();

            _context.CustomerGroups.Add(model);
            _context.SaveChanges();

            return Json(new 
            { 
                Id = model.Id,
                Title = model.Title
            });
        }

        [HttpPost]
        public ActionResult Edit(DomainClasses.CustomerGroup model)
        {
            if (!ModelState.IsValid) throw new Exception();

            DomainClasses.CustomerGroup item = _context.CustomerGroups.Find(model.Id);
            item.Title = model.Title;
            _context.SaveChanges();

            return Json(new
            {
                Id = model.Id,
                Title = model.Title
            });
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            bool result = false;

            DomainClasses.CustomerGroup item = _context.CustomerGroups.Find(id);
            try
            {
                _context.CustomerGroups.Remove(item);
                _context.SaveChanges();
                result = true;
            }
            catch { }

            return Content(result.ToString());
        }

        [HttpGet]
        public ActionResult Get()
        {
            JsonResult result = new JsonResult();

            result.Data = _context.CustomerGroups
                .Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}