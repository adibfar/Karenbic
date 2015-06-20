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