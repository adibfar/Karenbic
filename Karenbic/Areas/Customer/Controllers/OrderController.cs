﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Karenbic.Areas.Customer.Models;
using System.Web.Hosting;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Karenbic.Areas.Customer.Controllers
{
    public class OrderController : Controller
    {
        private DataAccess.Context _context;

        public OrderController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Add(int formId,
            OrderValue_TextBox[] textBoxs,
            OrderValue_TextArea[] textAreas,
            OrderValue_Numeric[] numerics,
            OrderValue_ColorPicker[] colorPickers,
            OrderValue_FileUploader[] fileUploaders,
            OrderValue_Checkbox[] checkboxs,
            OrderValue_WebUrl[] webUrls,
            OrderValue_DatePicker[] datePickers,
            OrderValue_DropDown[] dropDowns,
            OrderValue_RadioButtonGroup[] radioButtonGroups,
            OrderValue_CheckboxGroup[] checkBoxGroups,
            bool specialCreativity = false)
        {
            bool isDesignOrder = false;
            DomainClasses.DesignOrder designOrder = new DomainClasses.DesignOrder();
            DomainClasses.PrintOrder printOrder = new DomainClasses.PrintOrder();

            //Get Price 
            int priceId = await Add_GetOrderPrice(formId, numerics, checkboxs, dropDowns, radioButtonGroups, checkBoxGroups);

            //Get Form
            DomainClasses.Form form = await _context.Forms.FindAsync(formId);
            form.CanDelete = false;

            //Define Order
            if (form.Portal == DomainClasses.Portal.Design)
            {
                isDesignOrder = true;
                designOrder.Form = form;
                designOrder.SpecialCreativity = specialCreativity;
                designOrder.RegisterDate = DateTime.Now;
                designOrder.Customer = _context.Customers.Find(1);
                //designOrder.Customer = _context.Customers.Single(x => x.Username == User.Identity.Name); 

                //Check Price
                if (priceId != -1)
                {
                    //set Price
                    DomainClasses.DesignOrderPrice orderPrice = await _context.DesignOrderPrices.FindAsync(priceId);
                    designOrder.IsConfirm = true;
                    designOrder.ConfirmDate = designOrder.RegisterDate;
                    designOrder.Price = specialCreativity ? orderPrice.SpecialCreativityPrice : orderPrice.Price;
                    designOrder.Prepayment = specialCreativity ? orderPrice.SpecialCreativityPrepayment : orderPrice.Prepayment;

                    //Set Prepayment Factor
                    designOrder.PrepaymentFactor = new DomainClasses.PrepaymentDesignFactor();
                    designOrder.PrepaymentFactor.Price = designOrder.Price;
                    designOrder.PrepaymentFactor.RegisterDate = designOrder.RegisterDate;

                    //Set Final Factor
                    designOrder.FinalFactor = new DomainClasses.FinalDesignFactor();
                    designOrder.FinalFactor.Price = designOrder.Price - designOrder.Prepayment;
                    designOrder.FinalFactor.RegisterDate = designOrder.RegisterDate;
                }
            }
            else
            {
                printOrder.Form = form;
                printOrder.RegisterDate = DateTime.Now;
                printOrder.Customer = _context.Customers.Find(1);
                //printOrder.Customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

                //Check Price
                if (priceId != -1)
                {
                    //set Price
                    DomainClasses.PrintOrderPrice orderPrice = await _context.PrintOrderPrices.FindAsync(priceId);
                    printOrder.IsConfirm = true;
                    printOrder.ConfirmDate = designOrder.RegisterDate;
                    printOrder.PrintPrice = orderPrice.PrintPrice;
                    printOrder.PackingPrice = orderPrice.PackingPrice;

                    //Set Factor
                    printOrder.Factor = new DomainClasses.PrintFactor();
                    printOrder.Factor.Price = orderPrice.Price;
                    printOrder.Factor.RegisterDate = designOrder.RegisterDate; ;
                }
            }

            //TextBox
            if (textBoxs != null && textBoxs.Length > 0)
            {
                foreach (OrderValue_TextBox textBox in textBoxs)
                {
                    DomainClasses.FormField_TextBox field = _context.FormFields_TextBox.Find(textBox.FieldId);
                    field.CanDelete = false;

                    if (Add_TextBoxValidation(field, textBox))
                    {
                        DomainClasses.Order_Value_TextBox item = new DomainClasses.Order_Value_TextBox();
                        if (form.Portal == DomainClasses.Portal.Design)
                        {
                            item.Order = designOrder;
                        }
                        else
                        {
                            item.Order = printOrder;
                        }
                        item.Field = field;
                        item.Value = textBox.Value;
                        _context.Order_Values_TextBox.Add(item);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }

            //TextArea
            if (textAreas != null && textAreas.Length > 0)
            {
                foreach (OrderValue_TextArea textArea in textAreas)
                {
                    DomainClasses.FormField_TextArea field = _context.FormFields_TextArea.Find(textArea.FieldId);
                    field.CanDelete = false;

                    if (Add_TextAreaValidation(field, textArea))
                    {
                        DomainClasses.Order_Value_TextArea item = new DomainClasses.Order_Value_TextArea();
                        if (form.Portal == DomainClasses.Portal.Design)
                        {
                            item.Order = designOrder;
                        }
                        else
                        {
                            item.Order = printOrder;
                        }
                        item.Field = field;
                        item.Value = textArea.Value;
                        _context.Order_Values_TextArea.Add(item);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }

            //Numeric
            if (numerics != null && numerics.Length > 0)
            {
                foreach (OrderValue_Numeric numeric in numerics)
                {
                    DomainClasses.FormField_Numeric field = _context.FormFileds_Numeric.Find(numeric.FieldId);
                    field.CanDelete = false;

                    if (Add_NumericValidation(field, numeric))
                    {
                        DomainClasses.Order_Value_Numeric item = new DomainClasses.Order_Value_Numeric();
                        if (form.Portal == DomainClasses.Portal.Design)
                        {
                            item.Order = designOrder;
                        }
                        else
                        {
                            item.Order = printOrder;
                        }
                        item.Field = field;
                        item.Value = numeric.Value;
                        _context.Order_Values_Numeric.Add(item);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }

            //Color Picker
            if (colorPickers != null && colorPickers.Length > 0)
            {
                foreach (OrderValue_ColorPicker colorPicker in colorPickers)
                {
                    DomainClasses.FormField_ColorPicker field = _context.FormFields_ColorPicker.Find(colorPicker.FieldId);
                    field.CanDelete = false;

                    if (Add_ColorPickerValidation(field, colorPicker))
                    {
                        DomainClasses.Order_Value_ColorPicker item = new DomainClasses.Order_Value_ColorPicker();
                        if (form.Portal == DomainClasses.Portal.Design)
                        {
                            item.Order = designOrder;
                        }
                        else
                        {
                            item.Order = printOrder;
                        }
                        item.Field = field;
                        item.Value = colorPicker.Value;
                        _context.Order_Values_ColorPicker.Add(item);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }

            //File Uploader
            if (fileUploaders != null && fileUploaders.Length > 0)
            {
                foreach (OrderValue_FileUploader fileUploader in fileUploaders)
                {
                    DomainClasses.FormField_FileUploader field =
                        _context.FormFields_FileUploader.Find(fileUploader.FieldId);
                    field.CanDelete = false;

                    if (Add_FileUploaderValidation(field, fileUploader))
                    {
                        DomainClasses.Order_Value_FileUploader item = new DomainClasses.Order_Value_FileUploader();
                        if (form.Portal == DomainClasses.Portal.Design)
                        {
                            item.Order = designOrder;
                        }
                        else
                        {
                            item.Order = printOrder;
                        }
                        item.Field = field;

                        //Change File Directory
                        if (!string.IsNullOrEmpty(fileUploader.Value))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), fileUploader.Value)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), fileUploader.Value),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Order"), fileUploader.Value));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), fileUploader.Value));
                            }
                        }

                        item.FileName = fileUploader.Value;
                        _context.Order_Values_FileUploader.Add(item);
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
                foreach (OrderValue_Checkbox checkbox in checkboxs)
                {
                    DomainClasses.FormField_CheckBox field = _context.FormFields_CheckBox.Find(checkbox.FieldId);
                    field.CanDelete = false;

                    DomainClasses.Order_Value_Checkbox item = new DomainClasses.Order_Value_Checkbox();
                    if (form.Portal == DomainClasses.Portal.Design)
                    {
                        item.Order = designOrder;
                    }
                    else
                    {
                        item.Order = printOrder;
                    }
                    item.Field = field;
                    item.Value = checkbox.Value;
                    _context.Order_Values_Checkbox.Add(item);
                }
            }

            //Web Url
            if (webUrls != null && webUrls.Length > 0)
            {
                foreach (OrderValue_WebUrl webUrl in webUrls)
                {
                    DomainClasses.FormField_WebUrl field = _context.FormFields_WebUrl.Find(webUrl.FieldId);
                    field.CanDelete = false;

                    if (Add_WebUrlValidation(field, webUrl))
                    {
                        DomainClasses.Order_Value_WebUrl item = new DomainClasses.Order_Value_WebUrl();
                        if (form.Portal == DomainClasses.Portal.Design)
                        {
                            item.Order = designOrder;
                        }
                        else
                        {
                            item.Order = printOrder;
                        }
                        item.Field = field;
                        item.Value = webUrl.Value;
                        _context.Order_Values_WebUrl.Add(item);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }

            //Date Picker
            if (datePickers != null && datePickers.Length > 0)
            {
                foreach (OrderValue_DatePicker datePicker in datePickers)
                {
                    DomainClasses.FormField_DatePicker field = _context.FormFields_DatePicker.Find(datePicker.FieldId);
                    field.CanDelete = false;

                    if (Add_DatePickerValidation(field, datePicker))
                    {
                        DomainClasses.Order_Value_DatePicker item = new DomainClasses.Order_Value_DatePicker();
                        if (form.Portal == DomainClasses.Portal.Design)
                        {
                            item.Order = designOrder;
                        }
                        else
                        {
                            item.Order = printOrder;
                        }
                        item.Field = field;
                        if (!string.IsNullOrEmpty(datePicker.Value))
                        {
                            item.Value = Api.ConvertDate.PersianTOJulian(datePicker.Value);
                        }
                        _context.Order_Values_DatePicker.Add(item);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }

            //DropDown
            if (dropDowns != null && dropDowns.Length > 0)
            {
                foreach (OrderValue_DropDown dropDown in dropDowns)
                {
                    DomainClasses.FormField_DropDown field = _context.FormFields_DropDown.Find(dropDown.FieldId);
                    field.CanDelete = false;

                    if (Add_DropDownValidation(field, dropDown))
                    {
                        DomainClasses.Order_Value_DropDown item = new DomainClasses.Order_Value_DropDown();
                        if (form.Portal == DomainClasses.Portal.Design)
                        {
                            item.Order = designOrder;
                        }
                        else
                        {
                            item.Order = printOrder;
                        }
                        item.Field = field;
                        if (dropDown.Value != null &&
                            _context.FormField_DropDown_Items.Any(x => x.Id == dropDown.Value))
                        {
                            DomainClasses.FormField_DropDown_Item dropDownItem =
                                _context.FormField_DropDown_Items.Find(dropDown.Value);
                            dropDownItem.CanDelete = false;

                            item.Value = dropDownItem;
                        }
                        _context.Order_Values_DropDown.Add(item);
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
                foreach (OrderValue_RadioButtonGroup radioButtonGroup in radioButtonGroups)
                {
                    DomainClasses.FormField_RadioButtonGroup field =
                        _context.FormFields_RadioButtonGroup.Find(radioButtonGroup.FieldId);
                    field.CanDelete = false;

                    if (Add_RadioButtonGroupValidation(field, radioButtonGroup))
                    {
                        DomainClasses.Order_Value_RadioButtonGroup item = new DomainClasses.Order_Value_RadioButtonGroup();
                        if (form.Portal == DomainClasses.Portal.Design)
                        {
                            item.Order = designOrder;
                        }
                        else
                        {
                            item.Order = printOrder;
                        }
                        item.Field = field;
                        if (radioButtonGroup.Value != null &&
                            _context.FormField_RadioButtonGroup_Items.Any(x => x.Id == radioButtonGroup.Value))
                        {
                            DomainClasses.FormField_RadioButtonGroup_Item radioButtonItem =
                                _context.FormField_RadioButtonGroup_Items.Find(radioButtonGroup.Value);
                            radioButtonItem.CanDelete = false;

                            item.Value = radioButtonItem;
                        }
                        _context.Order_Values_RadioButtonGroup.Add(item);
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
                foreach (OrderValue_CheckboxGroup checkBoxGroup in checkBoxGroups)
                {
                    DomainClasses.FormField_CheckBoxGroup field =
                        _context.FormFields_CheckBoxGroup.Find(checkBoxGroup.FieldId);
                    field.CanDelete = false;

                    DomainClasses.Order_Value_CheckboxGroup item = new DomainClasses.Order_Value_CheckboxGroup();
                    if (form.Portal == DomainClasses.Portal.Design)
                    {
                        item.Order = designOrder;
                    }
                    else
                    {
                        item.Order = printOrder;
                    }
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
                    _context.Order_Values_CheckboxGroup.Add(item);
                }
            }

            //Add Order
            if (form.Portal == DomainClasses.Portal.Design)
            {
                _context.DesignOrders.Add(designOrder);
            }
            else
            {
                _context.PrintOrders.Add(printOrder);
            }

            _context.SaveChanges();

            if (isDesignOrder)
            {
                if (priceId != -1)
                {
                    return Json(new
                    {
                        Id = designOrder.Id,
                        Price = designOrder.Price,
                        Prepayment = designOrder.Prepayment,
                        PrepaymentFactor = new
                        {
                            Id = designOrder.PrepaymentFactor.Id,
                            Price = designOrder.PrepaymentFactor.Price
                        },
                        FinalFactor = new
                        {
                            Id = designOrder.FinalFactor.Id,
                            Price = designOrder.FinalFactor.Price
                        }
                    });
                }

                return Json(new
                {
                    Id = designOrder.Id
                });
            }
            else
            {
                if (priceId != -1)
                {
                    return Json(new
                    {
                        Id = printOrder.Id,
                        PrintPrice = printOrder.PrintPrice,
                        PackingPrice = printOrder.PackingPrice,
                        Price = printOrder.Price,
                        Factor = new
                        {
                            Id = printOrder.Factor.Id,
                            Price = printOrder.Factor.Price
                        }
                    });
                }

                return Json(new
                {
                    Id = printOrder.Id
                });
            }
        }

        [HttpPost]
        public ActionResult Add_UploadFile(HttpPostedFileBase file, int fieldId)
        {
            string fileName = "";
            string fileExtension = "";
            int fileSize = 0;

            if (file != null)
            {
                fileExtension = System.IO.Path.GetExtension(file.FileName).Substring(1);
                fileSize = file.ContentLength;

                DomainClasses.FormField_FileUploader field = _context.FormFields_FileUploader
                    .Include(x => x.Formats)
                    .Single(x => x.Id == fieldId);

                if (!field.Formats.Any(x => x.Extention.ToLower() == fileExtension.ToLower()))
                {
                    throw new Exception();
                }
                else if(field.SizeLimits == true &&
                    (fileSize < field.MinSize * 1024 || fileSize > field.MaxSize * 1024))
                {
                    throw new Exception();
                }
                else
                {
                    fileName = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(file.FileName));
                    file.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), fileName));
                }
            }
            else
            {
                throw new Exception();
            }

            return Content(fileName);
        }

        [NonAction]
        private bool Add_TextBoxValidation(DomainClasses.FormField_TextBox field, OrderValue_TextBox value)
        {
            bool result = true;

            if (field.IsRequired && string.IsNullOrEmpty(value.Value))
            {
                result = false;
            }

            return result;
        }

        [NonAction]
        private bool Add_TextAreaValidation(DomainClasses.FormField_TextArea field, OrderValue_TextArea value)
        {
            bool result = true;

            if (field.IsRequired && string.IsNullOrEmpty(value.Value))
            {
                result = false;
            }

            return result;
        }

        [NonAction]
        private bool Add_NumericValidation(DomainClasses.FormField_Numeric field, OrderValue_Numeric value)
        {
            bool result = true;

            if (field.IsRequired && value.Value == null)
            {
                result = false;
            }

            if (field.Limits && (field.Min > value.Value || field.Max < value.Value))
            {
                result = false;
            }

            return result;
        }

        [NonAction]
        private bool Add_ColorPickerValidation(DomainClasses.FormField_ColorPicker field, OrderValue_ColorPicker value)
        {
            bool result = true;

            if (field.IsRequired &&
                (string.IsNullOrEmpty(value.Value) || !Regex.IsMatch(value.Value, "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")))
            {
                result = false;
            }

            if (field.IsRequired == false &&
                !string.IsNullOrEmpty(value.Value) && 
                !Regex.IsMatch(value.Value, "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$"))
            {
                result = false;
            }

            return result;
        }

        [NonAction]
        public bool Add_FileUploaderValidation(DomainClasses.FormField_FileUploader field, OrderValue_FileUploader value)
        {
            bool result = true;

            if (field.IsRequired && 
                (string.IsNullOrEmpty(value.Value) ||
                !System.IO.File.Exists(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), value.Value))))
            {
                result = false;
            }

            return result;
        }

        [NonAction]
        private bool Add_WebUrlValidation(DomainClasses.FormField_WebUrl field, OrderValue_WebUrl value)
        {
            bool result = true;

            if (field.IsRequired &&
                (string.IsNullOrEmpty(value.Value) || 
                !Regex.IsMatch(value.Value, @"^((http|https):\/\/)?[\w-]+(\.[\w-]+)+([\w.,@?^=%&amp;:\/~+#-]*[\w@?^=%&amp;\/~+#-])?$")))
            {
                result = false;
            }

            if (field.IsRequired == false &&
                !string.IsNullOrEmpty(value.Value) &&
                !Regex.IsMatch(value.Value, @"^((http|https):\/\/)?[\w-]+(\.[\w-]+)+([\w.,@?^=%&amp;:\/~+#-]*[\w@?^=%&amp;\/~+#-])?$"))
            {
                result = false;
            }

            return result;
        }

        [NonAction]
        private bool Add_DatePickerValidation(DomainClasses.FormField_DatePicker field, OrderValue_DatePicker value)
        {
            bool result = true;
            DateTime date = Api.ConvertDate.PersianTOJulian(value.Value);

            if (field.IsRequired && string.IsNullOrEmpty(value.Value))
            {
                result = false;
            }

            if (!string.IsNullOrEmpty(value.Value) &&
                field.Limits &&
                (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddDays(-5) > date ||
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59).AddDays(5) < date))
            {
                result = false;
            }

            return result;
        }

        [NonAction]
        private bool Add_DropDownValidation(DomainClasses.FormField_DropDown field, OrderValue_DropDown value)
        {
            bool result = true;

            if (field.IsRequired && value.Value == null)
            {
                result = false;
            }

            return result;
        }

        [NonAction]
        private bool Add_RadioButtonGroupValidation(DomainClasses.FormField_RadioButtonGroup field, OrderValue_RadioButtonGroup value)
        {
            bool result = true;

            if (field.IsRequired && value.Value == null)
            {
                result = false;
            }

            return result;
        }

        [NonAction]
        public async Task<int> Add_GetOrderPrice(int formId,
            OrderValue_Numeric[] numerics,
            OrderValue_Checkbox[] checkboxs,
            OrderValue_DropDown[] dropDowns,
            OrderValue_RadioButtonGroup[] radioButtonGroups,
            OrderValue_CheckboxGroup[] checkBoxGroups)
        {
            List<DomainClasses.OrderPrice> prices = await _context.OrderPrices
                .Include(x => x.Values)
                .Include(x => x.Values.Select(c => c.Field))
                .Include(x => x.Values.Select(c => c.Field.MobilePosition))
                .Where(x => x.Form.Id == formId)
                .OrderByDescending(x => x.Priority)
                .ThenByDescending(x => x.RegisterDate)
                .ToListAsync();

            int findIndex = -1;

            foreach (DomainClasses.OrderPrice price in prices)
            {
                if (findIndex != -1) break;

                List<DomainClasses.OrderPriceValue> values = price.Values
                    .OrderByDescending(x => x.Field.MobilePosition.Row).ToList();

                int checkedItem = 0;

                foreach (DomainClasses.OrderPriceValue value in values)
                {
                    //Numeric
                    if (value is DomainClasses.OrderPriceValue_Numeric)
                    {
                        DomainClasses.OrderPriceValue_Numeric item = (DomainClasses.OrderPriceValue_Numeric)value;

                        if (!numerics.Any(x => x.FieldId == item.Field.Id && 
                            (x.Value >= item.MinValue && x.Value <= item.MaxValue)))
                            break;
                    }

                    //Drop Down
                    if (value is DomainClasses.OrderPriceValue_DropDown)
                    {
                        DomainClasses.OrderPriceValue_DropDown item = (DomainClasses.OrderPriceValue_DropDown)value;

                        if (!dropDowns.Any(x => x.FieldId == item.Field.Id && x.Value == item.Value.Id)) break;
                    }

                    //Multiple Choice
                    if (value is DomainClasses.OrderPriceValue_RadioButtonGroup)
                    {
                        DomainClasses.OrderPriceValue_RadioButtonGroup item = (DomainClasses.OrderPriceValue_RadioButtonGroup)value;
                        
                        if (!radioButtonGroups.Any(x => x.FieldId == item.Field.Id && x.Value == item.Value.Id)) break;
                    }

                    //Checkbox Group
                    if (value is DomainClasses.OrderPriceValue_CheckboxGroup)
                    {
                        DomainClasses.OrderPriceValue_CheckboxGroup item = (DomainClasses.OrderPriceValue_CheckboxGroup)value;

                        if (!checkBoxGroups.Any(x => x.FieldId == item.Field.Id)) break;
                        else
                        {
                            List<DomainClasses.FormField_CheckBoxGroup_Item> itemValues = item.Values.ToList();

                            int checkedCheckboxValue = 0;

                            foreach (DomainClasses.FormField_CheckBoxGroup_Item itemValue in itemValues)
                            {
                                if (!checkBoxGroups.Any(x => x.Values.Contains(itemValue.Id))) break;

                                checkedCheckboxValue++;
                            }

                            if (checkedCheckboxValue != itemValues.Count) break;
                        }
                    }

                    checkedItem++;
                }

                if (checkedItem == values.Count) findIndex = price.Id;
            }

            return findIndex;
        }

        [HttpGet]
        public ActionResult Details(int orderId)
        {
            List<object> values = new List<object>();

            DomainClasses.Customer customer = _context.Customers.Find(1);
            //DomainClasses.Customer customer = _context.Customers.Single(x => x.Username == User.Identity.Name);

            List<DomainClasses.Order_Value> list = _context.Order_Values
                .Include(x => x.Field)
                .Where(x => x.Order.Id == orderId && x.Order.Customer.Id == customer.Id)
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