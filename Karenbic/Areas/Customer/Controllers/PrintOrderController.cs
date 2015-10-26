using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Customer.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Customer")]
    public class PrintOrderController : Controller
    {
        private DataAccess.Context _context;

        public PrintOrderController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult PreOrder()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PreOrderText()
        {
            string text = string.Empty;

            text = _context.Setting.Find(1).PrePrintOrderText;

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

            IQueryable<DomainClasses.PrintOrder> query = _context.PrintOrders.AsQueryable();

            DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);
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

            int[] OrderIds = list.Select(c => c.Id).ToArray();

            List<DomainClasses.Order_Value_FileUploader> fileUploaders =
                _context.Order_Values_FileUploader
                .Include(x => x.Order)
                .Where(x => OrderIds.Contains(x.Order.Id)).ToList();

            List<DomainClasses.Order_Value_FileUploader2> extendedFileUploaders =
                _context.Order_Values_FileUploader2
                .Include(x => x.Order)
                .Where(x => OrderIds.Contains(x.Order.Id)).ToList();

            int?[] extendedFileUploaders_RelatedDesignOrderIds = extendedFileUploaders
                .Where(x => x.Type == 2 && x.DesignOrderId != null).Select(x => x.DesignOrderId).ToArray();

            List<DomainClasses.DesignOrder_FinalDesign> finalDesigns = _context.DesignOrder_FinalDesigns
                .Include(x => x.Order)
                .Where(x => extendedFileUploaders_RelatedDesignOrderIds.Contains(x.Order.Id))
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
                    //Files
                    Files = fileUploaders.Where(c => c.Order.Id == x.Id).Select(c => new
                    {
                        HasFile = c.HasFile,
                        FileName = c.FileName,
                        FilePath = c.FilePath
                    }),
                    ExtendedFiles_File = extendedFileUploaders.Where(c => c.Type == 1 && c.Order.Id == x.Id).Select(c => new
                    {
                        HasFile = c.HasFile,
                        FileName = c.FileName,
                        FilePath = c.FilePath
                    }),
                    ExtendedFiles_Design = extendedFileUploaders.Where(c => c.Type == 2 && c.Order.Id == x.Id).Select(c => new
                    {
                        Name = c.Field.Title,
                        Values = finalDesigns.Where(m => m.Order.Id == c.DesignOrderId)
                        .Select(m => new
                        {
                            Title = m.Title,
                            Link = m.Link
                        }).ToArray()
                    }),
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
                    Payment = x.Factor != null && x.Factor.Payment != null ? new
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

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}