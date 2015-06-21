using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace Karenbic.Areas.Admin.Controllers
{
    public class PrintOrderController : Controller
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
                IQueryable<DomainClasses.PrintOrder> query = context.PrintOrders.AsQueryable();

                query = query.Where(x => x.IsCanceled == false);
                query = query.Where(x => x.OrderState == DomainClasses.PrintOrderState.Register ||
                    x.OrderState == DomainClasses.PrintOrderState.Confirm);

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

                List<DomainClasses.PrintOrder> list = query
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
        public ActionResult OngoingOrderList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetOngoingOrders(int? orderId, string startDate, 
            string endDate, int[] states, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                IQueryable<DomainClasses.PrintOrder> query = context.PrintOrders.AsQueryable();

                query = query.Where(x => x.IsCanceled == false);
                query = query.Where(x => x.OrderState == DomainClasses.PrintOrderState.Paid ||
                    x.OrderState == DomainClasses.PrintOrderState.Print ||
                    x.OrderState == DomainClasses.PrintOrderState.FinishServes);

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

        [HttpGet]
        public ActionResult FinishedOrderList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetFinishedOrders(int? orderId, string startDate, string endDate, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                IQueryable<DomainClasses.PrintOrder> query = context.PrintOrders.AsQueryable();

                query = query.Where(x => x.IsCanceled == false && x.OrderState == DomainClasses.PrintOrderState.Finish);

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
                        //Confirm Data
                        IsConfirm = x.IsConfirm,
                        ConfirmDate = Api.ConvertDate.JulainToPersian(Convert.ToDateTime(x.ConfirmDate)),
                        Price = x.Price,
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
                IQueryable<DomainClasses.PrintOrder> query = context.PrintOrders.AsQueryable();

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

                List<DomainClasses.PrintOrder> list = query
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
        public ActionResult Confirm(int orderId, decimal price)
        {
            bool result = false;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.PrintOrder order = context.PrintOrders
                    .Include(x => x.Factor)
                    .Single(x => x.Id == orderId);

                if (order.IsCanceled == false && price > 0)
                {
                    order.IsConfirm = true;
                    order.OrderState = DomainClasses.PrintOrderState.Confirm;
                    order.ConfirmDate = DateTime.Now;
                    order.Price = price;

                    if (order.Factor != null)
                    {
                        order.Factor.Price = price;
                        order.Factor.RegisterDate = DateTime.Now;
                    }
                    else
                    {
                        order.Factor = new DomainClasses.PrintFactor();
                        order.Factor.Price = price;
                        order.Factor.RegisterDate = DateTime.Now;
                        order.Factor.Order = order;
                    }

                    context.SaveChanges();
                    result = true;
                }
            }
            return Content(result.ToString());
        }

        [HttpPost]
        public ActionResult ChangeState(int orderId, int state)
        {
            bool result = false;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.PrintOrder order = context.PrintOrders.Find(orderId);

                switch (state)
                {
                    case 2:
                        order.OrderState = DomainClasses.PrintOrderState.Paid;
                        result = true;
                        break;

                    case 3:
                        order.OrderState = DomainClasses.PrintOrderState.Print;
                        result = true;
                        break;

                    case 4:
                        order.OrderState = DomainClasses.PrintOrderState.FinishServes;
                        result = true;
                        break;

                    case 5:
                        order.OrderState = DomainClasses.PrintOrderState.Finish;
                        result = true;
                        break;
                }

                context.SaveChanges();
            }

            return Content(result.ToString());
        }

        [HttpPost]
        public ActionResult ChangeOrdersState(int[] ordersId, int state)
        {
            bool result = false;
            if (state == 2 || state == 3 || state == 4 || state == 5) result = true;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                foreach (int id in ordersId)
                {
                    DomainClasses.PrintOrder order = context.PrintOrders.Find(id);

                    switch (state)
                    {
                        case 2:
                            order.OrderState = DomainClasses.PrintOrderState.Paid;
                            break;

                        case 3:
                            order.OrderState = DomainClasses.PrintOrderState.Print;
                            break;

                        case 4:
                            order.OrderState = DomainClasses.PrintOrderState.FinishServes;
                            break;

                        case 5:
                            order.OrderState = DomainClasses.PrintOrderState.Finish;
                            break;
                    }
                }

                context.SaveChanges();
            }

            return Content(result.ToString());
        }
    }
}