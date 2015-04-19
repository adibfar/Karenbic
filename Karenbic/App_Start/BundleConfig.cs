using System.Web;
using System.Web.Optimization;

namespace Karenbic
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/admin/base").Include(
                // jQuery
                "~/Vendors/jquery-1.11.2/jquery-1.11.2.min.js",
                "~/Vendors/jquery-1.11.2/jquery-migrate-1.2.1.min.js",
                // lodash
                "~/Vendors/lodash-3.5.0/lodash.min.js",
                // Angular
                "~/Vendors/angular-1.3.15/angular.min.js",
                "~/Vendors/angular-1.3.15/angular-route.min.js",
                "~/Vendors/angular-1.3.15/angular-cookies.min.js",
                "~/Vendors/angular-1.3.15/angular-animate.min.js",
                "~/Vendors/angular-1.3.15/angular-touch.min.js",
                // Angular UI Route
                "~/Vendors/angular-ui-router/angular-ui-router.min.js",
                // Angular Storage
                "~/Vendors/angular-storage/ngStorage.js",
                // Angular File Upload
                "~/Vendors/angular-file-upload/angular-file-upload.min.js",
                "~/Vendors/angular-file-upload/FileAPI.min.js",
                // oclazyload
                "~/Vendors/oclazyload/ocLazyLoad.min.js",
                // Bootstrap
                "~/Vendors/bootstrap-3.3.4/js/bootstrap.min.js",
                // UI Bootstrap
                "~/Vendors/ui-bootstrap/ui-bootstrap-tpls-0.12.1.min.js",
                // Loading Bar
                "~/Vendors/angular-loadingbar/loading-bar.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/admin/app").Include(
                "~/Scripts/admin/app.init.js",
                "~/Scripts/admin/config.js",
                "~/Scripts/admin/constants.js",
                "~/Scripts/services/browser.js",
                "~/Scripts/services/colors.js",
                "~/Scripts/admin/controllers/main.js"
                //"~/Scripts/admin/modules/controllers/top-menu.js"
            ));

            bundles.Add(new StyleBundle("~/Styles/AdminStyle").Include(
                "~/Vendors/bootstrap-3.3.4/css/bootstrap.css",
                "~/Vendors/bootstrap-rtl/bootstrap-rtl.css",
                "~/Styles/Admin.css"
            ));

            BundleTable.EnableOptimizations = true;
        }
    }
}
