using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Karenbic.Areas.Customer.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Customer")]
    public class FinancialConflictController : Controller
    {
        private DataAccess.Context _context;

        public FinancialConflictController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Find(int id)
        {
            DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

            DomainClasses.FinancialConflict model = _context.FinancialConflicts
                .Include(x => x.Customer)
                .Include(x => x.Items)
                .Single(x => x.Id == id && x.Customer.Id == customer.Id);

            return Json(new
            {
                Id = model.Id,
                Description = model.Description,
                Price = model.Price,
                RegisterDate = model.RegisterDate,
                PersianRegisterDate = model.PersianRegisterDate,
                Customer = new
                {
                    Id = model.Customer.Id,
                    Name = model.Customer.Name,
                    Surname = model.Customer.Surname,
                    Username = model.Customer.Username,
                },
                Items = model.Items.Select(x => new
                {
                    Description = model.Description,
                    Price = model.Price
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Get(DomainClasses.Portal portal, string startDate, string endDate, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            IQueryable<DomainClasses.FinancialConflict> query = _context.FinancialConflicts.AsQueryable();
            query = query.Where(x => x.Portal == portal);

            DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);
            query = query.Where(x => x.Customer.Id == customer.Id);

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

            int resultCount = await query.CountAsync();
            int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(resultCount) / Convert.ToDouble(pageSize)));


            List<DomainClasses.FinancialConflict> list = await query
                .Include(x => x.Customer)
                .OrderByDescending(x => x.RegisterDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            result.Data = new
            {
                ResultCount = resultCount,
                PageCount = pageCount,
                PageIndex = pageIndex,
                List = list.Select(x => new
                {
                    Id = x.Id,
                    Description = x.Description,
                    Price = x.Price,
                    RegisterDate = x.RegisterDate,
                    PersianRegisterDate = x.PersianRegisterDate,
                    Time = string.Format("{0:D2}:{1:D2}", x.RegisterDate.Hour, x.RegisterDate.Minute),
                    //Paid Data
                    IsPaid = x.IsPaid,
                    Customer = new
                    {
                        Id = x.Customer.Id,
                        Name = x.Customer.Name,
                        Surname = x.Customer.Surname,
                        Username = x.Customer.Username,
                    },
                    Items = x.Items.Select(c => new
                    {
                        Description = c.Description,
                        Price = c.Price
                    })
                }).ToArray()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PaymentPreview()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PaymentPreview_Data(int id)
        {
            DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

            DomainClasses.FinancialConflict factor = _context.FinancialConflicts
                .Include(x => x.Items)
                .Single(x => x.Id == id && x.Customer.Id == customer.Id);

            return Json(new
            {
                now = Api.ConvertDate.JulainToLongPersian(DateTime.Now),
                factor = new
                {
                    Id = factor.Id,
                    Description = factor.Description,
                    Price = factor.Price,
                    RegisterDate = factor.RegisterDate,
                    PersianRegisterDate = Api.ConvertDate.JulainToLongPersian(factor.RegisterDate),
                    Time = string.Format("{0:D2}:{1:D2}", factor.RegisterDate.Hour, factor.RegisterDate.Minute),
                    Customer = new
                    {
                        Id = factor.Customer.Id,
                        Name = factor.Customer.Name,
                        Surname = factor.Customer.Surname,
                        Username = factor.Customer.Username,
                    },
                    Items = factor.Items.Select(c => new
                    {
                        Description = c.Description,
                        Price = c.Price
                    })
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PaymentPreview_GetGeteway(int id)
        {
            string result = string.Empty;

            //Get Customer Data
            DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

            //Get Financial Conflict Data
            DomainClasses.FinancialConflict model = _context.FinancialConflicts.Find(id);

            //Create Bank Payment
            DomainClasses.FinancialConflictPayment payment = new DomainClasses.FinancialConflictPayment();
            payment.FinancialConflict = model;
            payment.Money = model.Price;
            payment.RegisterDate = DateTime.Now;
            _context.FinancialConflictPayments.Add(payment);
            _context.SaveChanges();


            BPService.PaymentGatewayClient bpService = new BPService.PaymentGatewayClient();
            result = bpService.bpPayRequest(1,
                "user",
                "pass",
                payment.Id,
                (long)payment.Money,
                string.Format("{0:D4}{1:D2}{2:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                string.Format("{0:D2}{1:D2}{2:D2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                "نام و نام خانوادگی:" + customer.Name + " " + customer.Surname + "    شماره موبایل :" + customer.Mobile,
                string.Format("http://www.link.com/Customer/FinancialConflict/PaymentPreview_BPCheckout?paymentId={0}",
                    payment.Id),
                0);


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
        public ActionResult PaymentPreview_BPCheckout(int? paymentId,
            string RefId, string ResCode, long? saleOrderId, long? SaleReferenceId)
        {
            if (paymentId != null &&
                !string.IsNullOrEmpty(RefId) &&
                !string.IsNullOrEmpty(ResCode) &&
                saleOrderId != null &&
                SaleReferenceId != null)
            {
                DomainClasses.FinancialConflictPayment bankPayment = _context.FinancialConflictPayments
                    .Include(x => x.FinancialConflict)
                    .Single(x => x.Id == paymentId);

                bankPayment.IsComplete = true;
                bankPayment.RefId = RefId;
                bankPayment.ResCode = ResCode;
                bankPayment.SaleOrderId = saleOrderId;
                bankPayment.SaleReferenceId = SaleReferenceId;
                _context.SaveChanges();

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
                        bankPayment.FinancialConflict.IsPaid = true;
                        _context.SaveChanges();

                        result = bpService.bpSettleRequest(1,
                                    "user",
                                    "pass",
                                    (long)paymentId,
                                    (long)saleOrderId,
                                    (long)SaleReferenceId);

                        return Redirect(bankPayment.FinancialConflict.Portal == DomainClasses.Portal.Print ? "/Customer#/app/print/checkout-payment" : "/Customer#/app/design/checkout-payment");
                    }
                    else
                    {
                        return Redirect(bankPayment.FinancialConflict.Portal == DomainClasses.Portal.Print ? "/Customer#/app/print/erorr-payment/9999" : "/Customer#/app/design/erorr-payment/9999");
                    }
                }
                else
                {
                    return Redirect(bankPayment.FinancialConflict.Portal == DomainClasses.Portal.Print ? "/Customer#/app/print/erorr-payment/" + ResCode : "/Customer#/app/design/erorr-payment/" + ResCode);
                }
            }

            return Redirect("/Customer#/app/design/erorr-payment/9999");
        }

        [HttpGet]
        public ActionResult PaymentPreview_GetGeteway_FAKE(int id)
        {
            string result = string.Empty;

            //Get Customer Data
            DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

            //Get Financial Conflict Data
            DomainClasses.FinancialConflict model = _context.FinancialConflicts.Find(id);

            //Create Bank Payment
            DomainClasses.FinancialConflictPayment payment = new DomainClasses.FinancialConflictPayment();
            payment.FinancialConflict = model;
            payment.Money = model.Price;
            payment.RegisterDate = DateTime.Now;
            //FAKE DATA
            payment.IsComplete = true;
            payment.RefId = "0";
            payment.ResCode = "0";
            payment.SaleOrderId = model.Id;
            payment.SaleReferenceId = model.Id;
            payment.IsPaid = true;
            _context.FinancialConflictPayments.Add(payment);

            model.IsPaid = true;

            _context.SaveChanges();


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
    }
}