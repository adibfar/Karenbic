using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Customer.Controllers
{
    public class PrintOrderController : Controller
    {
        [HttpGet]
        public ActionResult PreOrder()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PreOrderText()
        {
            string text = string.Empty;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                text = context.Setting.Find(1).PrePrintOrderText;
            }

            return Json(text, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Get(int? orderId, string startDate,
            string endDate, int[] states, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                IQueryable<DomainClasses.PrintOrder> query = context.PrintOrders.AsQueryable();

                DomainClasses.Customer customer = context.Customers.Find(1);
                //DomainClasses.Customer customer = context.Customers.Single(x => x.Username == User.Identity.Name);
                query = query.Where(x => x.Customer.Id == customer.Id);

                if (orderId != null)
                {
                    string tempOrderId = Convert.ToString(orderId);
                    query = query.Where(x => SqlFunctions.StringConvert((double)x.Id + 1024).Contains(tempOrderId));
                }

                if (!string.IsNullOrEmpty(startDate))
                {
                    DateTime julianStartDate = Api.ConvertDate.PersianTOJulian(startDate);
                    query = query.Where(x => x.RegisterDate >= julianStartDate);
                }

                if (!string.IsNullOrEmpty(endDate))
                {
                    DateTime tempJulianEndDate = Api.ConvertDate.PersianTOJulian(endDate);
                    DateTime julianEndDate = new DateTime(tempJulianEndDate.Year, tempJulianEndDate.Month, tempJulianEndDate.Day, 23, 59, 59, 50);
                    query = query.Where(x => x.RegisterDate <= julianEndDate);
                }

                if (states != null && states.Length > 0)
                {
                    DomainClasses.PrintOrderState[] orderStates = new DomainClasses.PrintOrderState[states.Length];

                    for (int i = 0; i < states.Length; i++)
                    {
                        orderStates[i] = (DomainClasses.PrintOrderState)states[i];
                    }
                    query = query.Where(x => orderStates.Contains(x.OrderState));
                }

                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(query.Count()) / Convert.ToDouble(pageSize)));
                int resultCount = query.Count();

                List<DomainClasses.PrintOrder> list = query
                    .Include(x => x.Customer)
                    .Include(x => x.Form)
                    .Include(x => x.Factor)
                    .Include(x => x.Factor.Payment)
                    .OrderByDescending(x => x.RegisterDate)
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
                        Code = x.Code,
                        Time = string.Format("{0:D2}:{1:D2}", x.RegisterDate.Hour, x.RegisterDate.Minute),
                        RegisterDate = x.RegisterDate.ToShortDateString(),
                        PersianRegisterDate = x.PersianRegisterDate,
                        OrderState = x.OrderState,
                        //Confirm Data
                        IsConfirm = x.IsConfirm,
                        ConfirmDate = Api.ConvertDate.JulainToPersian(Convert.ToDateTime(x.ConfirmDate)),
                        Price = x.Price,
                        PrintPrice = x.PrintPrice,
                        PackingPrice = x.PackingPrice,
                        //Cancel Data
                        IsCanceled = x.IsCanceled,
                        CancelDate = x.CancelDate != null ? Api.ConvertDate.JulainToPersian(Convert.ToDateTime(x.CancelDate)) : "",
                        Form = new
                        {
                            Id = x.Form.Id,
                            Title = x.Form.Title
                        },
                        //Factor
                        Factor = x.Factor != null ? new
                        {
                            Id = x.Factor.Id,
                            Time = string.Format("{0:D2}:{1:D2}", x.Factor.RegisterDate.Hour, x.Factor.RegisterDate.Minute),
                            PersianRegisterDate = x.Factor.PersianRegisterDate,
                            Price = x.Factor.Price,
                            IsPaid = x.Factor.IsPaid
                        } : new
                        {
                            Id = 0,
                            Time = "",
                            PersianRegisterDate = " ",
                            Price = (decimal)0,
                            IsPaid = false
                        },
                        //Payment
                        Payment = x.Factor!= null && x.Factor.Payment != null ? new
                        {
                            Id = x.Factor.Payment.Id,
                            Code = x.Factor.Payment.Code,
                            Money = x.Factor.Payment.Money,
                            PersianRegisterDate = x.Factor.Payment.PersianRegisterDate,
                            Time = string.Format("{0:D2}:{1:D2}", x.Factor.Payment.RegisterDate.Hour, x.Factor.Payment.RegisterDate.Minute),
                            IsPaid = x.Factor.Payment.IsPaid,
                            //Bank Data
                            RefId = x.Factor.Payment.RefId
                        } : new
                        {
                            Id = 0,
                            Code = 0,
                            Money = (decimal)0,
                            PersianRegisterDate = "",
                            Time = "",
                            IsPaid = false,
                            //Bank Data
                            RefId = ""
                        }
                    }).ToArray()
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}