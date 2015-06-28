using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Customer.Controllers
{
    public class ReceiveMessageController : Controller
    {
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Get(int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                IQueryable<DomainClasses.AdminMessage_Customer> query = context.AdminMessages_Customer.AsQueryable();

                DomainClasses.Customer customer = context.Customers.Find(1);
                //DomainClasses.Customer customer = context.Customers.Single(x => x.Username == User.Identity.Name);
                query = query.Where(x => x.Customer.Id == customer.Id);

                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(query.Count()) / Convert.ToDouble(pageSize)));
                int resultCount = query.Count();

                List<DomainClasses.AdminMessage_Customer> list = query
                    .OrderByDescending(x => x.SendDate)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                result.Data = new
                {
                    ResultCount = resultCount,
                    PageCount = pageCount,
                    PageIndex = pageIndex,
                    List = list.Select(x => new
                    {
                        Id = x.Id,
                        SendDate = Api.ConvertDate.JulainToPersian(x.SendDate),
                        Time = string.Format("{0:D2}:{1:D2}", x.SendDate.Hour, x.SendDate.Minute),
                        Title = x.Title,
                        Text = x.Text,
                        IsRead = x.IsRead
                    }).ToArray()
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MarkAsRead(Guid id)
        {
            bool result = false;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.AdminMessage_Customer message = context.AdminMessages_Customer.Find(id);
                message.IsRead = true;
                context.SaveChanges();
                result = true;
            }

            return Content(result.ToString());
        }

        [HttpPost]
        public ActionResult Remove(Guid id)
        {
            bool result = false;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.AdminMessage_Customer message = context.AdminMessages_Customer.Find(id);
                context.AdminMessages_Customer.Remove(message);
                context.SaveChanges();
                result = true;
            }

            return Content(result.ToString());
        }
    }
}