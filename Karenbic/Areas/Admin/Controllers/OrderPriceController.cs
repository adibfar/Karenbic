using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Karenbic.Areas.Admin.Controllers
{
    public class OrderPriceController : Controller
    {
        private DataAccess.Context _context;

        public OrderPriceController(DataAccess.Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetFields(int id)
        {
            List<object> fields = new List<object>();

            //Numeric
            List<DomainClasses.FormField_Numeric> numerics = await _context.FormFileds_Numeric
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer && x.UseForPrice)
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
                    minValue = item.Defualt,
                    maxValue = item.Defualt,
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

            //Checkbox
            List<DomainClasses.FormField_CheckBox> checkboxs = await _context.FormFields_CheckBox
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer && x.UseForPrice)
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

            //Drop Down
            List<DomainClasses.FormField_DropDown> dropDowns = await _context.FormFields_DropDown
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer && x.UseForPrice)
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
            List<DomainClasses.FormField_RadioButtonGroup> radioButtonGroups = await _context.FormFields_RadioButtonGroup
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer && x.UseForPrice)
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
            List<DomainClasses.FormField_CheckBoxGroup> checkBoxGroups = await _context.FormFields_CheckBoxGroup
                .AsQueryable()
                .Where(x => x.Form.Id == id && x.Form.IsShow &&
                    x.ShowAdmin && x.ShowCustomer && x.UseForPrice)
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

            return Json(fields, JsonRequestBehavior.AllowGet);
        }
    }
}