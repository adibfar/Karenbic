using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Hosting;

namespace Karenbic.Areas.Admin.Controllers
{
    public class FormController : Controller
    {
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(DomainClasses.Form form,
            int groupId,
            DomainClasses.FormField_TextBox[] textBoxs,
            DomainClasses.FormField_TextArea[] textAreas,
            DomainClasses.FormField_Numeric[] numerics,
            DomainClasses.FormField_ColorPicker[] colorPickers,
            DomainClasses.FormField_FileUploader[] fileUploaders,
            DomainClasses.FormField_CheckBox[] checkboxs,
            DomainClasses.FormField_WebUrl[] webUrls,
            DomainClasses.FormField_DatePicker[] datePickers,
            DomainClasses.FormField_DropDown[] dropDowns,
            DomainClasses.FormField_RadioButtonGroup[] radioButtonGroups,
            DomainClasses.FormField_CheckBoxGroup[] checkBoxGroups)
        {
            using (DataAccess.Context context = new DataAccess.Context())
            {
                form.Group = context.FormGroups.Find(groupId);

                if ((textBoxs != null && textBoxs.Length > 0) ||
                    (textAreas != null && textAreas.Length > 0) ||
                    (numerics != null && numerics.Length > 0) ||
                    (colorPickers != null && colorPickers.Length > 0) ||
                    (fileUploaders != null && fileUploaders.Length > 0) ||
                    (checkboxs != null && checkboxs.Length > 0) ||
                    (webUrls != null && webUrls.Length > 0) ||
                    (datePickers != null && datePickers.Length > 0) ||
                    (dropDowns != null && dropDowns.Length > 0) ||
                    (radioButtonGroups != null && radioButtonGroups.Length > 0) ||
                    (checkBoxGroups != null && checkBoxGroups.Length > 0))
                {
                    form.Fields = new List<DomainClasses.FormField>();
                }

                //TextBox
                if (textBoxs != null && textBoxs.Length > 0)
                {
                    foreach (DomainClasses.FormField_TextBox field in textBoxs)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        form.Fields.Add(field);
                    }
                }

                //TextArea
                if (textAreas != null && textAreas.Length > 0)
                {
                    foreach (DomainClasses.FormField_TextArea field in textAreas)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        form.Fields.Add(field);
                    }
                }

                //Numeric Stepper
                if (numerics != null && numerics.Length > 0)
                {
                    foreach (DomainClasses.FormField_Numeric field in numerics)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        form.Fields.Add(field);
                    }
                }

                //Color Picker
                if (colorPickers != null && colorPickers.Length > 0)
                {
                    foreach (DomainClasses.FormField_ColorPicker field in colorPickers)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        form.Fields.Add(field);
                    }
                }

                //File Uploader
                if (fileUploaders != null && fileUploaders.Length > 0)
                {
                    foreach (DomainClasses.FormField_FileUploader field in fileUploaders)
                    {
                        DomainClasses.FormField_FileUploader newField = new DomainClasses.FormField_FileUploader()
                        {
                            Title = field.Title,
                            ShowAdmin = true,
                            ShowCustomer = true,
                            Description = field.Description,
                            PictureHelpFile = field.PictureHelpFile,
                            IsRequired = field.IsRequired,
                            SizeLimits = field.SizeLimits,
                            MinSize = field.MinSize,
                            MaxSize = field.MaxSize,
                            DesktopPosition = field.DesktopPosition,
                            TabletPosition = field.TabletPosition,
                            MobilePosition= field.MobilePosition
                        };

                        if (field.Formats != null && field.Formats.Count > 0)
                        {
                            newField.Formats = new List<DomainClasses.FileFormat>();
                            foreach (DomainClasses.FileFormat format in field.Formats)
                            {
                                if (context.FileFormats.Any(x => x.Id == format.Id))
                                {
                                    newField.Formats.Add(context.FileFormats.Find(format.Id));
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        form.Fields.Add(newField);
                    }
                }

                //Checkbox
                if (checkboxs != null && checkboxs.Length > 0)
                {
                    foreach (DomainClasses.FormField_CheckBox field in checkboxs)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        form.Fields.Add(field);
                    }
                }

                //Web Url
                if (webUrls != null && webUrls.Length > 0)
                {
                    foreach (DomainClasses.FormField_WebUrl field in webUrls)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        form.Fields.Add(field);
                    }
                }

                //Date Pickers
                if (datePickers != null && datePickers.Length > 0)
                {
                    foreach (DomainClasses.FormField_DatePicker field in datePickers)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        form.Fields.Add(field);
                    }
                }

                //Drop Down
                if (dropDowns != null && dropDowns.Length > 0)
                {
                    foreach (DomainClasses.FormField_DropDown field in dropDowns)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        form.Fields.Add(field);
                    }
                }

                //Radio Button Group
                if (radioButtonGroups != null && radioButtonGroups.Length > 0)
                {
                    foreach (DomainClasses.FormField_RadioButtonGroup field in radioButtonGroups)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        form.Fields.Add(field);
                    }
                }

                //CheckBox Group
                if (checkBoxGroups != null && checkBoxGroups.Length > 0)
                {
                    foreach (DomainClasses.FormField_CheckBoxGroup field in checkBoxGroups)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        form.Fields.Add(field);
                    }
                }

                context.Forms.Add(form);
                context.SaveChanges();
            }

            return Json(new
            {
                Id = form.Id,
                Title = form.Title,
                SpecialCreativity = form.SpecialCreativity,
                IsShow = form.IsShow,
                Description = form.Description
            });
        }

        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(DomainClasses.Form form,
            int groupId,
            DomainClasses.FormField_TextBox[] textBoxs,
            DomainClasses.FormField_TextBox[] textBoxs_new,
            DomainClasses.FormField_TextArea[] textAreas,
            DomainClasses.FormField_TextArea[] textAreas_new,
            DomainClasses.FormField_Numeric[] numerics,
            DomainClasses.FormField_Numeric[] numerics_new,
            DomainClasses.FormField_ColorPicker[] colorPickers,
            DomainClasses.FormField_ColorPicker[] colorPickers_new,
            DomainClasses.FormField_FileUploader[] fileUploaders,
            DomainClasses.FormField_FileUploader[] fileUploaders_new,
            DomainClasses.FormField_CheckBox[] checkboxs,
            DomainClasses.FormField_CheckBox[] checkboxs_new,
            DomainClasses.FormField_WebUrl[] webUrls,
            DomainClasses.FormField_WebUrl[] webUrls_new,
            DomainClasses.FormField_DatePicker[] datePickers,
            DomainClasses.FormField_DatePicker[] datePickers_new,
            DomainClasses.FormField_DropDown[] dropDowns,
            DomainClasses.FormField_DropDown[] dropDowns_new,
            DomainClasses.FormField_RadioButtonGroup[] radioButtonGroups,
            DomainClasses.FormField_RadioButtonGroup[] radioButtonGroups_new,
            DomainClasses.FormField_CheckBoxGroup[] checkBoxGroups,
            DomainClasses.FormField_CheckBoxGroup[] checkBoxGroups_new,
            int[] removedFields)
        {
            using (DataAccess.Context context = new DataAccess.Context())
            {
                //Change Form data
                DomainClasses.Form formItem = context.Forms.Find(form.Id);
                formItem.Title = form.Title;
                formItem.Group = context.FormGroups.Find(groupId);
                formItem.Priority = form.Priority;
                formItem.Description = form.Description;
                formItem.IsShow = form.IsShow;
                formItem.SpecialCreativity = form.SpecialCreativity;

                //New TextBox
                if (textBoxs_new != null && textBoxs_new.Length > 0)
                {
                    foreach (DomainClasses.FormField_TextBox field in textBoxs_new)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        field.Form = formItem;
                        context.FormFields_TextBox.Add(field);
                    }
                }

                //Edit TextBox
                if (textBoxs != null && textBoxs.Length > 0)
                {
                    foreach (DomainClasses.FormField_TextBox field in textBoxs)
                    {
                        DomainClasses.FormField_TextBox item = context.FormFields_TextBox
                            .Include(x => x.DesktopPosition)
                            .Include(x => x.TabletPosition)
                            .Include(x => x.MobilePosition)
                            .Single(x => x.Id == field.Id);

                        item.Title = field.Title;
                        item.Description = field.Description;
                        item.ShowCustomer = field.ShowCustomer;
                        item.Defualt = field.Defualt;
                        item.IsRequired = field.IsRequired;
                        item.CharacterLimits = field.CharacterLimits;
                        item.MinCharacters = field.MinCharacters;
                        item.MaxCharacters = field.MaxCharacters;
                        item.ShowInFactor = field.ShowInFactor;
                        item.FactorOrder = field.FactorOrder;

                        item.DesktopPosition.SizeX = field.DesktopPosition.SizeX;
                        item.DesktopPosition.SizeY = field.DesktopPosition.SizeY;
                        item.DesktopPosition.Row = field.DesktopPosition.Row;
                        item.DesktopPosition.Column = field.DesktopPosition.Column;

                        item.TabletPosition.SizeX = field.TabletPosition.SizeX;
                        item.TabletPosition.SizeY = field.TabletPosition.SizeY;
                        item.TabletPosition.Row = field.TabletPosition.Row;
                        item.TabletPosition.Column = field.TabletPosition.Column;

                        item.MobilePosition.SizeY = field.MobilePosition.SizeY;
                        item.MobilePosition.Row = field.MobilePosition.Row;

                        if (!string.IsNullOrEmpty(field.PictureHelpFile) && field.PictureHelpFile != item.PictureHelpFile)
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));

                                item.PictureHelpFile = field.PictureHelpFile;
                            }
                        }
                        else if (string.IsNullOrEmpty(field.PictureHelpFile) && !string.IsNullOrEmpty(item.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile)))
                            {
                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile));

                                item.PictureHelpFile = string.Empty;
                            }
                        }
                    }
                }

                //New TextArea
                if (textAreas_new != null && textAreas_new.Length > 0)
                {
                    foreach (DomainClasses.FormField_TextArea field in textAreas_new)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        field.Form = formItem;
                        context.FormFields_TextArea.Add(field);
                    }
                }

                //Edit TextArea
                if (textAreas != null && textAreas.Length > 0)
                {
                    foreach (DomainClasses.FormField_TextArea field in textAreas)
                    {
                        DomainClasses.FormField_TextArea item = context.FormFields_TextArea
                            .Include(x => x.DesktopPosition)
                            .Include(x => x.TabletPosition)
                            .Include(x => x.MobilePosition)
                            .Single(x => x.Id == field.Id);

                        item.Title = field.Title;
                        item.Description = field.Description;
                        item.ShowCustomer = field.ShowCustomer;
                        item.IsRequired = field.IsRequired;
                        item.CharacterLimits = field.CharacterLimits;
                        item.MinCharacters = field.MinCharacters;
                        item.MaxCharacters = field.MaxCharacters;
                        item.ShowInFactor = field.ShowInFactor;
                        item.FactorOrder = field.FactorOrder;
                        item.Height = field.Height;

                        item.DesktopPosition.SizeX = field.DesktopPosition.SizeX;
                        item.DesktopPosition.SizeY = field.DesktopPosition.SizeY;
                        item.DesktopPosition.Row = field.DesktopPosition.Row;
                        item.DesktopPosition.Column = field.DesktopPosition.Column;

                        item.TabletPosition.SizeX = field.TabletPosition.SizeX;
                        item.TabletPosition.SizeY = field.TabletPosition.SizeY;
                        item.TabletPosition.Row = field.TabletPosition.Row;
                        item.TabletPosition.Column = field.TabletPosition.Column;

                        item.MobilePosition.SizeY = field.MobilePosition.SizeY;
                        item.MobilePosition.Row = field.MobilePosition.Row;

                        if (!string.IsNullOrEmpty(field.PictureHelpFile) && field.PictureHelpFile != item.PictureHelpFile)
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));

                                item.PictureHelpFile = field.PictureHelpFile;
                            }
                        }
                        else if (string.IsNullOrEmpty(field.PictureHelpFile) && !string.IsNullOrEmpty(item.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile)))
                            {
                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile));

                                item.PictureHelpFile = string.Empty;
                            }
                        }
                    }
                }

                //New Numeric Stepper
                if (numerics_new != null && numerics_new.Length > 0)
                {
                    foreach (DomainClasses.FormField_Numeric field in numerics_new)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        field.Form = formItem;
                        context.FormFileds_Numeric.Add(field);
                    }
                }

                //Edit Numberic
                if (numerics != null && numerics.Length > 0)
                {
                    foreach (DomainClasses.FormField_Numeric field in numerics)
                    {
                        DomainClasses.FormField_Numeric item = context.FormFileds_Numeric
                            .Include(x => x.DesktopPosition)
                            .Include(x => x.TabletPosition)
                            .Include(x => x.MobilePosition)
                            .Single(x => x.Id == field.Id);

                        item.Title = field.Title;
                        item.Description = field.Description;
                        item.ShowCustomer = field.ShowCustomer;
                        item.IsInt = field.IsInt;
                        item.IsFloat = !field.IsInt;
                        item.Defualt = field.Defualt;
                        item.IsRequired = field.IsRequired;
                        item.Limits = field.Limits;
                        item.Min = field.Min;
                        item.Max = field.Max;
                        item.ShowInFactor = field.ShowInFactor;
                        item.FactorOrder = field.FactorOrder;

                        item.DesktopPosition.SizeX = field.DesktopPosition.SizeX;
                        item.DesktopPosition.SizeY = field.DesktopPosition.SizeY;
                        item.DesktopPosition.Row = field.DesktopPosition.Row;
                        item.DesktopPosition.Column = field.DesktopPosition.Column;

                        item.TabletPosition.SizeX = field.TabletPosition.SizeX;
                        item.TabletPosition.SizeY = field.TabletPosition.SizeY;
                        item.TabletPosition.Row = field.TabletPosition.Row;
                        item.TabletPosition.Column = field.TabletPosition.Column;

                        item.MobilePosition.SizeY = field.MobilePosition.SizeY;
                        item.MobilePosition.Row = field.MobilePosition.Row;

                        if (!string.IsNullOrEmpty(field.PictureHelpFile) && field.PictureHelpFile != item.PictureHelpFile)
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));

                                item.PictureHelpFile = field.PictureHelpFile;
                            }
                        }
                        else if (string.IsNullOrEmpty(field.PictureHelpFile) && !string.IsNullOrEmpty(item.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile)))
                            {
                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile));

                                item.PictureHelpFile = string.Empty;
                            }
                        }
                    }
                }

                //New Color Picker
                if (colorPickers_new != null && colorPickers_new.Length > 0)
                {
                    foreach (DomainClasses.FormField_ColorPicker field in colorPickers_new)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        field.Form = formItem;
                        context.FormFields_ColorPicker.Add(field);
                    }
                }

                //Edit Color Picker
                if (colorPickers != null && colorPickers.Length > 0)
                {
                    foreach (DomainClasses.FormField_ColorPicker field in colorPickers)
                    {
                        DomainClasses.FormField_ColorPicker item = context.FormFields_ColorPicker
                            .Include(x => x.DesktopPosition)
                            .Include(x => x.TabletPosition)
                            .Include(x => x.MobilePosition)
                            .Single(x => x.Id == field.Id);

                        item.Title = field.Title;
                        item.Description = field.Description;
                        item.ShowCustomer = field.ShowCustomer;
                        item.IsRequired = field.IsRequired;

                        item.DesktopPosition.SizeX = field.DesktopPosition.SizeX;
                        item.DesktopPosition.SizeY = field.DesktopPosition.SizeY;
                        item.DesktopPosition.Row = field.DesktopPosition.Row;
                        item.DesktopPosition.Column = field.DesktopPosition.Column;

                        item.TabletPosition.SizeX = field.TabletPosition.SizeX;
                        item.TabletPosition.SizeY = field.TabletPosition.SizeY;
                        item.TabletPosition.Row = field.TabletPosition.Row;
                        item.TabletPosition.Column = field.TabletPosition.Column;

                        item.MobilePosition.SizeY = field.MobilePosition.SizeY;
                        item.MobilePosition.Row = field.MobilePosition.Row;

                        if (!string.IsNullOrEmpty(field.PictureHelpFile) && field.PictureHelpFile != item.PictureHelpFile)
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));

                                item.PictureHelpFile = field.PictureHelpFile;
                            }
                        }
                        else if (string.IsNullOrEmpty(field.PictureHelpFile) && !string.IsNullOrEmpty(item.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile)))
                            {
                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile));

                                item.PictureHelpFile = string.Empty;
                            }
                        }
                    }
                }

                //New File Uploader
                if (fileUploaders_new != null && fileUploaders_new.Length > 0)
                {
                    foreach (DomainClasses.FormField_FileUploader field in fileUploaders_new)
                    {
                        DomainClasses.FormField_FileUploader newField = new DomainClasses.FormField_FileUploader()
                        {
                            Title = field.Title,
                            ShowAdmin = true,
                            ShowCustomer = true,
                            Description = field.Description,
                            PictureHelpFile = field.PictureHelpFile,
                            IsRequired = field.IsRequired,
                            SizeLimits = field.SizeLimits,
                            MinSize = field.MinSize,
                            MaxSize = field.MaxSize,
                            DesktopPosition = field.DesktopPosition,
                            TabletPosition = field.TabletPosition,
                            MobilePosition = field.MobilePosition
                        };

                        if (field.Formats != null && field.Formats.Count > 0)
                        {
                            newField.Formats = new List<DomainClasses.FileFormat>();

                            foreach (DomainClasses.FileFormat format in field.Formats)
                            {
                                if (context.FileFormats.Any(x => x.Id == format.Id))
                                {
                                    newField.Formats.Add(context.FileFormats.Find(format.Id));
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        newField.Form = formItem;
                        context.FormFields_FileUploader.Add(newField);
                    }
                }

                //Edit File Uploader
                if (fileUploaders != null && fileUploaders.Length > 0)
                {
                    foreach (DomainClasses.FormField_FileUploader field in fileUploaders)
                    {
                        DomainClasses.FormField_FileUploader item = context.FormFields_FileUploader
                            .Include(x => x.Formats)
                            .Include(x => x.DesktopPosition)
                            .Include(x => x.TabletPosition)
                            .Include(x => x.MobilePosition)
                            .Single(x => x.Id == field.Id);

                        item.Title = field.Title;
                        item.Description = field.Description;
                        item.ShowCustomer = field.ShowCustomer;
                        item.IsRequired = field.IsRequired;
                        item.SizeLimits = field.SizeLimits;
                        item.MinSize = field.MinSize;
                        item.MaxSize = field.MaxSize;

                        item.DesktopPosition.SizeX = field.DesktopPosition.SizeX;
                        item.DesktopPosition.SizeY = field.DesktopPosition.SizeY;
                        item.DesktopPosition.Row = field.DesktopPosition.Row;
                        item.DesktopPosition.Column = field.DesktopPosition.Column;

                        item.TabletPosition.SizeX = field.TabletPosition.SizeX;
                        item.TabletPosition.SizeY = field.TabletPosition.SizeY;
                        item.TabletPosition.Row = field.TabletPosition.Row;
                        item.TabletPosition.Column = field.TabletPosition.Column;

                        item.MobilePosition.SizeY = field.MobilePosition.SizeY;
                        item.MobilePosition.Row = field.MobilePosition.Row;

                        item.Formats.Clear();
                        if (field.Formats != null && field.Formats.Count > 0)
                        {
                            foreach (DomainClasses.FileFormat format in field.Formats)
                            {
                                if (context.FileFormats.Any(x => x.Id == format.Id))
                                {
                                    item.Formats.Add(context.FileFormats.Find(format.Id));
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(field.PictureHelpFile) && field.PictureHelpFile != item.PictureHelpFile)
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));

                                item.PictureHelpFile = field.PictureHelpFile;
                            }
                        }
                        else if (string.IsNullOrEmpty(field.PictureHelpFile) && !string.IsNullOrEmpty(item.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile)))
                            {
                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile));

                                item.PictureHelpFile = string.Empty;
                            }
                        }
                    }
                }

                //New Checkbox
                if (checkboxs_new != null && checkboxs_new.Length > 0)
                {
                    foreach (DomainClasses.FormField_CheckBox field in checkboxs_new)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        field.Form = formItem;
                        context.FormFields_CheckBox.Add(field);
                    }
                }

                //Edit Checkbox
                if (checkboxs != null && checkboxs.Length > 0)
                {
                    foreach (DomainClasses.FormField_CheckBox field in checkboxs)
                    {
                        DomainClasses.FormField_CheckBox item = context.FormFields_CheckBox
                            .Include(x => x.DesktopPosition)
                            .Include(x => x.TabletPosition)
                            .Include(x => x.MobilePosition)
                            .Single(x => x.Id == field.Id);

                        item.Title = field.Title;
                        item.Description = field.Description;
                        item.ShowCustomer = field.ShowCustomer;
                        item.ShowInFactor = field.ShowInFactor;
                        item.FactorOrder = field.FactorOrder;

                        item.DesktopPosition.SizeX = field.DesktopPosition.SizeX;
                        item.DesktopPosition.SizeY = field.DesktopPosition.SizeY;
                        item.DesktopPosition.Row = field.DesktopPosition.Row;
                        item.DesktopPosition.Column = field.DesktopPosition.Column;

                        item.TabletPosition.SizeX = field.TabletPosition.SizeX;
                        item.TabletPosition.SizeY = field.TabletPosition.SizeY;
                        item.TabletPosition.Row = field.TabletPosition.Row;
                        item.TabletPosition.Column = field.TabletPosition.Column;

                        item.MobilePosition.SizeY = field.MobilePosition.SizeY;
                        item.MobilePosition.Row = field.MobilePosition.Row;

                        if (!string.IsNullOrEmpty(field.PictureHelpFile) && field.PictureHelpFile != item.PictureHelpFile)
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));

                                item.PictureHelpFile = field.PictureHelpFile;
                            }
                        }
                        else if (string.IsNullOrEmpty(field.PictureHelpFile) && !string.IsNullOrEmpty(item.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile)))
                            {
                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile));

                                item.PictureHelpFile = string.Empty;
                            }
                        }
                    }
                }

                //New Web Url
                if (webUrls_new != null && webUrls_new.Length > 0)
                {
                    foreach (DomainClasses.FormField_WebUrl field in webUrls_new)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        field.Form = formItem;
                        context.FormFields_WebUrl.Add(field);
                    }
                }

                //Edit Web Url
                if (webUrls != null && webUrls.Length > 0)
                {
                    foreach (DomainClasses.FormField_WebUrl field in webUrls)
                    {
                        DomainClasses.FormField_WebUrl item = context.FormFields_WebUrl
                            .Include(x => x.DesktopPosition)
                            .Include(x => x.TabletPosition)
                            .Include(x => x.MobilePosition)
                            .Single(x => x.Id == field.Id);

                        item.Title = field.Title;
                        item.Description = field.Description;
                        item.ShowCustomer = field.ShowCustomer;
                        item.IsRequired = field.IsRequired;

                        item.DesktopPosition.SizeX = field.DesktopPosition.SizeX;
                        item.DesktopPosition.SizeY = field.DesktopPosition.SizeY;
                        item.DesktopPosition.Row = field.DesktopPosition.Row;
                        item.DesktopPosition.Column = field.DesktopPosition.Column;

                        item.TabletPosition.SizeX = field.TabletPosition.SizeX;
                        item.TabletPosition.SizeY = field.TabletPosition.SizeY;
                        item.TabletPosition.Row = field.TabletPosition.Row;
                        item.TabletPosition.Column = field.TabletPosition.Column;

                        item.MobilePosition.SizeY = field.MobilePosition.SizeY;
                        item.MobilePosition.Row = field.MobilePosition.Row;

                        if (!string.IsNullOrEmpty(field.PictureHelpFile) && field.PictureHelpFile != item.PictureHelpFile)
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));

                                item.PictureHelpFile = field.PictureHelpFile;
                            }
                        }
                        else if (string.IsNullOrEmpty(field.PictureHelpFile) && !string.IsNullOrEmpty(item.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile)))
                            {
                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile));

                                item.PictureHelpFile = string.Empty;
                            }
                        }
                    }
                }

                //New Date Pickers
                if (datePickers_new != null && datePickers_new.Length > 0)
                {
                    foreach (DomainClasses.FormField_DatePicker field in datePickers_new)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        field.Form = formItem;
                        context.FormFields_DatePicker.Add(field);
                    }
                }

                //Edit Date Picker
                if (datePickers != null && datePickers.Length > 0)
                {
                    foreach (DomainClasses.FormField_DatePicker field in datePickers)
                    {
                        DomainClasses.FormField_DatePicker item = context.FormFields_DatePicker
                            .Include(x => x.DesktopPosition)
                            .Include(x => x.TabletPosition)
                            .Include(x => x.MobilePosition)
                            .Single(x => x.Id == field.Id);

                        item.Title = field.Title;
                        item.Description = field.Description;
                        item.ShowCustomer = field.ShowCustomer;
                        item.IsRequired = field.IsRequired;
                        item.Limits = field.Limits;
                        item.Min = field.Min;
                        item.Max = field.Max;
                        item.ShowInFactor = field.ShowInFactor;
                        item.FactorOrder = field.FactorOrder;

                        item.DesktopPosition.SizeX = field.DesktopPosition.SizeX;
                        item.DesktopPosition.SizeY = field.DesktopPosition.SizeY;
                        item.DesktopPosition.Row = field.DesktopPosition.Row;
                        item.DesktopPosition.Column = field.DesktopPosition.Column;

                        item.TabletPosition.SizeX = field.TabletPosition.SizeX;
                        item.TabletPosition.SizeY = field.TabletPosition.SizeY;
                        item.TabletPosition.Row = field.TabletPosition.Row;
                        item.TabletPosition.Column = field.TabletPosition.Column;

                        item.MobilePosition.SizeY = field.MobilePosition.SizeY;
                        item.MobilePosition.Row = field.MobilePosition.Row;

                        if (!string.IsNullOrEmpty(field.PictureHelpFile) && field.PictureHelpFile != item.PictureHelpFile)
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));

                                item.PictureHelpFile = field.PictureHelpFile;
                            }
                        }
                        else if (string.IsNullOrEmpty(field.PictureHelpFile) && !string.IsNullOrEmpty(item.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile)))
                            {
                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile));

                                item.PictureHelpFile = string.Empty;
                            }
                        }
                    }
                }

                //New Drop Down
                if (dropDowns_new != null && dropDowns_new.Length > 0)
                {
                    foreach (DomainClasses.FormField_DropDown field in dropDowns_new)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        field.Form = formItem;
                        context.FormFields_DropDown.Add(field);
                    }
                }

                //Edit Drop Down
                if (dropDowns != null && dropDowns.Length > 0)
                {
                    foreach (DomainClasses.FormField_DropDown field in dropDowns)
                    {
                        DomainClasses.FormField_DropDown item = context.FormFields_DropDown
                            .Include(x => x.Items)
                            .Include(x => x.DesktopPosition)
                            .Include(x => x.TabletPosition)
                            .Include(x => x.MobilePosition)
                            .Single(x => x.Id == field.Id);

                        item.Title = field.Title;
                        item.Description = field.Description;
                        item.ShowCustomer = field.ShowCustomer;
                        item.IsRequired = field.IsRequired;
                        item.ShowInFactor = field.ShowInFactor;
                        item.FactorOrder = field.FactorOrder;

                        item.DesktopPosition.SizeX = field.DesktopPosition.SizeX;
                        item.DesktopPosition.SizeY = field.DesktopPosition.SizeY;
                        item.DesktopPosition.Row = field.DesktopPosition.Row;
                        item.DesktopPosition.Column = field.DesktopPosition.Column;

                        item.TabletPosition.SizeX = field.TabletPosition.SizeX;
                        item.TabletPosition.SizeY = field.TabletPosition.SizeY;
                        item.TabletPosition.Row = field.TabletPosition.Row;
                        item.TabletPosition.Column = field.TabletPosition.Column;

                        item.MobilePosition.SizeY = field.MobilePosition.SizeY;
                        item.MobilePosition.Row = field.MobilePosition.Row;

                        //Remove Old Items
                        if (item.Items.ToList().Count > 0)
                        {
                            foreach (DomainClasses.FormField_DropDown_Item dropDownItem in item.Items.ToList())
                            {
                                if (field.Items != null && field.Items.Count > 0)
                                {
                                    if (!field.Items.Any(x => x.Id == dropDownItem.Id))
                                    {
                                        if (dropDownItem.CanDelete)
                                        {
                                            context.FormField_DropDown_Items.Remove(context.FormField_DropDown_Items.Find(dropDownItem.Id));
                                        }
                                        else
                                        {
                                            dropDownItem.ShowAdmin = false;
                                            dropDownItem.ShowCustomer = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if (dropDownItem.CanDelete)
                                    {
                                        context.FormField_DropDown_Items.Remove(context.FormField_DropDown_Items.Find(dropDownItem.Id));
                                    }
                                    else
                                    {
                                        dropDownItem.ShowAdmin = false;
                                        dropDownItem.ShowCustomer = false;
                                    }
                                }
                            }
                        }

                        //Add New Items & Edit Old Items
                        if (field.Items.ToList().Count > 0)
                        {
                            foreach (DomainClasses.FormField_DropDown_Item dropDownItem in field.Items.ToList())
                            {
                                if (!context.FormField_DropDown_Items.Any(x => x.Id == dropDownItem.Id))
                                {
                                    item.Items.Add(dropDownItem);
                                }
                                else
                                {
                                    DomainClasses.FormField_DropDown_Item oldDropDownItem =
                                        context.FormField_DropDown_Items.Find(dropDownItem.Id);
                                    oldDropDownItem.Title = dropDownItem.Title;
                                    oldDropDownItem.Order = dropDownItem.Order;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(field.PictureHelpFile) && field.PictureHelpFile != item.PictureHelpFile)
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));

                                item.PictureHelpFile = field.PictureHelpFile;
                            }
                        }
                        else if (string.IsNullOrEmpty(field.PictureHelpFile) && !string.IsNullOrEmpty(item.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile)))
                            {
                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile));

                                item.PictureHelpFile = string.Empty;
                            }
                        }
                    }
                }

                //New Radio Button Group
                if (radioButtonGroups_new != null && radioButtonGroups_new.Length > 0)
                {
                    foreach (DomainClasses.FormField_RadioButtonGroup field in radioButtonGroups_new)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        field.Form = formItem;
                        context.FormFields_RadioButtonGroup.Add(field);
                    }
                }

                //Edit Radio Button Group
                if (radioButtonGroups != null && radioButtonGroups.Length > 0)
                {
                    foreach (DomainClasses.FormField_RadioButtonGroup field in radioButtonGroups)
                    {
                        DomainClasses.FormField_RadioButtonGroup item = context.FormFields_RadioButtonGroup
                            .Include(x => x.Items)
                            .Include(x => x.DesktopPosition)
                            .Include(x => x.TabletPosition)
                            .Include(x => x.MobilePosition)
                            .Single(x => x.Id == field.Id);

                        item.Title = field.Title;
                        item.Description = field.Description;
                        item.ShowCustomer = field.ShowCustomer;
                        item.IsRequired = field.IsRequired;
                        item.ShowInFactor = field.ShowInFactor;
                        item.FactorOrder = field.FactorOrder;

                        item.DesktopPosition.SizeX = field.DesktopPosition.SizeX;
                        item.DesktopPosition.SizeY = field.DesktopPosition.SizeY;
                        item.DesktopPosition.Row = field.DesktopPosition.Row;
                        item.DesktopPosition.Column = field.DesktopPosition.Column;

                        item.TabletPosition.SizeX = field.TabletPosition.SizeX;
                        item.TabletPosition.SizeY = field.TabletPosition.SizeY;
                        item.TabletPosition.Row = field.TabletPosition.Row;
                        item.TabletPosition.Column = field.TabletPosition.Column;

                        item.MobilePosition.SizeY = field.MobilePosition.SizeY;
                        item.MobilePosition.Row = field.MobilePosition.Row;

                        //Remove Old Items
                        if (item.Items.ToList().Count > 0)
                        {
                            foreach (DomainClasses.FormField_RadioButtonGroup_Item radioItem in item.Items.ToList())
                            {
                                if (field.Items != null && field.Items.Count > 0)
                                {
                                    if (!field.Items.Any(x => x.Id == radioItem.Id))
                                    {
                                        if (radioItem.CanDelete)
                                        {
                                            context.FormField_RadioButtonGroup_Items.Remove(context.FormField_RadioButtonGroup_Items.Find(radioItem.Id));
                                        }
                                        else
                                        {
                                            radioItem.ShowAdmin = false;
                                            radioItem.ShowCustomer = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if (radioItem.CanDelete)
                                    {
                                        context.FormField_RadioButtonGroup_Items.Remove(context.FormField_RadioButtonGroup_Items.Find(radioItem.Id));
                                    }
                                    else
                                    {
                                        radioItem.ShowAdmin = false;
                                        radioItem.ShowCustomer = false;
                                    }
                                }
                            }
                        }

                        //Add New Items & Edit Old Items
                        if (field.Items.ToList().Count > 0)
                        {
                            foreach (DomainClasses.FormField_RadioButtonGroup_Item radioItem in field.Items.ToList())
                            {
                                if (!context.FormField_RadioButtonGroup_Items.Any(x => x.Id == radioItem.Id))
                                {
                                    item.Items.Add(radioItem);
                                }
                                else
                                {
                                    DomainClasses.FormField_RadioButtonGroup_Item oldRadioItem =
                                        context.FormField_RadioButtonGroup_Items.Find(radioItem.Id);
                                    oldRadioItem.Title = radioItem.Title;
                                    oldRadioItem.Order = radioItem.Order;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(field.PictureHelpFile) && field.PictureHelpFile != item.PictureHelpFile)
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));

                                item.PictureHelpFile = field.PictureHelpFile;
                            }
                        }
                        else if (string.IsNullOrEmpty(field.PictureHelpFile) && !string.IsNullOrEmpty(item.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile)))
                            {
                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile));

                                item.PictureHelpFile = string.Empty;
                            }
                        }
                    }
                }

                //New CheckBox Group
                if (checkBoxGroups_new != null && checkBoxGroups_new.Length > 0)
                {
                    foreach (DomainClasses.FormField_CheckBoxGroup field in checkBoxGroups_new)
                    {
                        if (!string.IsNullOrEmpty(field.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));
                            }
                            else
                            {
                                field.PictureHelpFile = string.Empty;
                            }
                        }

                        field.Form = formItem;
                        context.FormFields_CheckBoxGroup.Add(field);
                    }
                }

                //Edit CheckBox Group
                if (checkBoxGroups != null && checkBoxGroups.Length > 0)
                {
                    foreach (DomainClasses.FormField_CheckBoxGroup field in checkBoxGroups)
                    {
                        DomainClasses.FormField_CheckBoxGroup item = context.FormFields_CheckBoxGroup
                            .Include(x => x.Items)
                            .Include(x => x.DesktopPosition)
                            .Include(x => x.TabletPosition)
                            .Include(x => x.MobilePosition)
                            .Single(x => x.Id == field.Id);

                        item.Title = field.Title;
                        item.Description = field.Description;
                        item.ShowCustomer = field.ShowCustomer;
                        item.ShowInFactor = field.ShowInFactor;
                        item.FactorOrder = field.FactorOrder;

                        item.DesktopPosition.SizeX = field.DesktopPosition.SizeX;
                        item.DesktopPosition.SizeY = field.DesktopPosition.SizeY;
                        item.DesktopPosition.Row = field.DesktopPosition.Row;
                        item.DesktopPosition.Column = field.DesktopPosition.Column;

                        item.TabletPosition.SizeX = field.TabletPosition.SizeX;
                        item.TabletPosition.SizeY = field.TabletPosition.SizeY;
                        item.TabletPosition.Row = field.TabletPosition.Row;
                        item.TabletPosition.Column = field.TabletPosition.Column;

                        item.MobilePosition.SizeY = field.MobilePosition.SizeY;
                        item.MobilePosition.Row = field.MobilePosition.Row;

                        //Remove Old Items
                        if (item.Items.ToList().Count > 0)
                        {
                            foreach (DomainClasses.FormField_CheckBoxGroup_Item checkboxItem in item.Items.ToList())
                            {
                                if (field.Items != null && field.Items.Count > 0)
                                {
                                    if (!field.Items.Any(x => x.Id == checkboxItem.Id))
                                    {
                                        if (checkboxItem.CanDelete)
                                        {
                                            context.FormField_CheckBoxGroup_Items.Remove(
                                                context.FormField_CheckBoxGroup_Items.Find(checkboxItem.Id));
                                        }
                                        else
                                        {
                                            checkboxItem.ShowAdmin = false;
                                            checkboxItem.ShowCustomer = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if (checkboxItem.CanDelete)
                                    {
                                        context.FormField_CheckBoxGroup_Items.Remove(
                                            context.FormField_CheckBoxGroup_Items.Find(checkboxItem.Id));
                                    }
                                    else
                                    {
                                        checkboxItem.ShowAdmin = false;
                                        checkboxItem.ShowCustomer = false;
                                    }
                                }
                            }
                        }

                        //Add New Items & Edit Old Items
                        if (field.Items.ToList().Count > 0)
                        {
                            foreach (DomainClasses.FormField_CheckBoxGroup_Item checkboxItem in field.Items.ToList())
                            {
                                if (!context.FormField_CheckBoxGroup_Items.Any(x => x.Id == checkboxItem.Id))
                                {
                                    item.Items.Add(checkboxItem);
                                }
                                else
                                {
                                    DomainClasses.FormField_CheckBoxGroup_Item oldCheckboxItem =
                                        context.FormField_CheckBoxGroup_Items.Find(checkboxItem.Id);
                                    oldCheckboxItem.Title = checkboxItem.Title;
                                    oldCheckboxItem.Order = checkboxItem.Order;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(field.PictureHelpFile) && field.PictureHelpFile != item.PictureHelpFile)
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile)))
                            {
                                System.IO.File.Move(
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile),
                                    string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));

                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/Upload"), field.PictureHelpFile));

                                item.PictureHelpFile = field.PictureHelpFile;
                            }
                        }
                        else if (string.IsNullOrEmpty(field.PictureHelpFile) && !string.IsNullOrEmpty(item.PictureHelpFile))
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile)))
                            {
                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/FormField"), item.PictureHelpFile));

                                item.PictureHelpFile = string.Empty;
                            }
                        }
                    }
                }

                //Remove Fields
                if (removedFields != null && removedFields.Length > 0)
                {
                    foreach (int id in removedFields)
                    {
                        DomainClasses.FormField field = context.FormFields.Find(id);
                        if (field.CanDelete)
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile)))
                            {
                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("/Content/FormField"), field.PictureHelpFile));
                            }
                            context.FormFields.Remove(field);
                        }
                        else
                        {
                            field.ShowAdmin = false;
                        }
                    }
                }

                context.SaveChanges();
            }

            return Json(new
            {
                Id = form.Id,
                Title = form.Title,
                SpecialCreativity = form.SpecialCreativity,
                IsShow = form.IsShow,
                Description = form.Description
            });
        }

        [HttpGet]
        public ActionResult Find(int id)
        {
            JsonResult result = new JsonResult();
            DomainClasses.Form form = new DomainClasses.Form();
            List<object> fields = new List<object>();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                form = context.Forms
                    .Include(x => x.Group)
                    .Include(x => x.Fields)
                    .Include(x => x.Fields.Select(c => c.DesktopPosition))
                    .Include(x => x.Fields.Select(c => c.TabletPosition))
                    .Include(x => x.Fields.Select(c => c.MobilePosition))
                    .Single(x => x.Id == id);

                foreach (DomainClasses.FormField field in form.Fields)
                {
                    if (field.ShowAdmin)
                    {
                        //TextBox
                        if (field is DomainClasses.FormField_TextBox)
                        {
                            DomainClasses.FormField_TextBox item = (DomainClasses.FormField_TextBox)field;

                            fields.Add(new
                            {
                                type = 0,
                                data = new
                                {
                                    id = item.Id,
                                    title = item.Title,
                                    showCustomer = item.ShowCustomer,
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
                                    canDelete = item.CanDelete,
                                    defualt = item.Defualt,
                                    isRequired = item.IsRequired,
                                    characterLimits = item.CharacterLimits,
                                    minCharacters = item.MinCharacters,
                                    maxCharacters = item.MaxCharacters,
                                    showInFactor = item.ShowInFactor,
                                    factorOrder = item.FactorOrder
                                },
                                desktop_position = new
                                {
                                    sizeX = item.DesktopPosition.SizeX,
                                    sizeY = item.DesktopPosition.SizeY,
                                    row = item.DesktopPosition.Row,
                                    col = item.DesktopPosition.Column
                                },
                                tablet_position = new
                                {
                                    sizeX = item.TabletPosition.SizeX,
                                    sizeY = item.TabletPosition.SizeY,
                                    row = item.TabletPosition.Row,
                                    col = item.TabletPosition.Column
                                },
                                mobile_position = new
                                {
                                    sizeX = 1,
                                    sizeY = item.MobilePosition.SizeY,
                                    row = item.MobilePosition.Row,
                                    col = 0
                                },
                                factor_position = new
                                {
                                    sizeX = 1,
                                    sizeY = 1,
                                    row = item.FactorOrder,
                                    col = 0
                                }
                            });
                        }

                        //TextArea
                        else if (field is DomainClasses.FormField_TextArea)
                        {
                            DomainClasses.FormField_TextArea item = (DomainClasses.FormField_TextArea)field;

                            fields.Add(new
                            {
                                type = 1,
                                data = new
                                {
                                    id = item.Id,
                                    title = item.Title,
                                    showCustomer = item.ShowCustomer,
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
                                    canDelete = item.CanDelete,
                                    isRequired = item.IsRequired,
                                    characterLimits = item.CharacterLimits,
                                    minCharacters = item.MinCharacters,
                                    maxCharacters = item.MaxCharacters,
                                    showInFactor = item.ShowInFactor,
                                    factorOrder = item.FactorOrder,
                                    height = item.Height
                                },
                                desktop_position = new
                                {
                                    sizeX = item.DesktopPosition.SizeX,
                                    sizeY = item.DesktopPosition.SizeY,
                                    row = item.DesktopPosition.Row,
                                    col = item.DesktopPosition.Column
                                },
                                tablet_position = new
                                {
                                    sizeX = item.TabletPosition.SizeX,
                                    sizeY = item.TabletPosition.SizeY,
                                    row = item.TabletPosition.Row,
                                    col = item.TabletPosition.Column
                                },
                                mobile_position = new
                                {
                                    sizeX = 1,
                                    sizeY = item.MobilePosition.SizeY,
                                    row = item.MobilePosition.Row,
                                    col = 0
                                },
                                factor_position = new
                                {
                                    sizeX = 1,
                                    sizeY = 1,
                                    row = item.FactorOrder,
                                    col = 0
                                }
                            });
                        }

                        //Numeric
                        else if (field is DomainClasses.FormField_Numeric)
                        {
                            DomainClasses.FormField_Numeric item = (DomainClasses.FormField_Numeric)field;

                            fields.Add(new
                            {
                                type = 2,
                                data = new
                                {
                                    id = item.Id,
                                    title = item.Title,
                                    showCustomer = item.ShowCustomer,
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
                                    isInt = item.IsInt,
                                    isFloat = item.IsFloat,
                                    canDelete = item.CanDelete,
                                    isRequired = item.IsRequired,
                                    limits = item.Limits,
                                    min = item.Min,
                                    max = item.Max,
                                    showInFactor = item.ShowInFactor,
                                    factorOrder = item.FactorOrder
                                },
                                desktop_position = new
                                {
                                    sizeX = item.DesktopPosition.SizeX,
                                    sizeY = item.DesktopPosition.SizeY,
                                    row = item.DesktopPosition.Row,
                                    col = item.DesktopPosition.Column
                                },
                                tablet_position = new
                                {
                                    sizeX = item.TabletPosition.SizeX,
                                    sizeY = item.TabletPosition.SizeY,
                                    row = item.TabletPosition.Row,
                                    col = item.TabletPosition.Column
                                },
                                mobile_position = new
                                {
                                    sizeX = 1,
                                    sizeY = item.MobilePosition.SizeY,
                                    row = item.MobilePosition.Row,
                                    col = 0
                                },
                                factor_position = new
                                {
                                    sizeX = 1,
                                    sizeY = 1,
                                    row = item.FactorOrder,
                                    col = 0
                                }
                            });
                        }

                        //Color Picker
                        else if (field is DomainClasses.FormField_ColorPicker)
                        {
                            DomainClasses.FormField_ColorPicker item = (DomainClasses.FormField_ColorPicker)field;

                            fields.Add(new
                            {
                                type = 3,
                                data = new
                                {
                                    id = item.Id,
                                    title = item.Title,
                                    showCustomer = item.ShowCustomer,
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
                                    canDelete = item.CanDelete,
                                    isRequired = item.IsRequired
                                },
                                desktop_position = new
                                {
                                    sizeX = item.DesktopPosition.SizeX,
                                    sizeY = item.DesktopPosition.SizeY,
                                    row = item.DesktopPosition.Row,
                                    col = item.DesktopPosition.Column
                                },
                                tablet_position = new
                                {
                                    sizeX = item.TabletPosition.SizeX,
                                    sizeY = item.TabletPosition.SizeY,
                                    row = item.TabletPosition.Row,
                                    col = item.TabletPosition.Column
                                },
                                mobile_position = new
                                {
                                    sizeX = 1,
                                    sizeY = item.MobilePosition.SizeY,
                                    row = item.MobilePosition.Row,
                                    col = 0
                                },
                                factor_position = new
                                {
                                    sizeX = 1,
                                    sizeY = 1,
                                    row = 0,
                                    col = 0
                                }
                            });
                        }


                        //File Uploader
                        else if (field is DomainClasses.FormField_FileUploader)
                        {
                            DomainClasses.FormField_FileUploader item = (DomainClasses.FormField_FileUploader)field;

                            fields.Add(new
                            {
                                type = 4,
                                data = new
                                {
                                    id = item.Id,
                                    title = item.Title,
                                    showCustomer = item.ShowCustomer,
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
                                    canDelete = item.CanDelete,
                                    isRequired = item.IsRequired,
                                    sizeLimits = item.SizeLimits,
                                    minSize = item.MinSize,
                                    maxSize = item.MaxSize,
                                    fileTypes = item.Formats.Select(c => new
                                    {
                                        Id = c.Id,
                                        Title = c.Title,
                                        Extention = c.Extention
                                    })
                                },
                                desktop_position = new
                                {
                                    sizeX = item.DesktopPosition.SizeX,
                                    sizeY = item.DesktopPosition.SizeY,
                                    row = item.DesktopPosition.Row,
                                    col = item.DesktopPosition.Column
                                },
                                tablet_position = new
                                {
                                    sizeX = item.TabletPosition.SizeX,
                                    sizeY = item.TabletPosition.SizeY,
                                    row = item.TabletPosition.Row,
                                    col = item.TabletPosition.Column
                                },
                                mobile_position = new
                                {
                                    sizeX = 1,
                                    sizeY = item.MobilePosition.SizeY,
                                    row = item.MobilePosition.Row,
                                    col = 0
                                },
                                factor_position = new
                                {
                                    sizeX = 1,
                                    sizeY = 1,
                                    row = 0,
                                    col = 0
                                }
                            });
                        }

                        //Checkbox
                        else if (field is DomainClasses.FormField_CheckBox)
                        {
                            DomainClasses.FormField_CheckBox item = (DomainClasses.FormField_CheckBox)field;

                            fields.Add(new
                            {
                                type = 5,
                                data = new
                                {
                                    id = item.Id,
                                    title = item.Title,
                                    showCustomer = item.ShowCustomer,
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
                                    canDelete = item.CanDelete,
                                    showInFactor = item.ShowInFactor,
                                    factorOrder = item.FactorOrder
                                },
                                desktop_position = new
                                {
                                    sizeX = item.DesktopPosition.SizeX,
                                    sizeY = item.DesktopPosition.SizeY,
                                    row = item.DesktopPosition.Row,
                                    col = item.DesktopPosition.Column
                                },
                                tablet_position = new
                                {
                                    sizeX = item.TabletPosition.SizeX,
                                    sizeY = item.TabletPosition.SizeY,
                                    row = item.TabletPosition.Row,
                                    col = item.TabletPosition.Column
                                },
                                mobile_position = new
                                {
                                    sizeX = 1,
                                    sizeY = item.MobilePosition.SizeY,
                                    row = item.MobilePosition.Row,
                                    col = 0
                                },
                                factor_position = new
                                {
                                    sizeX = 1,
                                    sizeY = 1,
                                    row = item.FactorOrder,
                                    col = 0
                                }
                            });
                        }

                        //Web URl
                        else if (field is DomainClasses.FormField_WebUrl)
                        {
                            DomainClasses.FormField_WebUrl item = (DomainClasses.FormField_WebUrl)field;

                            fields.Add(new
                            {
                                type = 6,
                                data = new
                                {
                                    id = item.Id,
                                    title = item.Title,
                                    showCustomer = item.ShowCustomer,
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
                                    canDelete = item.CanDelete,
                                    isRequired = item.IsRequired
                                },
                                desktop_position = new
                                {
                                    sizeX = item.DesktopPosition.SizeX,
                                    sizeY = item.DesktopPosition.SizeY,
                                    row = item.DesktopPosition.Row,
                                    col = item.DesktopPosition.Column
                                },
                                tablet_position = new
                                {
                                    sizeX = item.TabletPosition.SizeX,
                                    sizeY = item.TabletPosition.SizeY,
                                    row = item.TabletPosition.Row,
                                    col = item.TabletPosition.Column
                                },
                                mobile_position = new
                                {
                                    sizeX = 1,
                                    sizeY = item.MobilePosition.SizeY,
                                    row = item.MobilePosition.Row,
                                    col = 0
                                },
                                factor_position = new
                                {
                                    sizeX = 1,
                                    sizeY = 1,
                                    row = 0,
                                    col = 0
                                }
                            });
                        }

                        //Date Picker
                        else if (field is DomainClasses.FormField_DatePicker)
                        {
                            DomainClasses.FormField_DatePicker item = (DomainClasses.FormField_DatePicker)field;

                            fields.Add(new
                            {
                                type = 7,
                                data = new
                                {
                                    id = item.Id,
                                    title = item.Title,
                                    showCustomer = item.ShowCustomer,
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
                                    canDelete = item.CanDelete,
                                    isRequired = item.IsRequired,
                                    limits = item.Limits,
                                    min = item.Min,
                                    max = item.Max,
                                    showInFactor = item.ShowInFactor,
                                    factorOrder = item.FactorOrder
                                },
                                desktop_position = new
                                {
                                    sizeX = item.DesktopPosition.SizeX,
                                    sizeY = item.DesktopPosition.SizeY,
                                    row = item.DesktopPosition.Row,
                                    col = item.DesktopPosition.Column
                                },
                                tablet_position = new
                                {
                                    sizeX = item.TabletPosition.SizeX,
                                    sizeY = item.TabletPosition.SizeY,
                                    row = item.TabletPosition.Row,
                                    col = item.TabletPosition.Column
                                },
                                mobile_position = new
                                {
                                    sizeX = 1,
                                    sizeY = item.MobilePosition.SizeY,
                                    row = item.MobilePosition.Row,
                                    col = 0
                                },
                                factor_position = new
                                {
                                    sizeX = 1,
                                    sizeY = 1,
                                    row = item.FactorOrder,
                                    col = 0
                                }
                            });
                        }

                        //Drop Down
                        else if (field is DomainClasses.FormField_DropDown)
                        {
                            DomainClasses.FormField_DropDown item = (DomainClasses.FormField_DropDown)field;

                            fields.Add(new
                            {
                                type = 8,
                                data = new
                                {
                                    id = item.Id,
                                    title = item.Title,
                                    showCustomer = item.ShowCustomer,
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
                                    canDelete = item.CanDelete,
                                    isRequired = item.IsRequired,
                                    showInFactor = item.ShowInFactor,
                                    factorOrder = item.FactorOrder,
                                    items = item.Items.Where(x => x.ShowAdmin).OrderBy(c => c.Order).Select(c => new
                                    {
                                        id = c.Id,
                                        title = c.Title
                                    })
                                },
                                desktop_position = new
                                {
                                    sizeX = item.DesktopPosition.SizeX,
                                    sizeY = item.DesktopPosition.SizeY,
                                    row = item.DesktopPosition.Row,
                                    col = item.DesktopPosition.Column
                                },
                                tablet_position = new
                                {
                                    sizeX = item.TabletPosition.SizeX,
                                    sizeY = item.TabletPosition.SizeY,
                                    row = item.TabletPosition.Row,
                                    col = item.TabletPosition.Column
                                },
                                mobile_position = new
                                {
                                    sizeX = 1,
                                    sizeY = item.MobilePosition.SizeY,
                                    row = item.MobilePosition.Row,
                                    col = 0
                                },
                                factor_position = new
                                {
                                    sizeX = 1,
                                    sizeY = 1,
                                    row = item.FactorOrder,
                                    col = 0
                                }
                            });
                        }

                        //Multiple Choice
                        else if (field is DomainClasses.FormField_RadioButtonGroup)
                        {
                            DomainClasses.FormField_RadioButtonGroup item = (DomainClasses.FormField_RadioButtonGroup)field;

                            fields.Add(new
                            {
                                type = 9,
                                data = new
                                {
                                    id = item.Id,
                                    title = item.Title,
                                    showCustomer = item.ShowCustomer,
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
                                    canDelete = item.CanDelete,
                                    isRequired = item.IsRequired,
                                    showInFactor = item.ShowInFactor,
                                    factorOrder = item.FactorOrder,
                                    items = item.Items.Where(x => x.ShowAdmin).OrderBy(c => c.Order).Select(c => new
                                    {
                                        id = c.Id,
                                        title = c.Title
                                    })
                                },
                                desktop_position = new
                                {
                                    sizeX = item.DesktopPosition.SizeX,
                                    sizeY = item.DesktopPosition.SizeY,
                                    row = item.DesktopPosition.Row,
                                    col = item.DesktopPosition.Column
                                },
                                tablet_position = new
                                {
                                    sizeX = item.TabletPosition.SizeX,
                                    sizeY = item.TabletPosition.SizeY,
                                    row = item.TabletPosition.Row,
                                    col = item.TabletPosition.Column
                                },
                                mobile_position = new
                                {
                                    sizeX = 1,
                                    sizeY = item.MobilePosition.SizeY,
                                    row = item.MobilePosition.Row,
                                    col = 0
                                },
                                factor_position = new
                                {
                                    sizeX = 1,
                                    sizeY = 1,
                                    row = item.FactorOrder,
                                    col = 0
                                }
                            });
                        }

                        //CheckBox Group
                        else if (field is DomainClasses.FormField_CheckBoxGroup)
                        {
                            DomainClasses.FormField_CheckBoxGroup item = (DomainClasses.FormField_CheckBoxGroup)field;

                            fields.Add(new
                            {
                                type = 10,
                                data = new
                                {
                                    id = item.Id,
                                    title = item.Title,
                                    showCustomer = item.ShowCustomer,
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
                                    canDelete = item.CanDelete,
                                    showInFactor = item.ShowInFactor,
                                    factorOrder = item.FactorOrder,
                                    items = item.Items.Where(x => x.ShowAdmin).OrderBy(c => c.Order).Select(c => new
                                    {
                                        id = c.Id,
                                        title = c.Title
                                    })
                                },
                                desktop_position = new
                                {
                                    sizeX = item.DesktopPosition.SizeX,
                                    sizeY = item.DesktopPosition.SizeY,
                                    row = item.DesktopPosition.Row,
                                    col = item.DesktopPosition.Column
                                },
                                tablet_position = new
                                {
                                    sizeX = item.TabletPosition.SizeX,
                                    sizeY = item.TabletPosition.SizeY,
                                    row = item.TabletPosition.Row,
                                    col = item.TabletPosition.Column
                                },
                                mobile_position = new
                                {
                                    sizeX = 1,
                                    sizeY = item.MobilePosition.SizeY,
                                    row = item.MobilePosition.Row,
                                    col = 0
                                },
                                factor_position = new
                                {
                                    sizeX = 1,
                                    sizeY = 1,
                                    row = item.FactorOrder,
                                    col = 0
                                }
                            });
                        }
                    }
                }
            }

            result.Data = new
            {
                id = form.Id,
                title = form.Title,
                group = new 
                {
                    Id = form.Group.Id,
                    Title = form.Group.Title
                },
                priority = form.Priority,
                specialCreativity = form.SpecialCreativity,
                isShow = form.IsShow,
                description = form.Description,
                canDelete = form.CanDelete,
                fields = fields
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Get(DomainClasses.Portal portal, string title, int pageIndex = 1)
        {
            if (pageIndex <= 0) pageIndex = 1;
            int pageSize = 20;
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                IQueryable<DomainClasses.Form> query = context.Forms.AsQueryable();

                query = query.Where(x => x.Portal == portal);

                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(x => x.Title.Contains(title));
                }

                int pageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(query.Count()) / Convert.ToDouble(pageSize)));
                int resultCount = query.Count();

                List<DomainClasses.Form> list = query.OrderBy(x => x.Title)
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
                        Title = x.Title,
                        SpecialCreativity = x.SpecialCreativity,
                        IsShow = x.IsShow,
                        Description = x.Description,
                        CanDelete = x.CanDelete
                    }).ToArray()
                };
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Show(int id)
        {
            bool result = false;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.Form item = context.Forms.Find(id);
                item.IsShow = true;
                context.SaveChanges();
                result = true;
            }

            return Content(result.ToString());
        }

        [HttpPost]
        public ActionResult Hide(int id)
        {
            bool result = false;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.Form item = context.Forms.Find(id);
                item.IsShow = false;
                context.SaveChanges();
                result = true;
            }

            return Content(result.ToString());
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            bool result = false;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.Form item = context.Forms
                    .Include(x => x.Fields)
                    .Single(x => x.Id == id);
                if (item.CanDelete)
                {
                    //try
                    //{
                        context.Forms.Remove(item);
                        context.SaveChanges();

                        result = true;

                        foreach (DomainClasses.FormField field in item.Fields)
                        {
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("~/Content/FormField"), field.PictureHelpFile)))
                            {
                                System.IO.File.Delete(string.Format("{0}/{1}",
                                    HostingEnvironment.MapPath("~/Content/FormField"), field.PictureHelpFile));
                            }
                        }
                    //}
                    //catch { }
                }
            }

            return Content(result.ToString());
        }

        [HttpPost]
        public ActionResult UploadPicture(HttpPostedFileBase file)
        {
            string fileName = "";

            if (file != null)
            {
                if (file.ContentType == "image/jpg" || file.ContentType == "image/jpeg" || file.ContentType == "image/png")
                {
                    if (file.ContentLength <= 150 * 1024)
                    {
                        fileName = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(file.FileName));
                        file.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/Upload"), fileName));
                    }
                }
            }

            return Content(fileName);
        }
    }
}