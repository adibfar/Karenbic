using System.Web;
using System.Web.Optimization;

namespace Karenbic
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/portal/base").Include(
                // jQuery
                "~/Vendors/jquery-1.11.2/jquery-1.11.2.min.js",
                "~/Vendors/jquery-1.11.2/jquery-migrate-1.2.1.min.js",
                // SignalR
                "~/Scripts/jquery.signalR-2.2.0.min.js",
                // lodash
                "~/Vendors/lodash-3.5.0/lodash.min.js",
                // Angular
                "~/Vendors/angular-1.3.15/angular.min.js",
                "~/Vendors/angular-1.3.15/angular-route.min.js",
                "~/Vendors/angular-1.3.15/angular-cookies.min.js",
                "~/Vendors/angular-1.3.15/angular-animate.min.js",
                "~/Vendors/angular-1.3.15/angular-touch.min.js",
                "~/Vendors/angular-1.3.15/angular-resource.min.js",
                "~/Vendors/angular-1.3.15/angular-sanitize.min.js",
                // Angular UI Route
                "~/Vendors/angular-ui-router/angular-ui-router.min.js",
                // UI Utils
                "~/Vendors/angular-ui-utils/ui-utils.min.js",
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
                //Intialize
                "~/Scripts/admin/app.init.js",
                "~/Scripts/admin/config.js",
                "~/Scripts/admin/constants.js",
                // Services
                "~/Scripts/services/browser.js",
                "~/Scripts/services/colors.js",
                "~/Scripts/services/route-helpers.js",
                //Directives
                "~/Scripts/directives/anchor.js",
                "~/Scripts/directives/animate-enabled.js",
                "~/Scripts/directives/bootstrap-table-responsive.js",
                "~/Scripts/directives/chosen-select.js",
                "~/Scripts/directives/classy-loader.js",
                "~/Scripts/directives/clear-storage.js",
                "~/Scripts/directives/colorbox.js",
                "~/Scripts/directives/colorpicker.js",
                "~/Scripts/directives/datepicker.js",
                "~/Scripts/directives/ddlist-select.js",
                "~/Scripts/directives/image-scale.js",
                "~/Scripts/directives/load-css.js",
                "~/Scripts/directives/ngMatch.js",
                "~/Scripts/directives/nicefileinput.js",
                "~/Scripts/directives/now.js",
                "~/Scripts/directives/only-digits.js",
                "~/Scripts/directives/stepper.js",
                "~/Scripts/directives/scrollbar.js",
                "~/Scripts/directives/stopEvent.js",
                "~/Scripts/directives/tooltipster.js",
                //Filters
                "~/Scripts/filters/riyal-currency.js",
                //Controllers
                "~/Scripts/admin/controllers/main.js",
                "~/Scripts/admin/controllers/top-menu.js",
                "~/Scripts/admin/controllers/mobile-menu.js",
                "~/Scripts/admin/controllers/form-group.js",
                "~/Scripts/admin/controllers/form-add.js",
                "~/Scripts/admin/controllers/form-edit.js",
                "~/Scripts/admin/controllers/form-list.js",
                "~/Scripts/admin/controllers/financial-conflict-add.js",
                "~/Scripts/admin/controllers/financial-conflict-list.js",
                "~/Scripts/admin/controllers/customer-group.js",
                "~/Scripts/admin/controllers/customer.js",
                "~/Scripts/admin/controllers/portfolio-categories.js",
                "~/Scripts/admin/controllers/portfolio-add.js",
                "~/Scripts/admin/controllers/portfolio-edit.js",
                "~/Scripts/admin/controllers/portfolios.js",
                "~/Scripts/admin/controllers/price-list.js",
                "~/Scripts/admin/controllers/send-message-new.js",
                "~/Scripts/admin/controllers/send-message-list.js",
                "~/Scripts/admin/controllers/receive-message-list.js",
                "~/Scripts/admin/controllers/change-mobile-number.js",
                "~/Scripts/admin/controllers/change-password.js",
                "~/Scripts/admin/controllers/design-order-price.js",
                "~/Scripts/admin/controllers/design-preorder.js",
                "~/Scripts/admin/controllers/design-new-order-list.js",
                "~/Scripts/admin/controllers/design-ongoing-order-list.js",
                "~/Scripts/admin/controllers/design-finished-order-list.js",
                "~/Scripts/admin/controllers/design-canceled-order-list.js",
                "~/Scripts/admin/controllers/design-send-design.js",
                "~/Scripts/admin/controllers/design-show-design.js",
                "~/Scripts/admin/controllers/design-payment-list.js",
                "~/Scripts/admin/controllers/design-factor-list.js",
                "~/Scripts/admin/controllers/design-factor-text.js",
                "~/Scripts/admin/controllers/print-order-price.js",
                "~/Scripts/admin/controllers/print-preorder.js",
                "~/Scripts/admin/controllers/print-new-order-list.js",
                "~/Scripts/admin/controllers/print-ongoing-order-list.js",
                "~/Scripts/admin/controllers/print-finished-order-list.js",
                "~/Scripts/admin/controllers/print-canceled-order-list.js",
                "~/Scripts/admin/controllers/print-payment-list.js",
                "~/Scripts/admin/controllers/print-factor-list.js",
                "~/Scripts/admin/controllers/print-factor-text.js",
                "~/Scripts/admin/controllers/public-price.js",
                "~/Scripts/admin/controllers/public-price-add.js",
                "~/Scripts/admin/controllers/public-price-edit.js",
                "~/Scripts/admin/controllers/public-product-categories.js",
                "~/Scripts/admin/controllers/public-product-add.js",
                "~/Scripts/admin/controllers/public-product-edit.js",
                "~/Scripts/admin/controllers/public-products.js",
                "~/Scripts/admin/controllers/public-home-slide-show.js",
                "~/Scripts/admin/controllers/public-aboutus.js",
                "~/Scripts/admin/controllers/public-contactus.js",
                "~/Scripts/admin/controllers/public-help.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/customer/app").Include(
                //Intialize
                "~/Scripts/customer/app.init.js",
                "~/Scripts/customer/config.js",
                "~/Scripts/customer/constants.js",
                // Services
                "~/Scripts/services/browser.js",
                "~/Scripts/services/colors.js",
                "~/Scripts/services/route-helpers.js",
                //Directives
                "~/Scripts/directives/anchor.js",
                "~/Scripts/directives/animate-enabled.js",
                "~/Scripts/directives/chosen-select.js",
                "~/Scripts/directives/classy-loader.js",
                "~/Scripts/directives/clear-storage.js",
                "~/Scripts/directives/colorbox.js",
                "~/Scripts/directives/colorpicker.js",
                 "~/Scripts/directives/cropit.js",
                "~/Scripts/directives/datepicker.js",
                "~/Scripts/directives/ddlist-select.js",
                 "~/Scripts/directives/image-scale.js",
                "~/Scripts/directives/load-css.js",
                "~/Scripts/directives/ngMatch.js",
                "~/Scripts/directives/nicefileinput.js",
                "~/Scripts/directives/now.js",
                "~/Scripts/directives/only-digits.js",
                "~/Scripts/directives/stepper.js",
                "~/Scripts/directives/scrollbar.js",
                "~/Scripts/directives/stopEvent.js",
                "~/Scripts/directives/tooltipster.js",
                //Filters
                "~/Scripts/filters/riyal-currency.js",
                //Controllers
                "~/Scripts/customer/controllers/main.js",
                "~/Scripts/customer/controllers/top-menu.js",
                "~/Scripts/customer/controllers/mobile-menu.js",
                "~/Scripts/customer/controllers/order-add.js",
                "~/Scripts/customer/controllers/price-list.js",
                "~/Scripts/customer/controllers/send-message-new.js",
                "~/Scripts/customer/controllers/send-message-list.js",
                "~/Scripts/customer/controllers/receive-message-list.js",
                "~/Scripts/customer/controllers/change-profile.js",
                "~/Scripts/customer/controllers/change-password.js",
                "~/Scripts/customer/controllers/design-preorder.js",
                "~/Scripts/customer/controllers/design-factor-list.js",
                "~/Scripts/customer/controllers/design-payment-preview.js",
                "~/Scripts/customer/controllers/design-payment-checkout.js",
                "~/Scripts/customer/controllers/design-final-payment-preview.js",
                "~/Scripts/customer/controllers/design-final-payment-checkout.js",
                "~/Scripts/customer/controllers/design-payment-list.js",
                "~/Scripts/customer/controllers/financial-conflict-list.js",
                "~/Scripts/customer/controllers/financial-conflict-payment-preview.js",
                "~/Scripts/customer/controllers/design-order-list.js",
                "~/Scripts/customer/controllers/design-show-order.js",
                "~/Scripts/customer/controllers/print-preorder.js",
                "~/Scripts/customer/controllers/print-factor-list.js",
                "~/Scripts/customer/controllers/print-payment-preview.js",
                "~/Scripts/customer/controllers/print-payment-checkout.js",
                "~/Scripts/customer/controllers/print-payment-list.js",
                "~/Scripts/customer/controllers/print-order-list.js",
                "~/Scripts/customer/controllers/error-payment.js"
            ));

            bundles.Add(new ScriptBundle("~/Vendors/froala/js").Include(
                "~/Vendors/froala-editor/js/froala_editor.min.js",
                "~/Vendors/froala-editor/js/angular-froala.js",
                "~/Vendors/froala-editor/js/froala-sanitize.js",
                "~/Vendors/froala-editor/js/plugins/block_styles.min.js",
                "~/Vendors/froala-editor/js/plugins/colors.min.js",
                "~/Vendors/froala-editor/js/plugins/font_size.min.js",
                "~/Vendors/froala-editor/js/plugins/lists.min.js",
                "~/Vendors/froala-editor/js/plugins/tables.min.js",
                "~/Vendors/froala-editor/js/plugins/video.min.js",
                "~/Vendors/froala-editor/js/langs/fa.js"
            ));

            bundles.Add(new ScriptBundle("~/Vendors/ckeditor/js").Include(
                "~/Vendors/ckeditor/ckeditor.js",
                "~/Vendors/ckeditor/lang/fa.js",
                "~/Vendors/angular-ckeditor/ng-ckeditor.js"
            ));

            bundles.Add(new StyleBundle("~/Styles/AdminStyle").Include(
                "~/Vendors/bootstrap-3.3.4/css/bootstrap.css",
                "~/Vendors/bootstrap-rtl/bootstrap-rtl.css",
                "~/Styles/Admin.css"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/public/base").Include(
                // jQuery
                "~/Vendors/jquery-1.11.2/jquery-1.11.2.min.js",
                "~/Vendors/jquery-1.11.2/jquery-migrate-1.2.1.min.js",
                // lodash
                "~/Vendors/lodash-3.5.0/lodash.min.js",
                // Bootstrap
                "~/Vendors/bootstrap-3.3.4/js/bootstrap.min.js"
            ));

            bundles.Add(new StyleBundle("~/Styles/publicStyle").Include(
                "~/Vendors/bootstrap-3.3.4/css/bootstrap.css",
                "~/Vendors/bootstrap-rtl/bootstrap-rtl.css"
                //"~/Styles/public.css"
            ));

            //BundleTable.EnableOptimizations = false;
        }
    }
}
