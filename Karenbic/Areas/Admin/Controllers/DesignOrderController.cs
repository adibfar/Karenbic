using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Hosting;

namespace Karenbic.Areas.Admin.Controllers
{
    public class DesignOrderController : Controller
    {
        [HttpGet]
        public ActionResult PreOrder()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PreOrderText()
        {
            string text = string.Empty;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                text = context.Setting.Find(1).PreDesignOrderText;
            }

            return Json(text, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PreOrderText(string text)
        {
            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.Setting setting = context.Setting.Find(1);
                setting.PreDesignOrderText = text;
                context.SaveChanges();
            }

            return Json(text, JsonRequestBehavior.AllowGet);
        }

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
                query = query.Where(x => x.IsConfirm == false || (x.IsPaidPrepayment == false && x.IsPaidFinal == false));

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
                        SpecialCreativity = x.SpecialCreativity,
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
        public ActionResult OngoingOrderList()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetOngoingOrders(int? orderId, string startDate, string endDate, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                IQueryable<DomainClasses.DesignOrder> query = context.DesignOrders.AsQueryable();

                query = query.Where(x => x.IsCanceled == false);
                query = query.Where(x => (x.IsPaidPrepayment == true || x.IsPaidFinal == true) && x.IsAcceptDesign == false);

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
                    .Include(x => x.PrepaymentFactor)
                    .Include(x => x.PrepaymentFactor.Payment)
                    .Include(x => x.FinalFactor)
                    .Include(x => x.FinalFactor.Payment)
                    .OrderByDescending(x => x.LastChange)
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
                        SpecialCreativity = x.SpecialCreativity,
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
                IQueryable<DomainClasses.DesignOrder> query = context.DesignOrders.AsQueryable();

                query = query.Where(x => x.IsCanceled == false);
                query = query.Where(x => x.IsPaidPrepayment == true && x.IsPaidFinal == true && x.IsAcceptDesign == true);

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
                        Id = x.Id,
                        Code = x.Code,
                        Time = string.Format("{0:D2}:{1:D2}", x.RegisterDate.Hour, x.RegisterDate.Minute),
                        RegisterDate = x.RegisterDate.ToShortDateString(),
                        PersianRegisterDate = x.PersianRegisterDate,
                        SpecialCreativity = x.SpecialCreativity,
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
                        SpecialCreativity = x.SpecialCreativity,
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
                    order.LastChange = DateTime.Now;

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

        [HttpGet]
        public ActionResult SendOrderDesign()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendOrderDesign(int orderId, string description, HttpPostedFileBase[] file)
        {
            DomainClasses.DesignOrder_Design design = new DomainClasses.DesignOrder_Design();

            if (file != null && file.Length > 0)
            {
                using (DataAccess.Context context = new DataAccess.Context())
                {
                    DomainClasses.DesignOrder order = context.DesignOrders.Find(orderId);
                    order.LastChange = DateTime.Now;

                    design.Order = context.DesignOrders.Find(orderId);
                    design.Description = description;
                    design.Files = new List<DomainClasses.DesignOrder_Design_File>();

                    foreach (HttpPostedFileBase item in file)
                    {
                        if (item.ContentType == "image/jpg" || item.ContentType == "image/jpeg" || item.ContentType == "image/png")
                        {
                            if (item.ContentLength <= 250 * 1024)
                            {
                                DomainClasses.DesignOrder_Design_File designFile = new DomainClasses.DesignOrder_Design_File();
                                designFile.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(item.FileName));
                                item.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/DesignOrder"), designFile.PictureFile));
                                design.Files.Add(designFile);
                            }
                        }
                    }

                    context.DesignOrder_Designs.Add(design);
                    context.SaveChanges();
                } 
            }

            return Json(new
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
                    PicturePath = c.PicturePath
                })
            });
        }

        [HttpGet]
        public ActionResult SendOrderDesign_GetData(int id)
        {
            DomainClasses.DesignOrder order = new DomainClasses.DesignOrder();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                order = context.DesignOrders
                    .Include(x => x.Customer)
                    .Include(x => x.Form)
                    .Include(x => x.PrepaymentFactor)
                    .Include(x => x.PrepaymentFactor.Payment)
                    .Include(x => x.FinalFactor)
                    .Include(x => x.FinalFactor.Payment)
                    .Single(x => x.Id == id);
            }

            return Json(new 
            {
                Id = order.Id,
                Code = order.Code,
                Time = string.Format("{0:D2}:{1:D2}", order.RegisterDate.Hour, order.RegisterDate.Minute),
                RegisterDate = order.RegisterDate.ToShortDateString(),
                PersianRegisterDate = order.PersianRegisterDate,
                //Confirm Data
                IsConfirm = order.IsConfirm,
                ConfirmDate = Api.ConvertDate.JulainToPersian(Convert.ToDateTime(order.ConfirmDate)),
                Price = order.Price,
                Prepayment = order.Prepayment,
                Customer = new
                {
                    Name = order.Customer.Name,
                    Surname = order.Customer.Surname,
                    Username = order.Customer.Username,
                    Phone = order.Customer.Phone,
                    Mobile = order.Customer.Mobile,
                    Email = order.Customer.Email,
                    Address = order.Customer.Address
                },
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
        public ActionResult SendOrderDesign_GetDesigns(int id)
        {
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                List<DomainClasses.DesignOrder_Design> list = context.DesignOrder_Designs
                    .Where(x => x.Order.Id == id)
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
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}