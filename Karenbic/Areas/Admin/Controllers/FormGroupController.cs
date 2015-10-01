using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Karenbic.Areas.Admin.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Admin")]
    public class FormGroupController : Controller
    {
        private DataAccess.Context _context;

        public FormGroupController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(DomainClasses.FormGroup model)
        {
            if (!ModelState.IsValid) throw new Exception();

            _context.FormGroups.Add(model);
            _context.SaveChanges();

            return Json(new
            {
                Id = model.Id,
                Portal = model.Portal,
                IsShow = model.IsShow,
                Title = model.Title,
                Column = model.Column,
                Priority = model.Priority
            });
        }

        [HttpPost]
        public ActionResult Edit(DomainClasses.FormGroup model)
        {
            if (!ModelState.IsValid) throw new Exception();

            DomainClasses.FormGroup item = _context.FormGroups.Find(model.Id);
            item.Title = model.Title;
            item.Column = model.Column;
            item.Priority = model.Priority;
            _context.SaveChanges();

            return Json(new
            {
                Id = model.Id,
                Portal = model.Portal,
                IsShow = model.IsShow,
                Title = model.Title,
                Column = model.Column,
                Priority = model.Priority
            });
        }

        [HttpPost]
        public ActionResult Show(int id)
        {
            bool result = false;

            DomainClasses.FormGroup item = _context.FormGroups.Find(id);
            item.IsShow = true;
            _context.SaveChanges();
            result = true;

            return Content(result.ToString());
        }

        [HttpPost]
        public ActionResult Hide(int id)
        {
            bool result = false;

            DomainClasses.FormGroup item = _context.FormGroups.Find(id);
            item.IsShow = false;
            _context.SaveChanges();
            result = true;

            return Content(result.ToString());
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            bool result = false;

            DomainClasses.FormGroup item = _context.FormGroups.Find(id);
            try
            {
                _context.FormGroups.Remove(item);
                _context.SaveChanges();
                result = true;
            }
            catch { }

            return Content(result.ToString());
        }

        [HttpGet]
        public ActionResult Get(DomainClasses.Portal portal)
        {
            JsonResult result = new JsonResult();

            result.Data = _context.FormGroups
                .Where(x => x.Portal == portal)
                .OrderByDescending(x => x.Column)
                .ThenByDescending(x => x.Priority)
                .Select(x => new
                {
                    Id = x.Id,
                    Portal = x.Portal,
                    IsShow = x.IsShow,
                    Title = x.Title,
                    Column = x.Column,
                    Priority = x.Priority
                })
                .ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}