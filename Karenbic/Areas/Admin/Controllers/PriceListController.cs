using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Karenbic.Areas.Admin.Controllers
{
    public class PriceListController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string title, int order, DomainClasses.Portal portal, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid) throw new Exception();

            DomainClasses.PriceList model = new DomainClasses.PriceList();
            model.Title = title;
            model.Order = order;
            model.Portal = portal;

            if (file != null)
            {
                if (file.ContentType == "image/jpg" || file.ContentType == "image/jpeg" || file.ContentType == "image/png")
                {
                    if (file.ContentLength <= 250 * 1024)
                    {
                        model.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(file.FileName));
                        file.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/PriceList"), model.PictureFile));
                    }
                }
            }

            using (DataAccess.Context context = new DataAccess.Context())
            {
                context.PriceLists.Add(model);
                context.SaveChanges();
            }

            return Json(new 
            { 
                Id = model.Id,
                Title = model.Title,
                Order = model.Order,
                PictureFile = model.PictureFile,
                PicturePath = model.PicturePath
            });
        }

        [HttpPost]
        public ActionResult Edit(int id, string title, int order, HttpPostedFileBase file)
        {
            DomainClasses.PriceList model = new DomainClasses.PriceList();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                model = context.PriceLists.Find(id);
                model.Title = title;
                model.Order = order;

                if (file != null)
                {
                    if (file.ContentType == "image/jpg" || file.ContentType == "image/jpeg" || file.ContentType == "image/png")
                    {
                        if (file.ContentLength <= 250 * 1024)
                        {
                            string oldFile = model.PictureFile;

                            //save new picture
                            model.PictureFile = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(file.FileName));
                            file.SaveAs(string.Format("{0}/{1}", HostingEnvironment.MapPath("/Content/PriceList"), model.PictureFile));

                            //delete old picture
                            if (System.IO.File.Exists(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/PriceList"), oldFile)))
                            {
                                System.IO.File.Delete(string.Format("{0}/{1}",
                                HostingEnvironment.MapPath("/Content/PriceList"), oldFile));
                            }
                        }
                    }
                }

                context.SaveChanges();
            }

            return Json(new
            {
                Id = model.Id,
                Title = model.Title,
                Order = model.Order,
                PictureFile = model.PictureFile,
                PicturePath = model.PicturePath
            });
        }

        [HttpGet]
        public ActionResult Get(DomainClasses.Portal portal)
        {
            JsonResult result = new JsonResult();

            using (DataAccess.Context context = new DataAccess.Context())
            {
                List<DomainClasses.PriceList> list = context.PriceLists
                    .Where(x => x.Portal == portal)
                    .OrderBy(x => x.Order)
                    .ToList();

                result.Data = list.Select(x => new 
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Order = x.Order,
                        PictureFile = x.PictureFile,
                        PicturePath = x.PicturePath
                    }).ToArray();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Remove(int id)
        {
            bool result = false;

            using (DataAccess.Context context = new DataAccess.Context())
            {
                DomainClasses.PriceList item = context.PriceLists.Find(id);

                if (System.IO.File.Exists(string.Format("{0}/{1}", 
                    HostingEnvironment.MapPath("/Content/PriceList"), item.PictureFile)))
                {
                    System.IO.File.Delete(string.Format("{0}/{1}",
                    HostingEnvironment.MapPath("/Content/PriceList"), item.PictureFile));
                }

                context.PriceLists.Remove(item);
                context.SaveChanges();

                result = true;
            }

            return Content(result.ToString());
        }
    }
}