﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Customer.Controllers
{
    public class PrintPaymentController : Controller
    {
        [HttpGet]
        public ActionResult Preview()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetPreviewData(int[] factorsId)
        {
            JsonResult result = new JsonResult();

            if (factorsId != null && factorsId.Length > 0)
            {
                using (DataAccess.Context context = new DataAccess.Context())
                {
                    List<DomainClasses.PrintFactor> factors = context.PrintFactors
                        .Include(x => x.Order)
                        .Include(x => x.Order.Form)
                        .Where(x => factorsId.Contains(x.Id))
                        .ToList();

                    result.Data = new
                    {
                        now = Api.ConvertDate.JulainToLongPersian(DateTime.Now),
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

            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.PrintPayment payment = context.PrintPayments
                    .Include(x => x.Items)
                    .Single(x => x.Id == paymentId);

                int[] factorsId = payment.Items.Select(x => x.FactorId).ToArray();

                List<DomainClasses.PrintFactor> factors = context.PrintFactors
                    .Include(x => x.Order)
                    .Include(x => x.Order.Form)
                    .Where(x => factorsId.Contains(x.Id))
                    .ToList();

                if (payment.IsPaid)
                {
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
                else
                {
                    result.Data = new
                    {
                        payment = new
                        {
                            Id = payment.Id,
                            Money = payment.Money,
                            PersianRegisterDate = payment.PersianRegisterDate,
                            Time = string.Format("{0:D2}:{1:D2}", payment.RegisterDate.Hour, payment.RegisterDate.Minute),
                            IsPaid = payment.IsPaid,
                            //Bank Data
                            RefId = payment.RefId
                        },
                        factors = new {}
                    };
                }
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

        [HttpGet]
        public ActionResult GetGeteway(int[] factorsId)
        {
            string result = string.Empty;

            if (factorsId != null && factorsId.Length > 0)
            {
                using (DataAccess.Context context = new DataAccess.Context())
                {
                    //Get Factor List
                    List<DomainClasses.PrintFactor> factors = context.PrintFactors
                        .Where(x => factorsId.Contains(x.Id))
                        .ToList();

                    //Create Bank Payment
                    DomainClasses.PrintPayment payment = new DomainClasses.PrintPayment();
                    payment.Money = factors.Sum(x => x.Price);
                    payment.RegisterDate = DateTime.Now;
                    payment.Items = new List<DomainClasses.PrintPaymentItem>();
                    foreach (DomainClasses.PrintFactor factor in factors)
                    {
                        payment.Items.Add(new DomainClasses.PrintPaymentItem()
                        {
                            FactorId = factor.Id
                        });
                    }
                    context.PrintPayments.Add(payment);
                    context.SaveChanges();

                    //Get Customer Data
                    DomainClasses.Customer customer = context.Customers.Find(1);
                    //DomainClasses.Customer customer = context.Customers.Single(x => x.Username == User.Identity.Name);

                    BPService.PaymentGatewayClient bpService = new BPService.PaymentGatewayClient();
                    result = bpService.bpPayRequest(1,
                        "user",
                        "pass",
                        payment.Id,
                        (long)payment.Money,
                        string.Format("{0:D4}{1:D2}{2:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                        string.Format("{0:D2}{1:D2}{2:D2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                        "نام و نام خانوادگی:" + customer.Name + " " + customer.Surname + "    شماره موبایل :" + customer.Mobile,
                        string.Format("http://www.link.com/Customer/PrintPayment/BPCheckout?paymentId={0}",
                            payment.Id),
                        0);
                }
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
                using (DataAccess.Context context = new DataAccess.Context())
                {
                    DomainClasses.PrintPayment bankPayment = context.PrintPayments
                        .Include(x => x.Items)
                        .Single(x => x.Id == paymentId);
                    bankPayment.IsComplete = true;
                    bankPayment.RefId = RefId;
                    bankPayment.ResCode = ResCode;
                    bankPayment.SaleOrderId = saleOrderId;
                    bankPayment.SaleReferenceId = SaleReferenceId;
                    context.SaveChanges();

                    List<DomainClasses.PrintPaymentItem> items =
                        (List<DomainClasses.PrintPaymentItem>)bankPayment.Items;

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
                            foreach (DomainClasses.PrintPaymentItem item in bankPayment.Items)
                            {
                                DomainClasses.PrintFactor factor = context.PrintFactors
                                    .Include(x => x.Order)
                                    .Single(x => x.Id == item.FactorId);

                                factor.Payment = bankPayment;
                                factor.IsPaid = true;
                                factor.PaidDate = bankPayment.RegisterDate;
                                factor.Order.IsPaid = true;
                            }
                            context.SaveChanges();

                            result = bpService.bpSettleRequest(1,
                                        "user",
                                        "pass",
                                        (long)paymentId,
                                        (long)saleOrderId,
                                        (long)SaleReferenceId);
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
            }

            return Redirect("/Customer#/app/print/checkout-payment");
        }
    }
}