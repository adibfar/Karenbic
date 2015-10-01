using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using Microsoft.AspNet.SignalR;

namespace Karenbic.Areas.Customer.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Customer")]
    public class DesignOrderController : Controller
    {
        private DataAccess.Context _context;

        public DesignOrderController(DataAccess.Context context)
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

            text = _context.Setting.Find(1).PreDesignOrderText;

            return Json(text, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Get(int? orderId, string startDate, string endDate, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            IQueryable<DomainClasses.DesignOrder> query = _context.DesignOrders
                .Where(x => x.IsCanceled == false && x.IsPaidPrepayment)
                .AsQueryable();

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

            int resultCount = query.Count();
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(resultCount) / Convert.ToDouble(pageSize)));


            List<DomainClasses.DesignOrder> list = query
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
                List = list.Select(x => new
                {
                    //Order
                    Id = x.Id,
                    Code = x.Code,
                    Time = string.Format("{0:D2}:{1:D2}", x.RegisterDate.Hour, x.RegisterDate.Minute),
                    PersianRegisterDate = x.PersianRegisterDate,
                    IsAcceptDesign = x.IsAcceptDesign,
                    IsSendFinalDesign = x.IsSendFinalDesign,
                    CustomerMustSeeIt = x.CustomerMustSeeIt,
                    //Confirm Order
                    IsConfirm = x.IsConfirm,
                    ConfirmDate = Api.ConvertDate.JulainToPersian(Convert.ToDateTime(x.ConfirmDate)),
                    Price = x.Price,
                    IsPaidPrepayment = x.IsPaidPrepayment,
                    IsPaidFinal = x.IsPaidFinal,
                    //Form
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

        [HttpGet]
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Show_GetData(int id)
        {
            DomainClasses.DesignOrder order = new DomainClasses.DesignOrder();

            DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

            order = _context.DesignOrders
                .Where(x => x.Customer.Id == customer.Id)
                .Include(x => x.Form)
                .Include(x => x.PrepaymentFactor)
                .Include(x => x.PrepaymentFactor.Payment)
                .Include(x => x.FinalFactor)
                .Include(x => x.FinalFactor.Payment)
                .Single(x => x.Id == id);

            return Json(new
            {
                Id = order.Id,
                Code = order.Code,
                Time = string.Format("{0:D2}:{1:D2}", order.RegisterDate.Hour, order.RegisterDate.Minute),
                RegisterDate = order.RegisterDate.ToShortDateString(),
                PersianRegisterDate = order.PersianRegisterDate,
                IsAcceptDesign = order.IsAcceptDesign,
                IsSendFinalDesign = order.IsSendFinalDesign,
                //Confirm Order
                IsConfirm = order.IsConfirm,
                ConfirmDate = Api.ConvertDate.JulainToPersian(Convert.ToDateTime(order.ConfirmDate)),
                Price = order.Price,
                Prepayment = order.Prepayment,
                IsPaidPrepayment = order.IsPaidPrepayment,
                IsPaidFinal = order.IsPaidFinal,
                Form = new
                {
                    Id = order.Form.Id,
                    Title = order.Form.Title
                },
                //Prepayment Factor
                PrepaymentFactor = new
                {
                    Id = order.PrepaymentFactor.Id,
                    Time = string.Format("{0:D2}:{1:D2}", order.PrepaymentFactor.RegisterDate.Hour, order.PrepaymentFactor.RegisterDate.Minute),
                    PersianRegisterDate = order.PrepaymentFactor.PersianRegisterDate,
                    Price = order.PrepaymentFactor.Price,
                    IsPaid = order.PrepaymentFactor.IsPaid
                },
                //Prepayment Payment
                PrepaymentPayment = order.PrepaymentFactor.Payment != null ? new
                {
                    Id = order.PrepaymentFactor.Payment.Id,
                    Code = order.PrepaymentFactor.Payment.Code,
                    Money = order.PrepaymentFactor.Payment.Money,
                    PersianRegisterDate = order.PrepaymentFactor.Payment.PersianRegisterDate,
                    Time = string.Format("{0:D2}:{1:D2}", order.PrepaymentFactor.Payment.RegisterDate.Hour, order.PrepaymentFactor.Payment.RegisterDate.Minute),
                    IsPaid = order.PrepaymentFactor.Payment.IsPaid,
                    //Bank Data
                    RefId = order.PrepaymentFactor.Payment.RefId
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
                    Id = order.FinalFactor.Id,
                    Time = string.Format("{0:D2}:{1:D2}", order.FinalFactor.RegisterDate.Hour, order.FinalFactor.RegisterDate.Minute),
                    PersianRegisterDate = order.FinalFactor.PersianRegisterDate,
                    Price = order.FinalFactor.Price,
                    IsPaid = order.FinalFactor.IsPaid
                },
                //Final Payment
                FinalPayment = order.FinalFactor.Payment != null ? new
                {
                    Id = order.FinalFactor.Payment.Id,
                    Code = order.FinalFactor.Payment.Code,
                    Money = order.FinalFactor.Payment.Money,
                    PersianRegisterDate = order.FinalFactor.Payment.PersianRegisterDate,
                    Time = string.Format("{0:D2}:{1:D2}", order.FinalFactor.Payment.RegisterDate.Hour, order.FinalFactor.Payment.RegisterDate.Minute),
                    IsPaid = order.FinalFactor.Payment.IsPaid,
                    //Bank Data
                    RefId = order.FinalFactor.Payment.RefId
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
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Show_GetDesigns(int id)
        {
            JsonResult result = new JsonResult();

            DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

            List<DomainClasses.DesignOrder_Design> list = _context.DesignOrder_Designs
                .Where(x => x.Order.Id == id && x.Order.Customer.Id == customer.Id)
                .Include(x => x.Files)
                .OrderByDescending(x => x.RegisterDate)
                .ToList();

            result.Data = list.Select(design => new
            {
                Id = design.Id,
                Description = design.Description,
                PersianRegisterDate = Api.ConvertDate.JulainToPersian(design.RegisterDate),
                Time = string.Format("{0:D2}:{1:D2}", design.RegisterDate.Hour, design.RegisterDate.Minute),
                IsReview = design.IsReview,
                Files = design.Files.Select(c => new
                {
                    Id = c.Id,
                    PictureFile = c.PictureFile,
                    PicturePath = c.PicturePath,
                    State = c.State,
                    CustomerDescription = c.CustomerDescription
                })
            }).ToArray();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Show_SendReview(int designId, DomainClasses.DesignOrder_Design_File[] files)
        {
            DomainClasses.DesignOrder_Design design = _context.DesignOrder_Designs
                .Include(x => x.Order)
                .Single(x => x.Id == designId);

            DomainClasses.DesignOrder order = _context.DesignOrders.Find(design.Order.Id);
            order.LastChange = DateTime.Now;

            if (files.Any(x => x.State == DomainClasses.DesignOrder_Design_File_State.Accept) &&
                (order.IsPaidPrepayment == false || order.IsPaidFinal == false))
            {
                foreach (DomainClasses.DesignOrder_Design_File file in files)
                {
                    DomainClasses.DesignOrder_Design_File item = _context.DesignOrder_Design_Files.Find(file.Id);
                    item.TempState = file.State;
                    item.CustomerDescription = file.CustomerDescription;
                }
            }
            else if (files.Any(x => x.State == DomainClasses.DesignOrder_Design_File_State.Accept))
            {
                design.Order.CustomerMustSeeIt = false;
                design.Order.AdminMustSeeIt = true;
                design.IsReview = true;
                order.IsAcceptDesign = true;

                foreach (DomainClasses.DesignOrder_Design_File file in files)
                {
                    DomainClasses.DesignOrder_Design_File item = _context.DesignOrder_Design_Files.Find(file.Id);
                    item.State = file.State;
                    item.CustomerDescription = file.CustomerDescription;
                }

                //send notification
                var customerNotification = GlobalHost.ConnectionManager.GetHubContext<Hubs.CustomerNotification>();
                var adminNotification = GlobalHost.ConnectionManager.GetHubContext<Hubs.AdminNotification>();

                customerNotification.Clients
                    .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == User.Identity.Name)
                    .Value.ConnectionIds.ToArray<string>()).minusUnreviewedDesign();

                adminNotification.Clients.All.newUnCheckedDesignOrders();
                adminNotification.Clients.All.newUnSendedFinalDesignOfDesignOrders();
            }
            else
            {
                design.Order.CustomerMustSeeIt = false;
                design.Order.AdminMustSeeIt = true;
                design.IsReview = true;

                foreach (DomainClasses.DesignOrder_Design_File file in files)
                {
                    DomainClasses.DesignOrder_Design_File item = _context.DesignOrder_Design_Files.Find(file.Id);
                    item.State = file.State;
                    item.CustomerDescription = file.CustomerDescription;
                }

                //send notification
                var customerNotification = GlobalHost.ConnectionManager.GetHubContext<Hubs.CustomerNotification>();
                var adminNotification = GlobalHost.ConnectionManager.GetHubContext<Hubs.AdminNotification>();

                customerNotification.Clients
                    .Clients(Hubs.CustomerNotification.Users.Single(x => x.Key == User.Identity.Name)
                    .Value.ConnectionIds.ToArray<string>()).minusUnreviewedDesign();

                adminNotification.Clients.All.newUnCheckedDesignOrders();
                adminNotification.Clients.All.newUnCheckedOngoingDesignOrders();
            }
            _context.SaveChanges();

            return Content("True");
        }
    }
}