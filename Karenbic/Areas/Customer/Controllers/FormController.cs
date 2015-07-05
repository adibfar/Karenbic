using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Customer.Controllers
{
    public class FormController : Controller
    {
        [HttpGet]
        public ActionResult Get(DomainClasses.Portal portal, bool isPrintForm)
        {
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                List<DomainClasses.Form> list = context.Forms
                    .Where(x => x.IsShow && x.Portal == portal)
                    .OrderBy(x => x.Title)
                    .ToList();

                result.Data = list.Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    SpecialCreativity = x.SpecialCreativity,
                    Description = x.Description,
                    CanDelete = x.CanDelete
                });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetFields(int id)
        {
            JsonResult result = new JsonResult();
            DomainClasses.Form form = new DomainClasses.Form();
            List<object> fields = new List<object>();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                form = context.Forms
                    .Include(x => x.Fields)
                    .Include(x => x.Fields.Select(c => c.DesktopPosition))
                    .Include(x => x.Fields.Select(c => c.TabletPosition))
                    .Include(x => x.Fields.Select(c => c.MobilePosition))
                    .Single(x => x.Id == id && x.IsShow);

                foreach (DomainClasses.FormField field in form.Fields)
                {
                    if (field.ShowAdmin && field.ShowCustomer)
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
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
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
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
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
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
                                    isInt = item.IsInt,
                                    isFloat = item.IsFloat,
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
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
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
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
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
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
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
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
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
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
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
                                    description = item.Description,
                                    pictureHelpFile = item.PictureHelpFile,
                                    pictureHelpPath = item.PictureHelpPath,
                                    hasPictureHelpFile = item.HasPictureHelpFile,
                                    isRequired = item.IsRequired,
                                    showInFactor = item.ShowInFactor,
                                    factorOrder = item.FactorOrder,
                                    items = item.Items.Where(x => x.ShowAdmin && x.ShowCustomer).OrderBy(c => c.Order).Select(c => new
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
                                    items = item.Items.Where(x => x.ShowAdmin && x.ShowCustomer).OrderBy(c => c.Order).Select(c => new
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
                                    items = item.Items.Where(x => x.ShowAdmin && x.ShowCustomer).OrderBy(c => c.Order).Select(c => new
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
                specialCreativity = form.SpecialCreativity,
                isShow = form.IsShow,
                description = form.Description,
                canDelete = form.CanDelete,
                fields = fields
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}