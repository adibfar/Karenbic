using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Karenbic.Areas.Customer.Models;
using System.Threading.Tasks;

namespace Karenbic.Areas.Admin.Controllers
{
    public class DesignOrderPriceController : Controller
    {
        private DataAccess.Context _context;

        public DesignOrderPriceController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(int formId,
            OrderPriceValue_Numeric[] numerics,
            OrderPriceValue_Checkbox[] checkboxs,
            OrderPriceValue_DropDown[] dropDowns,
            OrderPriceValue_RadioButtonGroup[] radioButtonGroups,
            OrderPriceValue_CheckboxGroup[] checkBoxGroups,
            int priority, 
            decimal price, decimal prepayment,
            decimal specialCreativityPrice, decimal specialCreativityPrepayment)
        {
            //Get Form
            DomainClasses.Form form = _context.Forms.Find(formId);
            form.CanDelete = false;

            //Define Price
            DomainClasses.DesignOrderPrice orderPrice = new DomainClasses.DesignOrderPrice();
            orderPrice.Priority = priority;
            orderPrice.Price = price;
            orderPrice.Prepayment = prepayment;
            orderPrice.SpecialCreativityPrice = specialCreativityPrice;
            orderPrice.SpecialCreativityPrepayment = specialCreativityPrepayment;
            orderPrice.Form = form;

            //Numeric
            if (numerics != null && numerics.Length > 0)
            {
                foreach (OrderPriceValue_Numeric numeric in numerics)
                {
                    DomainClasses.FormField_Numeric field = _context.FormFileds_Numeric.Find(numeric.FieldId);
                    field.CanDelete = false;

                    if (Add_NumericValidation(field, numeric))
                    {
                        DomainClasses.OrderPriceValue_Numeric item = new DomainClasses.OrderPriceValue_Numeric();
                        item.OrderPrice = orderPrice;
                        item.Field = field;
                        item.MinValue = (float)numeric.MinValue;
                        item.MaxValue = (float)numeric.MaxValue;
                        _context.OrderPriceValues_Numeric.Add(item);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }

            //Checkbox
            if (checkboxs != null && checkboxs.Length > 0)
            {
                foreach (OrderPriceValue_Checkbox checkbox in checkboxs)
                {
                    DomainClasses.FormField_CheckBox field = _context.FormFields_CheckBox.Find(checkbox.FieldId);
                    field.CanDelete = false;

                    DomainClasses.OrderPriceValue_Checkbox item = new DomainClasses.OrderPriceValue_Checkbox();
                    item.OrderPrice = orderPrice;
                    item.Field = field;
                    item.Value = checkbox.Value;
                    _context.OrderPriceValues_Checkbox.Add(item);
                }
            }

            //DropDown
            if (dropDowns != null && dropDowns.Length > 0)
            {
                foreach (OrderPriceValue_DropDown dropDown in dropDowns)
                {
                    DomainClasses.FormField_DropDown field = _context.FormFields_DropDown.Find(dropDown.FieldId);
                    field.CanDelete = false;

                    if (Add_DropDownValidation(field, dropDown))
                    {
                        DomainClasses.OrderPriceValue_DropDown item = new DomainClasses.OrderPriceValue_DropDown();
                        item.OrderPrice = orderPrice;
                        item.Field = field;
                        if (dropDown.Value != null &&
                            _context.FormField_DropDown_Items.Any(x => x.Id == dropDown.Value))
                        {
                            DomainClasses.FormField_DropDown_Item dropDownItem =
                                _context.FormField_DropDown_Items.Find(dropDown.Value);
                            dropDownItem.CanDelete = false;

                            item.Value = dropDownItem;
                        }
                        _context.OrderPriceValues_DropDown.Add(item);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }

            //Radio Button Group
            if (radioButtonGroups != null && radioButtonGroups.Length > 0)
            {
                foreach (OrderPriceValue_RadioButtonGroup radioButtonGroup in radioButtonGroups)
                {
                    DomainClasses.FormField_RadioButtonGroup field =
                        _context.FormFields_RadioButtonGroup.Find(radioButtonGroup.FieldId);
                    field.CanDelete = false;

                    if (Add_RadioButtonGroupValidation(field, radioButtonGroup))
                    {
                        DomainClasses.OrderPriceValue_RadioButtonGroup item = new DomainClasses.OrderPriceValue_RadioButtonGroup();
                        item.OrderPrice = orderPrice;
                        item.Field = field;
                        if (radioButtonGroup.Value != null &&
                            _context.FormField_RadioButtonGroup_Items.Any(x => x.Id == radioButtonGroup.Value))
                        {
                            DomainClasses.FormField_RadioButtonGroup_Item radioButtonItem =
                                _context.FormField_RadioButtonGroup_Items.Find(radioButtonGroup.Value);
                            radioButtonItem.CanDelete = false;

                            item.Value = radioButtonItem;
                        }
                        _context.OrderPriceValues_RadioButtonGroup.Add(item);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }

            //CheckBox Group
            if (checkBoxGroups != null && checkBoxGroups.Length > 0)
            {
                foreach (OrderPriceValue_CheckboxGroup checkBoxGroup in checkBoxGroups)
                {
                    DomainClasses.FormField_CheckBoxGroup field =
                        _context.FormFields_CheckBoxGroup.Find(checkBoxGroup.FieldId);
                    field.CanDelete = false;

                    DomainClasses.OrderPriceValue_CheckboxGroup item = new DomainClasses.OrderPriceValue_CheckboxGroup();
                    item.OrderPrice = orderPrice;
                    item.Field = field;
                    if (checkBoxGroup.Values != null && checkBoxGroup.Values.Length > 0)
                    {
                        item.Values = new List<DomainClasses.FormField_CheckBoxGroup_Item>();

                        foreach (int id in checkBoxGroup.Values)
                        {
                            if (_context.FormField_CheckBoxGroup_Items.Any(x => x.Id == id))
                            {
                                DomainClasses.FormField_CheckBoxGroup_Item checkboxItem =
                                    _context.FormField_CheckBoxGroup_Items.Find(id);
                                checkboxItem.CanDelete = false;

                                item.Values.Add(checkboxItem);
                            }
                        }
                    }
                    _context.OrderPriceValues_CheckboxGroup.Add(item);
                }
            }

            //Add Price
            _context.DesignOrderPrices.Add(orderPrice);

            _context.SaveChanges();

            return Json(new
            {
                Id = orderPrice.Id
            });
        }

        [NonAction]
        private bool Add_NumericValidation(DomainClasses.FormField_Numeric field, OrderPriceValue_Numeric value)
        {
            bool result = true;

            if (field.Limits &&
                ((field.Min > value.MinValue || field.Max < value.MinValue) ||
                (field.Min > value.MaxValue || field.Max < value.MaxValue)))
            {
                result = false;
            }

            return result;
        }

        [NonAction]
        private bool Add_DropDownValidation(DomainClasses.FormField_DropDown field, OrderPriceValue_DropDown value)
        {
            bool result = true;

            if (field.IsRequired && value.Value == null)
            {
                result = false;
            }

            return result;
        }

        [NonAction]
        private bool Add_RadioButtonGroupValidation(DomainClasses.FormField_RadioButtonGroup field, OrderPriceValue_RadioButtonGroup value)
        {
            bool result = true;

            if (field.IsRequired && value.Value == null)
            {
                result = false;
            }

            return result;
        }

        [HttpGet]
        public async Task<ActionResult> Get(int formId)
        {
            List<DomainClasses.DesignOrderPrice> list = await _context.DesignOrderPrices
                .Include(x => x.Values)
                .Where(x => x.Form.Id == formId)
                .OrderByDescending(x => x.Priority)
                .ThenByDescending(x => x.RegisterDate)
                .ToListAsync();

            List<object> outputList = new List<object>();

            foreach (DomainClasses.DesignOrderPrice orderPrice in list)
            {
                List<object> values = new List<object>();

                foreach (DomainClasses.OrderPriceValue value in orderPrice.Values)
                {
                    //Numeric
                    if (value is DomainClasses.OrderPriceValue_Numeric)
                    {
                        DomainClasses.OrderPriceValue_Numeric item = (DomainClasses.OrderPriceValue_Numeric)value;
                        values.Add(new
                        {
                            Type = 2,
                            Name = item.Field.Title,
                            MinValue = item.MinValue,
                            MaxValue = item.MaxValue,
                        });
                    }

                    //Drop Down
                    if (value is DomainClasses.OrderPriceValue_DropDown)
                    {
                        DomainClasses.OrderPriceValue_DropDown item = (DomainClasses.OrderPriceValue_DropDown)value;
                        values.Add(new
                        {
                            Type = 8,
                            Name = item.Field.Title,
                            Value = item.Value.Title
                        });
                    }

                    //Multiple Choice
                    if (value is DomainClasses.OrderPriceValue_RadioButtonGroup)
                    {
                        DomainClasses.OrderPriceValue_RadioButtonGroup item = (DomainClasses.OrderPriceValue_RadioButtonGroup)value;
                        values.Add(new
                        {
                            Type = 9,
                            Name = item.Field.Title,
                            Value = item.Value.Title
                        });
                    }

                    //Checkbox Group
                    if (value is DomainClasses.OrderPriceValue_CheckboxGroup)
                    {
                        DomainClasses.OrderPriceValue_CheckboxGroup item = (DomainClasses.OrderPriceValue_CheckboxGroup)value;
                        values.Add(new
                        {
                            Type = 10,
                            Name = item.Field.Title,
                            Values = item.Values.Select(x => x.Title)
                        });
                    }
                }

                outputList.Add(new
                {
                    Id = orderPrice.Id,
                    Priority = orderPrice.Priority,
                    Price = orderPrice.Price,
                    Prepayment = orderPrice.Prepayment,
                    SpecialCreativityPrice = orderPrice.SpecialCreativityPrice,
                    SpecialCreativityPrepayment = orderPrice.SpecialCreativityPrepayment,
                    Values = values
                });
            }

            return Json(outputList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            DomainClasses.DesignOrderPrice item = _context.DesignOrderPrices
                .Include(x => x.Values)
                .Single(x => x.Id == id);

            foreach (DomainClasses.OrderPriceValue value in item.Values.ToList())
            {
                _context.OrderPriceValues.Remove(value);
            }

            _context.DesignOrderPrices.Remove(item);

            _context.SaveChanges();

            return Content("True");
        }
    }
}