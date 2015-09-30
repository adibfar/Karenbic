using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Karenbic
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Products",
               url: "Products/{categoryId}",
               defaults: new { controller = "Product", action = "Index", categoryId = UrlParameter.Optional },
               namespaces: new string[] { "Karenbic.Controllers" }
           );

            routes.MapRoute(
               name: "Product",
               url: "Product/{id}",
               defaults: new { controller = "Product", action = "Detail" },
               namespaces: new string[] { "Karenbic.Controllers" }
           );

            routes.MapRoute(
               name: "PortfolioDetail",
               url: "Portfolio/Detail/{id}",
               defaults: new { controller = "Portfolio", action = "Detail" },
               namespaces: new string[] { "Karenbic.Controllers" }
           );

            routes.MapRoute(
               name: "Portfolio",
               url: "Portfolio/{typeId}/{categoryId}",
               defaults: new { controller = "Portfolio", action = "Index", categoryId = UrlParameter.Optional },
               namespaces: new string[] { "Karenbic.Controllers" }
           );

            routes.MapRoute(
               name: "PublicPriceList",
               url: "PriceList/{id}",
               defaults: new { controller = "PriceList", action = "Show" },
               namespaces: new string[] { "Karenbic.Controllers" }
           );

            routes.MapRoute(
               name: "AboutUs",
               url: "AboutUs",
               defaults: new { controller = "Home", action = "AboutUs" },
               namespaces: new string[] { "Karenbic.Controllers" }
           );

            routes.MapRoute(
               name: "ContactUs",
               url: "ContactUs",
               defaults: new { controller = "Home", action = "ContactUs" },
               namespaces: new string[] { "Karenbic.Controllers" }
           );

            routes.MapRoute(
               name: "PublicHelp",
               url: "Help",
               defaults: new { controller = "Home", action = "Help" },
               namespaces: new string[] { "Karenbic.Controllers" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new string[] { "Karenbic.Controllers" }
            );
        }
    }
}
