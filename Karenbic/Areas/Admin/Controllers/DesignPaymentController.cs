using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Admin.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Admin")]
    public class DesignPaymentController : Controller
    {
        private DataAccess.Context _context;

        public DesignPaymentController(DataAccess.Context context)
        {
            _context = context;
        }

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

            IQueryable<DomainClasses.DesignPayment> query = _context.DesignPayments.AsQueryable();
            query = query.Where(x => x.IsComplete && x.IsPaid);

            if (!string.IsNullOrEmpty(customerName))
            {
                query = query.Where(x => x.PrepaymentFactors.FirstOrDefault().Order.Customer.Name.Contains(customerName) ||
                    x.PrepaymentFactors.FirstOrDefault().Order.Customer.Surname.Contains(customerName) ||
                    x.FinalFactors.FirstOrDefault().Order.Customer.Name.Contains(customerName) ||
                    x.FinalFactors.FirstOrDefault().Order.Customer.Surname.Contains(customerName));
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

            List<DomainClasses.DesignPayment> list = query
                .Include(x => x.PrepaymentFactors)
                .Include(x => x.PrepaymentFactors.Select(c => c.Order))
                .Include(x => x.PrepaymentFactors.Select(c => c.Order.Customer))
                .Include(x => x.FinalFactors)
                .Include(x => x.FinalFactors.Select(c => c.Order))
                .Include(x => x.FinalFactors.Select(c => c.Order.Customer))
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
                    Customer = x.PrepaymentFactors.Count > 0 ? new
                    {
                        Name = x.PrepaymentFactors.First().Order.Customer.Name,
                        Surname = x.PrepaymentFactors.First().Order.Customer.Surname,
                        Username = x.PrepaymentFactors.First().Order.Customer.Username,
                        Phone = x.PrepaymentFactors.First().Order.Customer.Phone,
                        Mobile = x.PrepaymentFactors.First().Order.Customer.Mobile,
                        Email = x.PrepaymentFactors.First().Order.Customer.Email,
                        Address = x.PrepaymentFactors.First().Order.Customer.Address
                    } : new
                    {
                        Name = x.FinalFactors.First().Order.Customer.Name,
                        Surname = x.FinalFactors.First().Order.Customer.Surname,
                        Username = x.FinalFactors.First().Order.Customer.Username,
                        Phone = x.FinalFactors.First().Order.Customer.Phone,
                        Mobile = x.FinalFactors.First().Order.Customer.Mobile,
                        Email = x.FinalFactors.First().Order.Customer.Email,
                        Address = x.FinalFactors.First().Order.Customer.Address
                    }
                }).ToArray()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            JsonResult result = new JsonResult();

            DomainClasses.DesignPayment payment = _context.DesignPayments
                .Include(x => x.PrepaymentItems)
                .Include(x => x.FinalItems)
                .Single(x => x.Id == id);

            int[] prepaymentFactorsId = payment.PrepaymentItems.Select(x => x.FactorId).ToArray();
            int[] finalFactorsId = payment.FinalItems.Select(x => x.FactorId).ToArray();

            if (payment.IsPaid)
            {
                List<DomainClasses.PrepaymentDesignFactor> prepaymentFactors =
                    new List<DomainClasses.PrepaymentDesignFactor>();
                if (prepaymentFactorsId != null && prepaymentFactorsId.Length > 0)
                {
                    prepaymentFactors = _context.PrepaymentDesignFactors
                        .Include(x => x.Order)
                        .Include(x => x.Order.Form)
                        .Where(x => prepaymentFactorsId.Contains(x.Id))
                        .ToList();
                }

                List<DomainClasses.FinalDesignFactor> finalFactors = new List<DomainClasses.FinalDesignFactor>();
                if (finalFactorsId != null && finalFactorsId.Length > 0)
                {
                    finalFactors = _context.FinalDesignFactors
                        .Include(x => x.Order)
                        .Include(x => x.Order.Form)
                        .Where(x => finalFactorsId.Contains(x.Id))
                        .ToList();
                }

                List<object> factors = prepaymentFactors.Select(x => new
                {
                    Id = x.Id,
                    Time = string.Format("{0:D2}:{1:D2}", x.RegisterDate.Hour, x.RegisterDate.Minute),
                    RegisterDate = x.RegisterDate.ToShortDateString(),
                    PersianRegisterDate = x.PersianRegisterDate,
                    Price = x.Price,
                    IsPaid = x.IsPaid,
                    PaidDate = x.PaidDate,
                    PersianPaidDate = x.PersianPaidDate,
                    Index = 0,
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
                        Prepayment = ((DomainClasses.DesignOrder)x.Order).Prepayment,
                    },
                    //Form
                    Form = new
                    {
                        Id = x.Order.Form.Id,
                        Title = x.Order.Form.Title
                    }
                })
                .Union(finalFactors.Select(x => new
                {
                    Id = x.Id,
                    Time = string.Format("{0:D2}:{1:D2}", x.RegisterDate.Hour, x.RegisterDate.Minute),
                    RegisterDate = x.RegisterDate.ToShortDateString(),
                    PersianRegisterDate = x.PersianRegisterDate,
                    Price = x.Price,
                    IsPaid = x.IsPaid,
                    PaidDate = x.PaidDate,
                    PersianPaidDate = x.PersianPaidDate,
                    Index = 1,
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
                        Prepayment = ((DomainClasses.DesignOrder)x.Order).Prepayment,
                    },
                    //Form
                    Form = new
                    {
                        Id = x.Order.Form.Id,
                        Title = x.Order.Form.Title
                    }
                })).ToList<object>();

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
                    factors = factors
                };
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}