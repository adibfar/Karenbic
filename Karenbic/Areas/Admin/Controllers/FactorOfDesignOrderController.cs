﻿using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Admin.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Admin")]
    public class FactorOfDesignOrderController : Controller
    {
        private DataAccess.Context _context;

        public FactorOfDesignOrderController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult FactorText()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetFactorText()
        {
            string text = string.Empty;

            text = _context.Setting.Find(1).DesignFactorText;

            return Json(text, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetFactorText(string text)
        {
            DomainClasses.Setting setting = _context.Setting.Find(1);
            setting.DesignFactorText = text;
            _context.SaveChanges();

            return Json(text, JsonRequestBehavior.AllowGet);
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

            IQueryable<DomainClasses.DesignOrder> query = _context.DesignOrders.AsQueryable();

            query = query.Where(x => x.IsCanceled == false);
            query = query.Where(x => x.IsPaidPrepayment || x.IsPaidFinal);

            if (!string.IsNullOrEmpty(customerName))
            {
                query = query.Where(x => x.Customer.Name.Contains(customerName) ||
                    x.Customer.Surname.Contains(customerName));
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
            decimal totalPrepayment = query.Any(x => x.IsPaidPrepayment) ? query.Where(x => x.IsPaidPrepayment).Sum(x => x.Prepayment) : 0;
            decimal totalFinal = query.Any(x => x.IsPaidFinal) ? query.Where(x => x.IsPaidFinal).Sum(x => x.Price - x.Prepayment) : 0;
            decimal totalPrice = totalPrepayment + totalFinal;

            List<DomainClasses.DesignOrder> list = query
                .Include(x => x.Customer)
                .Include(x => x.Form)
                .Include(x => x.PrepaymentFactor)
                .Include(x => x.PrepaymentFactor.Payment)
                .Include(x => x.FinalFactor)
                .Include(x => x.FinalFactor.Payment)
                .OrderByDescending(x => x.RegisterDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            result.Data = new
            {
                ResultCount = resultCount,
                PageCount = pageCount,
                PageIndex = pageIndex,
                TotalPrice = totalPrice,
                List = list.Select(x => new
                {
                    Id = x.Id,
                    Code = x.Code,
                    Time = string.Format("{0:D2}:{1:D2}", x.RegisterDate.Hour, x.RegisterDate.Minute),
                    RegisterDate = x.RegisterDate.ToShortDateString(),
                    PersianRegisterDate = x.PersianRegisterDate,
                    //Confirm Data
                    IsConfirm = x.IsConfirm,
                    ConfirmDate = Api.ConvertDate.JulainToPersian(Convert.ToDateTime(x.ConfirmDate)),
                    Price = x.Price,
                    Prepayment = x.Prepayment,
                    Customer = new
                    {
                        Name = x.Customer.Name,
                        Surname = x.Customer.Surname,
                        Username = x.Customer.Username,
                        Phone = x.Customer.Phone,
                        Mobile = x.Customer.Mobile,
                        Email = x.Customer.Email,
                        Address = x.Customer.Address
                    },
                    Form = new
                    {
                        Id = x.Form.Id,
                        Title = x.Form.Title
                    },
                    //Prepayment Factor
                    PrepaymentFactor = new
                    {
                        Id = x.PrepaymentFactor.Id,
                        Time = string.Format("{0:D2}:{1:D2}", x.PrepaymentFactor.RegisterDate.Hour, x.PrepaymentFactor.RegisterDate.Minute),
                        PersianRegisterDate = x.PrepaymentFactor.PersianRegisterDate,
                        Price = x.PrepaymentFactor.Price,
                        IsPaid = x.PrepaymentFactor.IsPaid
                    },
                    //Prepayment Payment
                    PrepaymentPayment = x.PrepaymentFactor.Payment != null ? new
                    {
                        Id = x.PrepaymentFactor.Payment.Id,
                        Code = x.PrepaymentFactor.Payment.Code,
                        Money = x.PrepaymentFactor.Payment.Money,
                        PersianRegisterDate = x.PrepaymentFactor.Payment.PersianRegisterDate,
                        Time = string.Format("{0:D2}:{1:D2}", x.PrepaymentFactor.Payment.RegisterDate.Hour, x.PrepaymentFactor.Payment.RegisterDate.Minute),
                        IsPaid = x.PrepaymentFactor.Payment.IsPaid,
                        //Bank Data
                        RefId = x.PrepaymentFactor.Payment.RefId
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
                    },
                    //Final Factor
                    FinalFactor = new
                    {
                        Id = x.FinalFactor.Id,
                        Time = string.Format("{0:D2}:{1:D2}", x.FinalFactor.RegisterDate.Hour, x.FinalFactor.RegisterDate.Minute),
                        PersianRegisterDate = x.FinalFactor.PersianRegisterDate,
                        Price = x.FinalFactor.Price,
                        IsPaid = x.FinalFactor.IsPaid
                    },
                    //Final Payment
                    FinalPayment = x.FinalFactor.Payment != null ? new
                    {
                        Id = x.FinalFactor.Payment.Id,
                        Code = x.FinalFactor.Payment.Code,
                        Money = x.FinalFactor.Payment.Money,
                        PersianRegisterDate = x.FinalFactor.Payment.PersianRegisterDate,
                        Time = string.Format("{0:D2}:{1:D2}", x.FinalFactor.Payment.RegisterDate.Hour, x.FinalFactor.Payment.RegisterDate.Minute),
                        IsPaid = x.FinalFactor.Payment.IsPaid,
                        //Bank Data
                        RefId = x.FinalFactor.Payment.RefId
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

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}