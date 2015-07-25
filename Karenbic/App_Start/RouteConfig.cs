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
               name: "ShowPriceList",
               url: "PriceList/{id}",
               defaults: new { controller = "PriceList", action = "Show" },
               namespaces: new string[] { "Karenbic.Controllers" }
           );

            routes.MapRoute(
               name: "PortfolioType",
               url: "Portfolio",
               defaults: new { controller = "Portfolio", action = "ShowTypes" },
               namespaces: new string[] { "Karenbic.Controllers" }
           );

            routes.MapRoute(
               name: "PortfolioCategory",
               url: "Portfolio/{typeId}",
               defaults: new { controller = "Portfolio", action = "ShowCategories" },
               namespaces: new string[] { "Karenbic.Controllers" }
           );

            routes.MapRoute(
               name: "Portfolio",
               url: "Portfolio/{typeId}/{categoryId}",
               defaults: new { controller = "Portfolio", action = "ShowPortfolios" },
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
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new string[] { "Karenbic.Controllers" }
            );
        }
    }
}
