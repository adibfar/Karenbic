using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Data.Entity.Infrastructure;
using System.Xml.Linq;
using System.Web.Hosting;

namespace Karenbic
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataAccess.Context, DataAccess.Migrations.Configuration>());
            Database.SetInitializer<DataAccess.Context>(null);
            //using (DataAccess.Context context = new DataAccess.Context())
            //{
            //    context.Database.Initialize(true);
            //}

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
