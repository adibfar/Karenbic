﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.SignalR;

namespace Karenbic.Areas.Customer.Controllers
{
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
        public ActionResult Get(string startDate, string endDate, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            IQueryable<DomainClasses.DesignPayment> query = _context.DesignPayments.AsQueryable();
            query = query.Where(x => x.IsComplete && x.IsPaid);

            DomainClasses.Customer customer = _context.Customers.Find(1);
            //DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);
            query = query.Where(x => x.PrepaymentFactors.FirstOrDefault().Order.Customer.Id == customer.Id ||
                x.FinalFactors.FirstOrDefault().Order.Customer.Id == customer.Id);

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
                    RefId = x.RefId
                }).ToArray()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Preview()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetPreviewData(int[] prepaymentFactorsId, int[] finalFactorsId)
        {
            JsonResult result = new JsonResult();

            if ((prepaymentFactorsId != null && prepaymentFactorsId.Length > 0) ||
                (finalFactorsId != null && finalFactorsId.Length > 0))
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
                    now = Api.ConvertDate.JulainToLongPersian(DateTime.Now),
                    factors = factors
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Checkout()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCheckoutData(int paymentId)
        {
            JsonResult result = new JsonResult();

            DomainClasses.DesignPayment payment = _context.DesignPayments
                .Include(x => x.PrepaymentItems)
                .Include(x => x.FinalItems)
                .Single(x => x.Id == paymentId);

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

        [HttpGet]
        public ActionResult GetGeteway(int[] prepaymentFactorsId, int[] finalFactorsId)
        {
            string result = string.Empty;

            if ((prepaymentFactorsId != null && prepaymentFactorsId.Length > 0) ||
                (finalFactorsId != null && finalFactorsId.Length > 0))
            {
                //Get Factor List
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

                //Create Bank Payment
                DomainClasses.DesignPayment payment = new DomainClasses.DesignPayment();
                payment.Money = prepaymentFactors.Sum(x => x.Price) + finalFactors.Sum(x => x.Price);
                payment.RegisterDate = DateTime.Now;
                if (prepaymentFactors.Count > 0)
                {
                    payment.PrepaymentItems = new List<DomainClasses.PrepaymentDesignPaymentItem>();
                    foreach (DomainClasses.PrepaymentDesignFactor factor in prepaymentFactors)
                    {
                        payment.PrepaymentItems.Add(new DomainClasses.PrepaymentDesignPaymentItem()
                        {
                            FactorId = factor.Id
                        });
                    }
                }
                if (finalFactors.Count > 0)
                {
                    payment.FinalItems = new List<DomainClasses.FinalDesignPaymentItem>();
                    foreach (DomainClasses.FinalDesignFactor factor in finalFactors)
                    {
                        payment.FinalItems.Add(new DomainClasses.FinalDesignPaymentItem()
                        {
                            FactorId = factor.Id
                        });
                    }
                }
                _context.DesignPayments.Add(payment);
                _context.SaveChanges();

                //Get Customer Data
                DomainClasses.Customer customer = _context.Customers.Find(1);
                //DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

                BPService.PaymentGatewayClient bpService = new BPService.PaymentGatewayClient();
                result = bpService.bpPayRequest(1,
                    "user",
                    "pass",
                    payment.Id,
                    (long)payment.Money,
                    string.Format("{0:D4}{1:D2}{2:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    string.Format("{0:D2}{1:D2}{2:D2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                    "نام و نام خانوادگی:" + customer.Name + " " + customer.Surname + "    شماره موبایل :" + customer.Mobile,
                    string.Format("http://www.link.com/Customer/DesignPayment/BPCheckout?paymentId={0}",
                        payment.Id),
                    0);
            }


            if (!string.IsNullOrEmpty(result))
            {
                String[] resultArray = result.Split(',');
                return Json(new
                {
                    ResCode = resultArray[0],
                    RefId = resultArray.Length > 1 ? resultArray[1] : "0"
                }, JsonRequestBehavior.AllowGet);
            }
            return Content(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult BPCheckout(int? paymentId,
            string RefId, string ResCode, long? saleOrderId, long? SaleReferenceId)
        {
            if (paymentId != null &&
                !string.IsNullOrEmpty(RefId) &&
                !string.IsNullOrEmpty(ResCode) &&
                saleOrderId != null &&
                SaleReferenceId != null)
            {
                DomainClasses.DesignPayment bankPayment = _context.DesignPayments
                    .Include(x => x.PrepaymentItems)
                    .Include(x => x.FinalItems)
                    .Single(x => x.Id == paymentId);
                bankPayment.IsComplete = true;
                bankPayment.RefId = RefId;
                bankPayment.ResCode = ResCode;
                bankPayment.SaleOrderId = saleOrderId;
                bankPayment.SaleReferenceId = SaleReferenceId;
                _context.SaveChanges();

                List<DomainClasses.PrepaymentDesignPaymentItem> prepaymentItems =
                    (List<DomainClasses.PrepaymentDesignPaymentItem>)bankPayment.PrepaymentItems;

                List<DomainClasses.FinalDesignPaymentItem> finalItems =
                    (List<DomainClasses.FinalDesignPaymentItem>)bankPayment.FinalItems;

                if (ResCode == "0")
                {
                    BPService.PaymentGatewayClient bpService = new BPService.PaymentGatewayClient();
                    string result = bpService.bpVerifyRequest(1,
                        "user",
                        "pass",
                        (long)paymentId,
                        (long)saleOrderId,
                        (long)SaleReferenceId);

                    if (result == "0" || result == "43")
                    {
                        bankPayment.IsPaid = true;
                        foreach (DomainClasses.PrepaymentDesignPaymentItem item in bankPayment.PrepaymentItems)
                        {
                            DomainClasses.PrepaymentDesignFactor factor = _context.PrepaymentDesignFactors
                                .Include(x => x.Order)
                                .Single(x => x.Id == item.FactorId);

                            factor.Payment = bankPayment;
                            factor.IsPaid = true;
                            factor.PaidDate = bankPayment.RegisterDate;

                            DomainClasses.DesignOrder order = _context.DesignOrders.Find(factor.Order.Id);
                            order.IsPaidPrepayment = true;
                            order.AdminMustSeeIt = true;
                        }
                        foreach (DomainClasses.FinalDesignPaymentItem item in bankPayment.FinalItems)
                        {
                            DomainClasses.FinalDesignFactor factor = _context.FinalDesignFactors
                                .Include(x => x.Order)
                                .Single(x => x.Id == item.FactorId);

                            factor.Payment = bankPayment;
                            factor.IsPaid = true;
                            factor.PaidDate = bankPayment.RegisterDate;

                            DomainClasses.DesignOrder order = _context.DesignOrders.Find(factor.Order.Id);
                            order.IsPaidFinal = true;
                            order.AdminMustSeeIt = true;
                        }
                        _context.SaveChanges();

                        result = bpService.bpSettleRequest(1,
                                    "user",
                                    "pass",
                                    (long)paymentId,
                                    (long)saleOrderId,
                                    (long)SaleReferenceId);

                        //send notification to the admin 
                        //var notificationHub = GlobalHost.ConnectionManager.GetHubContext<Hubs.AdminNotification>();
                        //notificationHub.Clients.All.newUnCheckedOngoingDesignOrders();
                    }
                    else
                    {
                        return Redirect("/Customer#/app/print/erorr-payment/9999");
                    }
                }
                else
                {
                    return Redirect("/Customer#/app/print/erorr-payment/" + ResCode);
                }
            }

            return Redirect("/Customer#/app/print/checkout-payment");
        }

        [HttpGet]
        public ActionResult GetGeteway_FAKE(int[] prepaymentFactorsId, int[] finalFactorsId)
        {
            string result = string.Empty;

            if ((prepaymentFactorsId != null && prepaymentFactorsId.Length > 0) ||
                (finalFactorsId != null && finalFactorsId.Length > 0))
            {
                //Get Factor List
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

                //Create Bank Payment
                DomainClasses.DesignPayment payment = new DomainClasses.DesignPayment();
                payment.Money = prepaymentFactors.Sum(x => x.Price) + finalFactors.Sum(x => x.Price);
                payment.RegisterDate = DateTime.Now;
                payment.IsComplete = true;
                payment.IsPaid = true;
                payment.RefId = "test";
                payment.ResCode = "test";
                payment.SaleOrderId = 0;
                payment.SaleReferenceId = 0;
                if (prepaymentFactors.Count > 0)
                {
                    payment.PrepaymentItems = new List<DomainClasses.PrepaymentDesignPaymentItem>();
                    foreach (DomainClasses.PrepaymentDesignFactor factor in prepaymentFactors)
                    {
                        factor.Payment = payment;
                        factor.IsPaid = true;
                        factor.PaidDate = payment.RegisterDate;
                        payment.PrepaymentItems.Add(new DomainClasses.PrepaymentDesignPaymentItem()
                        {
                            FactorId = factor.Id
                        });

                        DomainClasses.DesignOrder order = _context.DesignOrders.Find(factor.Order.Id);
                        order.IsPaidPrepayment = true;
                        order.AdminMustSeeIt = true;
                    }
                }
                if (finalFactors.Count > 0)
                {
                    payment.FinalItems = new List<DomainClasses.FinalDesignPaymentItem>();
                    foreach (DomainClasses.FinalDesignFactor factor in finalFactors)
                    {
                        factor.Payment = payment;
                        factor.IsPaid = true;
                        factor.PaidDate = payment.RegisterDate;
                        payment.FinalItems.Add(new DomainClasses.FinalDesignPaymentItem()
                        {
                            FactorId = factor.Id
                        });

                        DomainClasses.DesignOrder order = _context.DesignOrders.Find(factor.Order.Id);
                        order.IsPaidFinal = true;
                        order.AdminMustSeeIt = true;
                    }
                }
                _context.DesignPayments.Add(payment);
                _context.SaveChanges();

                //Get Customer Data
                DomainClasses.Customer customer = _context.Customers.Find(1);
                //DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

                //send notification to the admin 
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<Hubs.AdminNotification>();
                notificationHub.Clients.All.newUnCheckedOngoingDesignOrders();
            }


            if (!string.IsNullOrEmpty(result))
            {
                String[] resultArray = result.Split(',');
                return Json(new
                {
                    ResCode = "21",
                    RefId = "0"
                }, JsonRequestBehavior.AllowGet);
            }
            return Content(result);
        }
    }
}