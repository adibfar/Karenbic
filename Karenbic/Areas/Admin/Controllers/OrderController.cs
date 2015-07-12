using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Admin.Controllers
{
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
        public ActionResult Details(int orderId)
        {
            List<object> values = new List<object>();

            List<DomainClasses.Order_Value> list = _context.Order_Values
                .Include(x => x.Field)
                .Where(x => x.Order.Id == orderId)
                .OrderBy(x => x.Field.MobilePosition.Row)
                .ToList();

            foreach (DomainClasses.Order_Value value in list)
            {
                //TextBox
                if (value is DomainClasses.Order_Value_TextBox)
                {
                    DomainClasses.Order_Value_TextBox item = (DomainClasses.Order_Value_TextBox)value;
                    values.Add(new
                    {
                        Type = 0,
                        Name = item.Field.Title,
                        Value = item.Value
                    });
                }

                //TextArea
                if (value is DomainClasses.Order_Value_TextArea)
                {
                    DomainClasses.Order_Value_TextArea item = (DomainClasses.Order_Value_TextArea)value;
                    values.Add(new
                    {
                        Type = 1,
                        Name = item.Field.Title,
                        Value = item.Value
                    });
                }

                //Numeric
                if (value is DomainClasses.Order_Value_Numeric)
                {
                    DomainClasses.Order_Value_Numeric item = (DomainClasses.Order_Value_Numeric)value;
                    values.Add(new
                    {
                        Type = 2,
                        Name = item.Field.Title,
                        Value = item.Value
                    });
                }

                //Color Picker
                if (value is DomainClasses.Order_Value_ColorPicker)
                {
                    DomainClasses.Order_Value_ColorPicker item = (DomainClasses.Order_Value_ColorPicker)value;
                    values.Add(new
                    {
                        Type = 3,
                        Name = item.Field.Title,
                        Value = item.Value
                    });
                }

                //File Uploader
                if (value is DomainClasses.Order_Value_FileUploader)
                {
                    DomainClasses.Order_Value_FileUploader item = (DomainClasses.Order_Value_FileUploader)value;
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
                if (value is DomainClasses.Order_Value_Checkbox)
                {
                    DomainClasses.Order_Value_Checkbox item = (DomainClasses.Order_Value_Checkbox)value;
                    values.Add(new
                    {
                        Type = 5,
                        Name = item.Field.Title,
                        Value = item.Value
                    });
                }

                //Web Url
                if (value is DomainClasses.Order_Value_WebUrl)
                {
                    DomainClasses.Order_Value_WebUrl item = (DomainClasses.Order_Value_WebUrl)value;
                    values.Add(new
                    {
                        Type = 6,
                        Name = item.Field.Title,
                        Value = item.Value
                    });
                }

                //Date Picker
                if (value is DomainClasses.Order_Value_DatePicker)
                {
                    DomainClasses.Order_Value_DatePicker item = (DomainClasses.Order_Value_DatePicker)value;
                    values.Add(new
                    {
                        Type = 7,
                        Name = item.Field.Title,
                        Value = item.Value != null ? Api.ConvertDate.JulainToPersian(Convert.ToDateTime(item.Value)) : ""
                    });
                }

                //Drop Down
                if (value is DomainClasses.Order_Value_DropDown)
                {
                    DomainClasses.Order_Value_DropDown item = (DomainClasses.Order_Value_DropDown)value;
                    values.Add(new
                    {
                        Type = 8,
                        Name = item.Field.Title,
                        Value = item.Value.Title
                    });
                }

                //Multiple Choice
                if (value is DomainClasses.Order_Value_RadioButtonGroup)
                {
                    DomainClasses.Order_Value_RadioButtonGroup item = (DomainClasses.Order_Value_RadioButtonGroup)value;
                    values.Add(new
                    {
                        Type = 9,
                        Name = item.Field.Title,
                        Value = item.Value.Title
                    });
                }

                //Checkbox Group
                if (value is DomainClasses.Order_Value_CheckboxGroup)
                {
                    DomainClasses.Order_Value_CheckboxGroup item = (DomainClasses.Order_Value_CheckboxGroup)value;
                    values.Add(new
                    {
                        Type = 10,
                        Name = item.Field.Title,
                        Values = item.Values.Select(x => x.Title)
                    });
                }
            }

            return Json(values, JsonRequestBehavior.AllowGet);
        }
    }
}