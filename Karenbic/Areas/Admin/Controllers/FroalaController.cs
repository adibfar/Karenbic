using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Karenbic.Areas.Admin.Controllers
{
    public class FroalaController : Controller
    {
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            string fileName = string.Empty;

            if (file != null &&
                (file.ContentType == "image/jpg" || file.ContentType == "image/jpeg" || file.ContentType == "image/png") &&
                file.ContentLength <= 250 * 1024)
            {
                fileName = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(file.FileName));
                file.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/FroalaUpload"), fileName));
            }
            else
            {
                throw new Exception();
            }

            return Json(new 
            {
                link = string.Format("{0}/{1}", "/Content/FroalaUpload", fileName)
            });
        }
    }
}