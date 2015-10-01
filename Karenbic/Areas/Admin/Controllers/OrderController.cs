using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Karenbic.Areas.Admin.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Admin")]
    public class OrderController : Controller
    {
        private DataAccess.Context _context;

        public OrderController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult CancelOrder(int orderId)
        {
            bool result = false;

            DomainClasses.Order order = _context.Orders.Find(orderId);
            if (order.IsCanceled == false)
            {
                order.IsCanceled = true;
                order.CancelDate = DateTime.Now;
                _context.SaveChanges();
                result = true;
            }
            return Content(result.ToString());
        }

        [HttpGet]
        public async Task<ActionResult> Details(int orderId)
        {
            List<object> values = new List<object>();

            //TextBox
            List<DomainClasses.Order_Value_TextBox> textboxes = await _context.Order_Values_TextBox
                .Include(x => x.Field)
                .Where(x => x.Order.Id == orderId).ToListAsync();

            foreach (DomainClasses.Order_Value_TextBox item in textboxes)
            {
                values.Add(new
                {
                    Type = 0,
                    Name = item.Field.Title,
                    Value = item.Value
                });
            }

            //TextArea
            List<DomainClasses.Order_Value_TextArea> textAreas = await _context.Order_Values_TextArea
                .Include(x => x.Field)
                .Where(x => x.Order.Id == orderId).ToListAsync();

            foreach (DomainClasses.Order_Value_TextArea item in textAreas)
            {
                values.Add(new
                {
                    Type = 1,
                    Name = item.Field.Title,
                    Value = item.Value
                });
            }

            //Numeric
            List<DomainClasses.Order_Value_Numeric> numerics = await _context.Order_Values_Numeric
                .Include(x => x.Field)
                .Where(x => x.Order.Id == orderId).ToListAsync();

            foreach (DomainClasses.Order_Value_Numeric item in numerics)
            {
                values.Add(new
                {
                    Type = 2,
                    Name = item.Field.Title,
                    Value = item.Value
                });
            }

            //Color Picker
            List<DomainClasses.Order_Value_ColorPicker> colorPickers = await _context.Order_Values_ColorPicker
                .Include(x => x.Field)
                .Where(x => x.Order.Id == orderId).ToListAsync();

            foreach (DomainClasses.Order_Value_ColorPicker item in colorPickers)
            {
                values.Add(new
                {
                    Type = 3,
                    Name = item.Field.Title,
                    Value = item.Value
                });
            }

            //File Uploader
            List<DomainClasses.Order_Value_FileUploader> fileUploaders = await _context.Order_Values_FileUploader
                .Include(x => x.Field)
                .Where(x => x.Order.Id == orderId).ToListAsync();

            foreach (DomainClasses.Order_Value_FileUploader item in fileUploaders)
            {
                values.Add(new
                {
                    Type = 4,
                    Name = item.Field.Title,
                    FileName = item.FileName,
                    FilePath = item.FilePath,
                    HasFile = item.HasFile
                });
            }

            //Checkbox
            List<DomainClasses.Order_Value_Checkbox> checkboxs = await _context.Order_Values_Checkbox
                .Include(x => x.Field)
                .Where(x => x.Order.Id == orderId).ToListAsync();

            foreach (DomainClasses.Order_Value_Checkbox item in checkboxs)
            {
                values.Add(new
                {
                    Type = 5,
                    Name = item.Field.Title,
                    Value = item.Value
                });
            }

            //Web Url
            List<DomainClasses.Order_Value_WebUrl> webUrls = await _context.Order_Values_WebUrl
                .Include(x => x.Field)
                .Where(x => x.Order.Id == orderId).ToListAsync();

            foreach (DomainClasses.Order_Value_WebUrl item in webUrls)
            {
                values.Add(new
                {
                    Type = 6,
                    Name = item.Field.Title,
                    Value = item.Value
                });
            }

            //Date Picker
            List<DomainClasses.Order_Value_DatePicker> datePickers = await _context.Order_Values_DatePicker
                .Include(x => x.Field)
                .Where(x => x.Order.Id == orderId).ToListAsync();

            foreach (DomainClasses.Order_Value_DatePicker item in datePickers)
            {
                values.Add(new
                {
                    Type = 7,
                    Name = item.Field.Title,
                    Value = item.Value != null ? Api.ConvertDate.JulainToPersian(Convert.ToDateTime(item.Value)) : ""
                });
            }

            //Drop Down
            List<DomainClasses.Order_Value_DropDown> dropDowns = await _context.Order_Values_DropDown
                .Include(x => x.Field).Include(x => x.Value)
                .Where(x => x.Order.Id == orderId).ToListAsync();

            foreach (DomainClasses.Order_Value_DropDown item in dropDowns)
            {
                values.Add(new
                {
                    Type = 8,
                    Name = item.Field.Title,
                    Value = item.Value.Title
                });
            }

            //Multiple Choice
            List<DomainClasses.Order_Value_RadioButtonGroup> radioButtonGroups = await _context.Order_Values_RadioButtonGroup
                .Include(x => x.Field).Include(x => x.Value)
                .Where(x => x.Order.Id == orderId).ToListAsync();

            foreach (DomainClasses.Order_Value_RadioButtonGroup item in radioButtonGroups)
            {
                values.Add(new
                {
                    Type = 9,
                    Name = item.Field.Title,
                    Value = item.Value.Title
                });
            }

            //Checkbox Group
            List<DomainClasses.Order_Value_CheckboxGroup> checkboxGroups = await _context.Order_Values_CheckboxGroup
                .Include(x => x.Field).Include(x => x.Values)
                .Where(x => x.Order.Id == orderId).ToListAsync();

            foreach (DomainClasses.Order_Value_CheckboxGroup item in checkboxGroups)
            {
                values.Add(new
                {
                    Type = 10,
                    Name = item.Field.Title,
                    Values = item.Values.Select(x => x.Title)
                });
            }

            //Extended File Uploader
            List<DomainClasses.Order_Value_FileUploader2> extendedFileUploaders = await _context.Order_Values_FileUploader2
                .Include(x => x.Field)
                .Where(x => x.Order.Id == orderId).ToListAsync();

            foreach (DomainClasses.Order_Value_FileUploader2 item in extendedFileUploaders)
            {
                if (item.Type == 1)
                {
                    values.Add(new
                    {
                        Type = 11,
                        ValueType = item.Type,
                        Name = item.Field.Title,
                        FileName = item.FileName,
                        FilePath = item.FilePath,
                        HasFile = item.HasFile,
                    });
                }
                else if (item.Type == 2)
                {
                    values.Add(new
                    {
                        Type = 11,
                        ValueType = item.Type,
                        Name = item.Field.Title,
                        Values = _context.DesignOrder_FinalDesigns
                        .Where(x => x.Order.Id == item.DesignOrderId)
                        .Select(c => new
                        {
                            Title = c.Title,
                            Link = c.Link
                        }).ToArray()
                    });
                }
            }

            return Json(values, JsonRequestBehavior.AllowGet);
        }
    }
}