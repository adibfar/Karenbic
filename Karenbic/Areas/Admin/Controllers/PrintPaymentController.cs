using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Admin.Controllers
{
    public class PrintPaymentController : Controller
    {
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Get(string customerName, string startDate, string endDate, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                IQueryable<DomainClasses.PrintPayment> query = context.PrintPayments.AsQueryable();
                query = query.Where(x => x.IsComplete && x.IsPaid);

                if (!string.IsNullOrEmpty(customerName))
                {
                    query = query.Where(x => x.Factors.FirstOrDefault().Order.Customer.Name.Contains(customerName) ||
                        x.Factors.FirstOrDefault().Order.Customer.Surname.Contains(customerName));
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

                List<DomainClasses.PrintPayment> list = query
                    .Include(x => x.Factors)
                    .Include(x => x.Factors.Select(c => c.Order))
                    .Include(x => x.Factors.Select(c => c.Order.Customer))
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
                        Money = x.Money,
                        Time = string.Format("{0:D2}:{1:D2}", x.RegisterDate.Hour, x.RegisterDate.Minute),
                        RegisterDate = x.RegisterDate.ToShortDateString(),
                        PersianRegisterDate = x.PersianRegisterDate,
                        RefId = x.RefId,
                        Customer = new
                        {
                            Name = x.Factors.First().Order.Customer.Name,
                            Surname = x.Factors.First().Order.Customer.Surname,
                            Username = x.Factors.First().Order.Customer.Username,
                            Phone = x.Factors.First().Order.Customer.Phone,
                            Mobile = x.Factors.First().Order.Customer.Mobile,
                            Email = x.Factors.First().Order.Customer.Email,
                            Address = x.Factors.First().Order.Customer.Address
                        }
                    }).ToArray()
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.PrintPayment payment = context.PrintPayments
                    .Include(x => x.Items)
                    .Single(x => x.Id == id);

                int[] factorsId = payment.Items.Select(x => x.FactorId).ToArray();

                List<DomainClasses.PrintFactor> factors = context.PrintFactors
                    .Include(x => x.Order)
                    .Include(x => x.Order.Form)
                    .Where(x => factorsId.Contains(x.Id))
                    .ToList();

                result.Data = new
                {
                    payment = new
                    {
                        Id = payment.Id,
                        Code = payment.Code,
                        Money = payment.Money,
                        PersianRegisterDate = payment.PersianRegisterDate,
                        Time = string.Format("{0:D2}:{1:D2}", payment.RegisterDate.Hour, payment.RegisterDate.Minute),
                        IsPaid = payment.IsPaid,
                        //Bank Data
                        RefId = payment.RefId
                    },
                    factors = factors.Select(x => new
                    {
                        Id = x.Id,
                        Time = string.Format("{0:D2}:{1:D2}", x.RegisterDate.Hour, x.RegisterDate.Minute),
                        RegisterDate = x.RegisterDate.ToShortDateString(),
                        PersianRegisterDate = x.PersianRegisterDate,
                        Price = x.Price,
                        IsPaid = x.IsPaid,
                        PaidDate = x.PaidDate,
                        PersianPaidDate = x.PersianPaidDate,
                        //Order
                        Order = new
                        {
                            Id = x.Order.Id,
                            Code = x.Order.Code,
                            Time = string.Format("{0:D2}:{1:D2}", x.Order.RegisterDate.Hour, x.Order.RegisterDate.Minute),
                            RegisterDate = x.Order.RegisterDate.ToShortDateString(),
                            PersianRegisterDate = x.Order.PersianRegisterDate,
                            //Confirm
                            IsConfirm = x.Order.IsConfirm,
                            ConfirmDate = Api.ConvertDate.JulainToPersian(Convert.ToDateTime(x.Order.ConfirmDate)),
                            Price = x.Order.Price,
                        },
                        //Form
                        Form = new
                        {
                            Id = x.Order.Form.Id,
                            Title = x.Order.Form.Title
                        }
                    })
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}