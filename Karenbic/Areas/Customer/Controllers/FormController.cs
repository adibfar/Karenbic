using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Karenbic.Areas.Customer.Controllers
{
    [UserInfrastructure.RACVAccess(Roles = "Customer")]
    public class FormController : Controller
    {
        private DataAccess.Context _context;

        public FormController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Get(DomainClasses.Portal portal, bool isPrintForm)
        {
            JsonResult result = new JsonResult();

            List<DomainClasses.Form> list = _context.Forms
                .Where(x => x.IsShow && x.IsShowAdmin && x.Portal == portal)
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

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetFields(int id)
        {
            JsonResult result = new JsonResult();
            DomainClasses.Form form = new DomainClasses.Form();
            List<object> fields = new List<object>();

            form = await _context.Forms.SingleAsync(x => x.Id == id && x.IsShow);

            //Text Box
            List<DomainClasses.FormField_TextBox> textboxs = await _context.FormFields_TextBox
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer)
                .ToListAsync();

            foreach (DomainClasses.FormField_TextBox item in textboxs)
            {
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
                        priority = item.Priority,
                        defualt = item.Defualt,
                        isRequired = item.IsRequired,
                        characterLimits = item.CharacterLimits,
                        minCharacters = item.MinCharacters,
                        maxCharacters = item.MaxCharacters,
                        showInFactor = item.ShowInFactor,
                        factorOrder = item.FactorOrder
                    },
                    value = item.Defualt,
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

            //Text Area
            List<DomainClasses.FormField_TextArea> textAreas = await _context.FormFields_TextArea
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer)
                .ToListAsync();

            foreach (DomainClasses.FormField_TextArea item in textAreas)
            {
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
                        priority = item.Priority,
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
            List<DomainClasses.FormField_Numeric> numerics = await _context.FormFileds_Numeric
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer)
                .ToListAsync();

            foreach (DomainClasses.FormField_Numeric item in numerics)
            {
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
                        priority = item.Priority,
                        isInt = item.IsInt,
                        isFloat = item.IsFloat,
                        isRequired = item.IsRequired,
                        limits = item.Limits,
                        min = item.Min,
                        max = item.Max,
                        showInFactor = item.ShowInFactor,
                        factorOrder = item.FactorOrder
                    },
                    value = item.Defualt,
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

            //ColorPicker
            List<DomainClasses.FormField_ColorPicker> colorboxes = await _context.FormFields_ColorPicker
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer)
                .ToListAsync();

            foreach (DomainClasses.FormField_ColorPicker item in colorboxes)
            {
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
                        priority = item.Priority,
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
            List<DomainClasses.FormField_FileUploader> fileUploaders = await _context.FormFields_FileUploader
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer)
                .ToListAsync();

            foreach (DomainClasses.FormField_FileUploader item in fileUploaders)
            {
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
                        priority = item.Priority,
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
            List<DomainClasses.FormField_CheckBox> checkboxs = await _context.FormFields_CheckBox
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer)
                .ToListAsync();

            foreach (DomainClasses.FormField_CheckBox item in checkboxs)
            {
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
                        priority = item.Priority,
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
            List<DomainClasses.FormField_WebUrl> webUrls = await _context.FormFields_WebUrl
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer)
                .ToListAsync();

            foreach (DomainClasses.FormField_WebUrl item in webUrls)
            {
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
                        priority = item.Priority,
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
            List<DomainClasses.FormField_DatePicker> datePickers = await _context.FormFields_DatePicker
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer)
                .ToListAsync();

            foreach (DomainClasses.FormField_DatePicker item in datePickers)
            {
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
                        priority = item.Priority,
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
            List<DomainClasses.FormField_DropDown> dropDowns = await _context.FormFields_DropDown
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer)
                .Include(x => x.Items)
                .ToListAsync();

            foreach (DomainClasses.FormField_DropDown item in dropDowns)
            {
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
                        priority = item.Priority,
                        isRequired = item.IsRequired,
                        showInFactor = item.ShowInFactor,
                        factorOrder = item.FactorOrder,
                        items = item.Items.Where(x => x.ShowAdmin && x.ShowCustomer).OrderBy(c => c.Order).Select(c => new
                        {
                            id = c.Id,
                            title = c.Title
                        }).ToArray()
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
            List<DomainClasses.FormField_RadioButtonGroup> radioButtonGroups = await _context.FormFields_RadioButtonGroup
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer)
                .Include(x => x.Items)
                .ToListAsync();

            foreach (DomainClasses.FormField_RadioButtonGroup item in radioButtonGroups)
            {
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
                        priority = item.Priority,
                        isRequired = item.IsRequired,
                        showInFactor = item.ShowInFactor,
                        factorOrder = item.FactorOrder,
                        items = item.Items.Where(x => x.ShowAdmin && x.ShowCustomer).OrderBy(c => c.Order).Select(c => new
                        {
                            id = c.Id,
                            title = c.Title,
                            pictureHelpFile = c.PictureHelpFile,
                            pictureHelpPath = c.PictureHelpPath,
                            hasPictureHelpFile = c.HasPictureHelpFile
                        }).ToArray()
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
            List<DomainClasses.FormField_CheckBoxGroup> checkBoxGroups = await _context.FormFields_CheckBoxGroup
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer)
                .Include(x => x.Items)
                .ToListAsync();

            foreach (DomainClasses.FormField_CheckBoxGroup item in checkBoxGroups)
            {
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
                        priority = item.Priority,
                        showInFactor = item.ShowInFactor,
                        factorOrder = item.FactorOrder,
                        items = item.Items.Where(x => x.ShowAdmin && x.ShowCustomer).OrderBy(c => c.Order).Select(c => new
                        {
                            id = c.Id,
                            title = c.Title,
                            pictureHelpFile = c.PictureHelpFile,
                            pictureHelpPath = c.PictureHelpPath,
                            hasPictureHelpFile = c.HasPictureHelpFile
                        }).ToArray()
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

            //Extended File Uploader
            List<DomainClasses.FormField_FileUploader2> extendedFileUploaders = await _context.FormFields_FileUploader2
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer)
                .ToListAsync();

            foreach (DomainClasses.FormField_FileUploader2 item in extendedFileUploaders)
            {
                fields.Add(new
                {
                    type = 11,
                    data = new
                    {
                        id = item.Id,
                        title = item.Title,
                        description = item.Description,
                        pictureHelpFile = item.PictureHelpFile,
                        pictureHelpPath = item.PictureHelpPath,
                        hasPictureHelpFile = item.HasPictureHelpFile,
                        priority = item.Priority,
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

            //Label
            List<DomainClasses.FormField_Label> labels = await _context.FormFields_Label
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer)
                .ToListAsync();

            foreach (DomainClasses.FormField_Label item in labels)
            {
                fields.Add(new
                {
                    type = 12,
                    data = new
                    {
                        id = item.Id,
                        title = item.Title,
                        description = item.Description,
                        pictureHelpFile = item.PictureHelpFile,
                        pictureHelpPath = item.PictureHelpPath,
                        hasPictureHelpFile = item.HasPictureHelpFile,
                        priority = item.Priority,
                        font = item.FontFamily,
                        size = item.FontSize,
                        color = item.Color,
                        underline = item.Underline,
                        upline = item.Upline
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