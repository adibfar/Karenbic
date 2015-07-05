using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Customer.Controllers
{
    public class FormGroupController : Controller
    {
        [HttpGet]
        public ActionResult Get(DomainClasses.Portal portal)
        {
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                List<DomainClasses.FormGroup> list = context.FormGroups
                    .Include(x => x.Forms)
                    .Where(x => x.IsShow && x.Portal == portal)
                    .OrderByDescending(x => x.Priority)
                    .ThenBy(x => x.Title)
                    .ToList();

                result.Data = list.Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Column = x.Column,
                    Priority = x.Priority,
                    Forms = x.Forms
                        .Where(c => c.IsShow)
                        .OrderByDescending(c => c.Priority)
                        .ThenBy(c => c.Title)
                        .Select(c => new {
                            Id = c.Id,
                            Title = c.Title,
                            Priority = c.Priority,
                            SpecialCreativity = c.SpecialCreativity,
                            Description = c.Description,
                            CanDelete = c.CanDelete
                        })
                });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}