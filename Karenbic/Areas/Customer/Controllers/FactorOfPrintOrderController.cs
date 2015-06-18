using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Customer.Controllers
{
    public class FactorOfPrintOrderController : Controller
    {
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Get(bool isPaid, bool isNotPaid, string startDate, string endDate, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                IQueryable<DomainClasses.PrintOrder> query = context.PrintOrders
                    .Where(x => x.IsCanceled == false && x.IsConfirm)
                    .AsQueryable();

                DomainClasses.Customer customer = context.Customers.Find(1);
                //DomainClasses.Customer customer = context.Customers.Single(x => x.Username == User.Identity.Name);
                query = query.Where(x => x.Customer.Id == customer.Id);

                if (isPaid == true && isNotPaid == false)
                {
                    query = query.Where(x => x.IsPaid == true);
                }
                else if (isPaid == false && isNotPaid == true)
                {
                    query = query.Where(x => x.IsPaid == false);
                }
                else if (isPaid == false && isNotPaid == false)
                {
                    query = query.Where(x => x.IsPaid == true && x.IsPaid == false);
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

                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(query.Count()) / Convert.ToDouble(pageSize)));
                int resultCount = query.Count();

                List<DomainClasses.PrintOrder> list = query
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
                        //Order
                        Id = x.Id,
                        Code = x.Code,
                        Time = string.Format("{0:D2}:{1:D2}", x.RegisterDate.Hour, x.RegisterDate.Minute),
                        PersianRegisterDate = x.PersianRegisterDate,
                        //Confirm Order
                        IsConfirm = x.IsConfirm,
                        ConfirmDate = Api.ConvertDate.JulainToPersian(Convert.ToDateTime(x.ConfirmDate)),
                        Price = x.Price,
                        IsPaid = x.IsPaid,
                        //Form
                        Form = new
                        {
                            Id = x.Form.Id,
                            Title = x.Form.Title
                        },
                        //Factor
                        Factor = new 
                        {
                            Id = x.Factor.Id,
                            Time = string.Format("{0:D2}:{1:D2}", x.Factor.RegisterDate.Hour, x.Factor.RegisterDate.Minute),
                            PersianRegisterDate = x.Factor.PersianRegisterDate,
                            Price = x.Factor.Price,
                            IsPaid = x.Factor.IsPaid
                        },
                        //Payment
                        Payment = x.Factor.Payment != null ? new
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