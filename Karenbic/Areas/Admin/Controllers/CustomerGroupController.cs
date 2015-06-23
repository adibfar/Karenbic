using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Admin.Controllers
{
    public class CustomerGroupController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(DomainClasses.CustomerGroup model)
        {
            if (!ModelState.IsValid) throw new Exception();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                context.CustomerGroups.Add(model);
                context.SaveChanges();
            }

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

            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.CustomerGroup item = context.CustomerGroups.Find(model.Id);
                item.Title = model.Title;
                context.SaveChanges();
            }

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

            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.CustomerGroup item = context.CustomerGroups.Find(id);
                try
                {
                    context.CustomerGroups.Remove(item);
                    context.SaveChanges();
                    result = true;
                }
                catch { }
            }

            return Content(result.ToString());
        }

        [HttpGet]
        public ActionResult Get()
        {
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                result.Data = context.CustomerGroups
                    .Select(x => new
                    {
                        Id = x.Id,
                        Title = x.Title
                    }).ToArray();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}