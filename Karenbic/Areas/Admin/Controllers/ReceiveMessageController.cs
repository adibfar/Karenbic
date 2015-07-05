﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Admin.Controllers
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
                IQueryable<DomainClasses.CustomerMessage> query = context.CustomerMessages.AsQueryable();
                query = query.Where(x => x.IsShowAdmin);

                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(query.Count()) / Convert.ToDouble(pageSize)));
                int resultCount = query.Count();

                List<DomainClasses.CustomerMessage> list = query
                    .Include(x => x.Sender)
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
                        IsRead  = x.IsReadAdmin,
                        IsAdminReply = x.IsAdminReply,
                        AdminReply = x.AdminReply,
                        Customer = new
                        {
                            Id = x.Sender.Id,
                            Name = x.Sender.Name,
                            Surname = x.Sender.Surname,
                            Phone = x.Sender.Phone,
                            Mobile = x.Sender.Mobile
                        }
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
                DomainClasses.CustomerMessage message = context.CustomerMessages
                        .Include(x => x.Sender)
                        .Single(x => x.Id == id);

                message.IsReadAdmin = true;
                context.SaveChanges();
                result = true;
            }

            return Content(result.ToString());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Reply(Guid id, string text)
        {
            bool result = false;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.CustomerMessage message = context.CustomerMessages
                    .Include(x => x.Sender)
                    .Single(x => x.Id == id);
                message.IsReadCustomer = false;
                message.IsAdminReply = true;
                message.AdminReply = text;
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
                DomainClasses.CustomerMessage message = context.CustomerMessages
                        .Include(x => x.Sender)
                        .Single(x => x.Id == id);

                if (message.IsShowCustomer)
                {
                    message.IsShowAdmin = false;
                }
                else
                {
                    context.CustomerMessages.Remove(message);
                }
                context.SaveChanges();
                result = true;
            }

            return Content(result.ToString());
        }
    }
}