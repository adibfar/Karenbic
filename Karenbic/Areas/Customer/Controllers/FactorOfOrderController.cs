using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Customer.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Customer")]
    public class FactorOfOrderController : Controller
    {
        private DataAccess.Context _context;

        public FactorOfOrderController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Details(int orderId)
        {
            List<object> values = new List<object>();

            DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

            if (_context.Orders.Any(x => x.Id == orderId && x.Customer.Id == customer.Id))
            {
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
                        DomainClasses.FormField_TextBox field = (DomainClasses.FormField_TextBox)item.Field;

                        if (field.ShowInFactor)
                        {
                            values.Add(new
                            {
                                Type = 0,
                                Name = item.Field.Title,
                                Value = item.Value,
                                Order = field.FactorOrder
                            });
                        }
                    }

                    //TextArea
                    if (value is DomainClasses.Order_Value_TextArea)
                    {
                        DomainClasses.Order_Value_TextArea item = (DomainClasses.Order_Value_TextArea)value;
                        DomainClasses.FormField_TextArea field = (DomainClasses.FormField_TextArea)item.Field;

                        if (field.ShowInFactor)
                        {
                            values.Add(new
                            {
                                Type = 1,
                                Name = item.Field.Title,
                                Value = item.Value,
                                Order = field.FactorOrder
                            });
                        }
                    }

                    //Numeric
                    if (value is DomainClasses.Order_Value_Numeric)
                    {
                        DomainClasses.Order_Value_Numeric item = (DomainClasses.Order_Value_Numeric)value;
                        DomainClasses.FormField_Numeric field = (DomainClasses.FormField_Numeric)item.Field;

                        if (field.ShowInFactor)
                        {
                            values.Add(new
                            {
                                Type = 2,
                                Name = item.Field.Title,
                                Value = item.Value,
                                Order = field.FactorOrder
                            });
                        }
                    }

                    //Checkbox
                    if (value is DomainClasses.Order_Value_Checkbox)
                    {
                        DomainClasses.Order_Value_Checkbox item = (DomainClasses.Order_Value_Checkbox)value;
                        DomainClasses.FormField_CheckBox field = (DomainClasses.FormField_CheckBox)item.Field;

                        if (field.ShowInFactor)
                        {
                            values.Add(new
                            {
                                Type = 5,
                                Name = item.Field.Title,
                                Value = item.Value,
                                Orde = field.FactorOrder
                            });
                        }
                    }

                    //Date Picker
                    if (value is DomainClasses.Order_Value_DatePicker)
                    {
                        DomainClasses.Order_Value_DatePicker item = (DomainClasses.Order_Value_DatePicker)value;
                        DomainClasses.FormField_DatePicker field = (DomainClasses.FormField_DatePicker)item.Field;

                        if (((DomainClasses.FormField_DatePicker)item.Field).ShowInFactor)
                        {
                            values.Add(new
                            {
                                Type = 7,
                                Name = item.Field.Title,
                                Value = item.Value != null ? Api.ConvertDate.JulainToPersian(Convert.ToDateTime(item.Value)) : "",
                                Order = field.FactorOrder
                            });
                        }
                    }

                    //Drop Down
                    if (value is DomainClasses.Order_Value_DropDown)
                    {
                        DomainClasses.Order_Value_DropDown item = (DomainClasses.Order_Value_DropDown)value;
                        DomainClasses.FormField_DropDown field = (DomainClasses.FormField_DropDown)item.Field;

                        if (((DomainClasses.FormField_DropDown)item.Field).ShowInFactor)
                        {
                            values.Add(new
                            {
                                Type = 8,
                                Name = item.Field.Title,
                                Value = item.Value.Title,
                                Order = field.FactorOrder
                            });
                        }
                    }

                    //Multiple Choice
                    if (value is DomainClasses.Order_Value_RadioButtonGroup)
                    {
                        DomainClasses.Order_Value_RadioButtonGroup item = (DomainClasses.Order_Value_RadioButtonGroup)value;
                        DomainClasses.FormField_RadioButtonGroup field = (DomainClasses.FormField_RadioButtonGroup)item.Field;

                        if (((DomainClasses.FormField_RadioButtonGroup)item.Field).ShowInFactor)
                        {
                            values.Add(new
                            {
                                Type = 9,
                                Name = item.Field.Title,
                                Value = item.Value.Title,
                                Order = field.FactorOrder
                            });
                        }
                    }

                    //Checkbox Group
                    if (value is DomainClasses.Order_Value_CheckboxGroup)
                    {
                        DomainClasses.Order_Value_CheckboxGroup item = (DomainClasses.Order_Value_CheckboxGroup)value;
                        DomainClasses.FormField_CheckBoxGroup field = (DomainClasses.FormField_CheckBoxGroup)item.Field;

                        if (((DomainClasses.FormField_CheckBoxGroup)item.Field).ShowInFactor)
                        {
                            values.Add(new
                            {
                                Type = 10,
                                Name = item.Field.Title,
                                Values = item.Values.Select(x => x.Title),
                                Order = field.FactorOrder
                            });
                        }
                    }
                }
            }

            return Json(values, JsonRequestBehavior.AllowGet);
        }
    }
}