﻿using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Admin.Controllers
{
    public class DesignOrderController : Controller
    {
        [HttpGet]
        public ActionResult NewOrderList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetNewOrders(int? orderId, string startDate, string endDate, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                IQueryable<DomainClasses.DesignOrder> query = context.DesignOrders.AsQueryable();

                query = query.Where(x => x.IsCanceled == false);

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
                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(query.Count()) / Convert.ToDouble(pageSize)));
                int resultCount = query.Count();

                List<DomainClasses.DesignOrder> list = query
                    .Include(x => x.Customer)
                    .Include(x => x.Form)
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
                        }
                    }).ToArray()
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CanceledOrderList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCanceledOrders(int? orderId, string startDate, string endDate, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                IQueryable<DomainClasses.DesignOrder> query = context.DesignOrders.AsQueryable();

                query = query.Where(x => x.IsCanceled == true);

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
                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(query.Count()) / Convert.ToDouble(pageSize)));
                int resultCount = query.Count();

                List<DomainClasses.DesignOrder> list = query
                    .Include(x => x.Customer)
                    .Include(x => x.Form)
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
                        }
                    }).ToArray()
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Confirm(int orderId, decimal price, decimal prepayment)
        {
            bool result = false;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.DesignOrder order = context.DesignOrders
                    .Include(x => x.PrepaymentFactor)
                    .Include(x => x.FinalFactor)
                    .Single(x => x.Id == orderId);

                if (order.IsCanceled == false && price > 0)
                {
                    order.IsConfirm = true;
                    order.ConfirmDate = DateTime.Now;
                    order.Price = price;
                    order.Prepayment = prepayment;

                    //Set Prepayment Factor
                    if (order.PrepaymentFactor != null)
                    {
                        order.PrepaymentFactor.Price = prepayment;
                        order.PrepaymentFactor.RegisterDate = DateTime.Now;
                    }
                    else
                    {
                        order.PrepaymentFactor = new DomainClasses.PrepaymentDesignFactor();
                        order.PrepaymentFactor.Price = prepayment;
                        order.PrepaymentFactor.RegisterDate = DateTime.Now;
                    }

                    //Set Final Factor
                    if (order.FinalFactor != null)
                    {
                        order.FinalFactor.Price = price - prepayment;
                        order.FinalFactor.RegisterDate = DateTime.Now;
                    }
                    else
                    {
                        order.FinalFactor = new DomainClasses.FinalDesignFactor();
                        order.FinalFactor.Price = price - prepayment;
                        order.FinalFactor.RegisterDate = DateTime.Now;
                    }

                    context.SaveChanges();
                    result = true;
                }
            }
            return Content(result.ToString());
        }
    }
}