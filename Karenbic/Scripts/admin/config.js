/**=========================================================
 * Module: config.js
 * App routes and resources configuration
 =========================================================*/

App
    .config(['$stateProvider', '$locationProvider', '$urlRouterProvider', 'RouteHelpersProvider',
        function ($stateProvider, $locationProvider, $urlRouterProvider, helper) {
            'use strict';

            // Set the following to true to enable the HTML5 Mode
            // You may have to set <base> tag in index and a routing configuration in your server
            $locationProvider.html5Mode(false);

            // defaults to dashboard
            $urlRouterProvider.otherwise('/app/design/dashboard');

            // 
            // Application Routes
            // -----------------------------------  
            $stateProvider
                .state('app', {
                    url: '/app',
                    abstract: true,
                    templateUrl: helper.basepath('Admin'),
                    controller: 'AppController',
                    resolve: helper.resolveFor('fastclick', 'modernizr', 'icons', 'animo', 'classyloader', 'toaster', 'whirl', 'tooltipster')
                })
                .state('app.design', {
                    url: '/design',
                    abstract: true,
                    templateUrl: helper.basepath('DesignAdmin'),
                    controller: 'NullController'
                })
                .state('app.design.dashboard', {
                    url: '/dashboard',
                    templateUrl: helper.basepath('DesignAdmin/Dashboard'),
                    controller: 'NullController'
                })
                .state('app.design.form-groups', {
                    url: '/form-groups',
                    templateUrl: helper.basepath('FormGroup/Index'),
                    controller: 'FormGroupController',
                    resolve: helper.resolveFor('stepper', 'ngDialog')
                })
                .state('app.design.add-form', {
                    url: '/add-form',
                    templateUrl: helper.basepath('Form/Add'),
                    controller: 'AddFormController',
                    resolve: helper.resolveFor('ddlist', 'chosen', 'stepper', 'nicefileinput', 'jquery-resize',
                        'gridster', 'ngDialog', 'colorpicker', 'jquery-ui', 'jquery-ui-datepicker')
                })
                .state('app.design.edit-form', {
                    url: '/edit-form/:id',
                    templateUrl: helper.basepath('Form/Edit'),
                    controller: 'EditFormController',
                    resolve: helper.resolveFor('ddlist', 'chosen', 'stepper', 'nicefileinput', 'jquery-resize',
                        'gridster', 'ngDialog', 'colorpicker', 'jquery-ui', 'jquery-ui-datepicker')
                })
                .state('app.design.forms-list', {
                    url: '/forms-list',
                    templateUrl: helper.basepath('Form/List'),
                    controller: 'FormsListController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.design.preorder', {
                    url: '/preorder',
                    templateUrl: helper.basepath('DesignOrder/PreOrder'),
                    controller: 'PreDesignOrderController',
                    resolve: helper.resolveFor('ngDialog', 'ngCkeditor')
                })
                .state('app.design.new-order-list', {
                    url: '/new-order-list',
                    templateUrl: helper.basepath('DesignOrder/NewOrderList'),
                    controller: 'NewDesignOrderListController',
                    resolve: helper.resolveFor('ngDialog', 'jquery-ui', 'jquery-ui-datepicker')
                })
                .state('app.design.ongoing-order-list', {
                    url: '/ongoing-order-list',
                    templateUrl: helper.basepath('DesignOrder/OngoingOrderList'),
                    controller: 'OngoingDesignOrderListController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'jquery-ui', 'jquery-ui-datepicker')
                })
                 .state('app.design.finished-order-list', {
                     url: '/finished-order-list',
                     templateUrl: helper.basepath('DesignOrder/FinishedOrderList'),
                     controller: 'FinishedDesignOrderListController',
                     resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker')
                 })
                .state('app.design.canceled-order-list', {
                    url: '/canceled-order-list',
                    templateUrl: helper.basepath('DesignOrder/CanceledOrderList'),
                    controller: 'CanceledDesignOrderListController',
                    resolve: helper.resolveFor('ngDialog', 'jquery-ui', 'jquery-ui-datepicker')
                })
                .state('app.design.send-order-design', {
                    url: '/send-order-design/:id',
                    templateUrl: helper.basepath('DesignOrder/SendOrderDesign'),
                    controller: 'SendOrderDesignController',
                    resolve: helper.resolveFor('ngDialog', 'image-scale', 'jquery-colorbox')
                })
                .state('app.design.show-order-design', {
                    url: '/show-order-design/:id',
                    templateUrl: helper.basepath('DesignOrder/ShowOrderDesign'),
                    controller: 'ShowOrderDesignController',
                    resolve: helper.resolveFor('ngDialog', 'image-scale', 'jquery-colorbox')
                })
                .state('app.design.payment-list', {
                    url: '/payment-list',
                    templateUrl: helper.basepath('DesignPayment/List'),
                    controller: 'DesignPaymentListController',
                    resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker')
                })
                .state('app.design.factor-list', {
                    url: '/factor-list',
                    templateUrl: helper.basepath('FactorOfDesignOrder/List'),
                    controller: 'FactorOfDesignOrderListController',
                    resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker')
                })
                .state('app.design.add-financial-conflict', {
                    url: '/add-financial-conflict',
                    templateUrl: helper.basepath('FinancialConflict/Add'),
                    controller: 'AddFinancialConflictController',
                    resolve: helper.resolveFor('chosen')
                })
                .state('app.design.financial-conflict-list', {
                    url: '/financial-conflict-list',
                    templateUrl: helper.basepath('FinancialConflict/List'),
                    controller: 'FinancialConflictListController',
                    resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker', 'ngDialog')
                })
                .state('app.print', {
                    url: '/print',
                    abstract: true,
                    templateUrl: helper.basepath('PrintAdmin'),
                    controller: 'NullController'
                })
                .state('app.print.dashboard', {
                    url: '/dashboard',
                    templateUrl: helper.basepath('PrintAdmin/Dashboard'),
                    controller: 'NullController'
                })
                .state('app.print.form-groups', {
                    url: '/form-groups',
                    templateUrl: helper.basepath('FormGroup/Index'),
                    controller: 'FormGroupController',
                    resolve: helper.resolveFor('stepper', 'ngDialog')
                })
                .state('app.print.add-form', {
                    url: '/add-form',
                    templateUrl: helper.basepath('Form/Add'),
                    controller: 'AddFormController',
                    resolve: helper.resolveFor('ddlist', 'chosen', 'stepper', 'nicefileinput', 'jquery-resize',
                        'gridster', 'ngDialog', 'colorpicker', 'jquery-ui', 'jquery-ui-datepicker')
                })
                .state('app.print.edit-form', {
                    url: '/edit-form/:id',
                    templateUrl: helper.basepath('Form/Edit'),
                    controller: 'EditFormController',
                    resolve: helper.resolveFor('ddlist', 'chosen', 'stepper', 'nicefileinput', 'jquery-resize',
                        'gridster', 'ngDialog', 'colorpicker', 'jquery-ui', 'jquery-ui-datepicker')
                })
                .state('app.print.forms-list', {
                    url: '/forms-list',
                    templateUrl: helper.basepath('Form/List'),
                    controller: 'FormsListController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.print.preorder', {
                    url: '/preorder',
                    templateUrl: helper.basepath('PrintOrder/PreOrder'),
                    controller: 'PrePrintOrderController',
                    resolve: helper.resolveFor('ngDialog', 'ngCkeditor')
                })
                .state('app.print.new-order-list', {
                    url: '/new-order-list',
                    templateUrl: helper.basepath('PrintOrder/NewOrderList'),
                    controller: 'NewPrintOrderListController',
                    resolve: helper.resolveFor('ngDialog', 'jquery-ui', 'jquery-ui-datepicker')
                })
                .state('app.print.ongoing-order-list', {
                    url: '/ongoing-order-list',
                    templateUrl: helper.basepath('PrintOrder/OngoingOrderList'),
                    controller: 'OngoingPrintOrderListController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'jquery-ui', 'jquery-ui-datepicker')
                })
                 .state('app.print.finished-order-list', {
                     url: '/finished-order-list',
                     templateUrl: helper.basepath('PrintOrder/FinishedOrderList'),
                     controller: 'FinishedPrintOrderListController',
                     resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker')
                 })
                .state('app.print.canceled-order-list', {
                    url: '/canceled-order-list',
                    templateUrl: helper.basepath('PrintOrder/CanceledOrderList'),
                    controller: 'CanceledPrintOrderListController',
                    resolve: helper.resolveFor('ngDialog', 'jquery-ui', 'jquery-ui-datepicker')
                })
                .state('app.print.payment-list', {
                    url: '/payment-list',
                    templateUrl: helper.basepath('PrintPayment/List'),
                    controller: 'PrintPaymentListController',
                    resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker')
                })
                .state('app.print.factor-list', {
                    url: '/factor-list',
                    templateUrl: helper.basepath('FactorOfPrintOrder/List'),
                    controller: 'FactorOfPrintOrderListController',
                    resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker')
                })
                .state('app.print.add-financial-conflict', {
                    url: '/add-financial-conflict',
                    templateUrl: helper.basepath('FinancialConflict/Add'),
                    controller: 'AddFinancialConflictController',
                    resolve: helper.resolveFor('chosen')
                })
                .state('app.print.financial-conflict-list', {
                    url: '/financial-conflict-list',
                    templateUrl: helper.basepath('FinancialConflict/List'),
                    controller: 'FinancialConflictListController',
                    resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker', 'ngDialog')
                })
                
                //Shared Pages
                //_____________________
                .state('app.print.order-prices', {
                    url: '/order-prices',
                    templateUrl: helper.basepath('PrintOrderPrice/Index'),
                    controller: 'PrintOrderPriceController',
                    resolve: helper.resolveFor('ngDialog', 'chosen', 'stepper')
                })
                .state('app.design.order-prices', {
                    url: '/order-prices',
                    templateUrl: helper.basepath('DesignOrderPrice/Index'),
                    controller: 'DesignOrderPriceController',
                    resolve: helper.resolveFor('ngDialog', 'chosen', 'stepper')
                })
                .state('app.print.customer-group', {
                    url: '/customer-group',
                    templateUrl: helper.basepath('CustomerGroup/Index'),
                    controller: 'CustomerGroupController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.design.customer-group', {
                    url: '/customer-group',
                    templateUrl: helper.basepath('CustomerGroup/Index'),
                    controller: 'CustomerGroupController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.print.customer', {
                    url: '/customer',
                    templateUrl: helper.basepath('Customer/List'),
                    controller: 'CustomerController',
                    resolve: helper.resolveFor('ngDialog', 'chosen')
                })
                .state('app.design.customer', {
                    url: '/customer',
                    templateUrl: helper.basepath('Customer/List'),
                    controller: 'CustomerController',
                    resolve: helper.resolveFor('ngDialog', 'chosen')
                })
                .state('app.print.price-list', {
                    url: '/price-list',
                    templateUrl: helper.basepath('PriceList/Index'),
                    controller: 'PriceListController',
                    resolve: helper.resolveFor('ngDialog', 'nicefileinput', 'stepper')
                })
                .state('app.design.price-list', {
                    url: '/price-list',
                    templateUrl: helper.basepath('PriceList/Index'),
                    controller: 'PriceListController',
                    resolve: helper.resolveFor('ngDialog', 'nicefileinput', 'stepper')
                })
                .state('app.print.send-message-new', {
                    url: '/send-message-new',
                    templateUrl: helper.basepath('SendMessage/New'),
                    controller: 'NewSendMessageController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'ngCkeditor')
                })
                .state('app.design.send-message-new', {
                    url: '/send-message-new',
                    templateUrl: helper.basepath('SendMessage/New'),
                    controller: 'NewSendMessageController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'ngCkeditor')
                })
                .state('app.print.send-message-list', {
                    url: '/send-message-list',
                    templateUrl: helper.basepath('SendMessage/List'),
                    controller: 'SendMessageListController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.design.send-message-list', {
                    url: '/send-message-list',
                    templateUrl: helper.basepath('SendMessage/List'),
                    controller: 'SendMessageListController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.print.receive-message-list', {
                    url: '/receive-message-list',
                    templateUrl: helper.basepath('ReceiveMessage/List'),
                    controller: 'ReceiveMessageListController',
                    resolve: helper.resolveFor('ngDialog', 'froala')
                })
                .state('app.design.receive-message-list', {
                    url: '/receive-message-list',
                    templateUrl: helper.basepath('ReceiveMessage/List'),
                    controller: 'ReceiveMessageListController',
                    resolve: helper.resolveFor('ngDialog', 'froala')
                })
                .state('app.print.change-password', {
                    url: '/change-password',
                    templateUrl: helper.basepath('Profile/ChangePassword'),
                    controller: 'ChangePasswordController'
                })
                .state('app.design.change-password', {
                    url: '/change-password',
                    templateUrl: helper.basepath('Profile/ChangePassword'),
                    controller: 'ChangePasswordController'
                })
                
                //Public Portal
                //_____________________
                 .state('app.print.portfolio-categories', {
                     url: '/portfolio-categories',
                     templateUrl: helper.basepath('PortfolioCategory/Index'),
                     controller: 'PortfolioCategoriesController',
                     resolve: helper.resolveFor('chosen', 'ngDialog', 'nicefileinput', 'stepper')
                 })
                .state('app.design.portfolio-categories', {
                    url: '/portfolio-categories',
                    templateUrl: helper.basepath('PortfolioCategory/Index'),
                    controller: 'PortfolioCategoriesController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'nicefileinput', 'stepper')
                })
                .state('app.print.portfolio-add', {
                    url: '/portfolio-add',
                    templateUrl: helper.basepath('Portfolio/Add'),
                    controller: 'AddPortfolioController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'nicefileinput', 'stepper', 'image-scale')
                })
                .state('app.design.portfolio-add', {
                    url: '/portfolio-add',
                    templateUrl: helper.basepath('Portfolio/Add'),
                    controller: 'AddPortfolioController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'nicefileinput', 'stepper', 'image-scale')
                })
                .state('app.print.portfolios', {
                    url: '/portfolios',
                    templateUrl: helper.basepath('Portfolio/Index'),
                    controller: 'PortfoliosController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'nicefileinput', 'stepper', 'image-scale')
                })
                .state('app.design.portfolios', {
                    url: '/portfolios',
                    templateUrl: helper.basepath('Portfolio/Index'),
                    controller: 'PortfoliosController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'nicefileinput', 'stepper', 'image-scale')
                })
                .state('app.print.public-price', {
                    url: '/public-price',
                    templateUrl: helper.basepath('PublicPrice/Index'),
                    controller: 'PublicPriceController',
                    resolve: helper.resolveFor('ngDialog', 'nicefileinput', 'stepper', 'chosen')
                })
                .state('app.design.public-price', {
                    url: '/public-price',
                    templateUrl: helper.basepath('PublicPrice/Index'),
                    controller: 'PublicPriceController',
                    resolve: helper.resolveFor('ngDialog', 'nicefileinput', 'stepper', 'chosen')
                })
                .state('app.print.public-price-add', {
                    url: '/public-price-add',
                    templateUrl: helper.basepath('PublicPrice/Add'),
                    controller: 'AddPublicPriceController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'ngCkeditor', 'stepper')
                })
                .state('app.design.public-price-add', {
                    url: '/public-price-add',
                    templateUrl: helper.basepath('PublicPrice/Add'),
                    controller: 'AddPublicPriceController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'ngCkeditor', 'stepper')
                })
                .state('app.print.public-price-edit', {
                    url: '/public-price-edit/:id',
                    templateUrl: helper.basepath('PublicPrice/Edit'),
                    controller: 'EditPublicPriceController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'ngCkeditor', 'stepper')
                })
                .state('app.design.public-price-edit', {
                    url: '/public-price-edit/:id',
                    templateUrl: helper.basepath('PublicPrice/Edit'),
                    controller: 'EditPublicPriceController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'ngCkeditor', 'stepper')
                })
                .state('app.print.product-categories', {
                    url: '/product-categories',
                    templateUrl: helper.basepath('ProductCategory/Index'),
                    controller: 'ProductCategoriesController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'nicefileinput', 'stepper')
                })
                .state('app.design.product-categories', {
                    url: '/product-categories',
                    templateUrl: helper.basepath('ProductCategory/Index'),
                    controller: 'ProductCategoriesController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'nicefileinput', 'stepper')
                })
                .state('app.print.product-add', {
                    url: '/product-add',
                    templateUrl: helper.basepath('Product/Add'),
                    controller: 'AddProductController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'nicefileinput', 'stepper', 'image-scale')
                })
                .state('app.design.product-add', {
                    url: '/product-add',
                    templateUrl: helper.basepath('Product/Add'),
                    controller: 'AddProductController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'nicefileinput', 'stepper', 'image-scale')
                })
                .state('app.print.products', {
                    url: '/products',
                    templateUrl: helper.basepath('Product/List'),
                    controller: 'ProductsController',
                    resolve: helper.resolveFor('chosen', 'ngDialog')
                })
                .state('app.design.products', {
                    url: '/products',
                    templateUrl: helper.basepath('Product/List'),
                    controller: 'ProductsController',
                    resolve: helper.resolveFor('chosen', 'ngDialog')
                })
                .state('app.design.product-edit', {
                    url: '/product-edit/:id',
                    templateUrl: helper.basepath('Product/Edit'),
                    controller: 'EditProductController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'nicefileinput', 'stepper', 'image-scale')
                })
                .state('app.print.product-edit', {
                    url: '/product-edit/:id',
                    templateUrl: helper.basepath('Product/Edit'),
                    controller: 'EditProductController',
                    resolve: helper.resolveFor('chosen', 'ngDialog', 'nicefileinput', 'stepper', 'image-scale')
                })
                .state('app.print.public-home-slide-show', {
                    url: '/public-home-slide-show',
                    templateUrl: helper.basepath('HomeSlideShow/Index'),
                    controller: 'HomeSlideShowController',
                    resolve: helper.resolveFor('ngDialog', 'nicefileinput', 'stepper')
                })
                .state('app.design.public-home-slide-show', {
                    url: '/public-home-slide-show',
                    templateUrl: helper.basepath('HomeSlideShow/Index'),
                    controller: 'HomeSlideShowController',
                    resolve: helper.resolveFor('ngDialog', 'nicefileinput', 'stepper')
                })
                .state('app.print.aboutus', {
                    url: '/aboutus',
                    templateUrl: helper.basepath('PublicContent/AboutUs'),
                    controller: 'Public_AboutUsController',
                    resolve: helper.resolveFor('ngDialog', 'ngCkeditor')
                })
                .state('app.design.aboutus', {
                    url: '/aboutus',
                    templateUrl: helper.basepath('PublicContent/AboutUs'),
                    controller: 'Public_AboutUsController',
                    resolve: helper.resolveFor('ngDialog', 'ngCkeditor')
                })
                .state('app.print.contactus', {
                    url: '/contactus',
                    templateUrl: helper.basepath('PublicContent/ContactUs'),
                    controller: 'Public_ContactUsController',
                    resolve: helper.resolveFor('ngDialog', 'ngCkeditor')
                })
                .state('app.design.contactus', {
                    url: '/contactus',
                    templateUrl: helper.basepath('PublicContent/ContactUs'),
                    controller: 'Public_ContactUsController',
                    resolve: helper.resolveFor('ngDialog', 'ngCkeditor')
                })
                .state('app.print.public-help', {
                    url: '/public-help',
                    templateUrl: helper.basepath('PublicContent/PublicHelp'),
                    controller: 'Public_HelpController',
                    resolve: helper.resolveFor('ngDialog', 'ngCkeditor')
                })
                .state('app.design.public-help', {
                    url: '/public-help',
                    templateUrl: helper.basepath('PublicContent/PublicHelp'),
                    controller: 'Public_HelpController',
                    resolve: helper.resolveFor('ngDialog', 'ngCkeditor')
                });

    }]).config(['$ocLazyLoadProvider', 'APP_REQUIRES',
        function ($ocLazyLoadProvider, APP_REQUIRES) {
        'use strict';
        // Lazy Load modules configuration
        $ocLazyLoadProvider.config({
            debug: false,
            events: true,
            modules: APP_REQUIRES.modules
        });

    }]).config(['$controllerProvider', '$compileProvider', '$filterProvider', '$provide',
        function ($controllerProvider, $compileProvider, $filterProvider, $provide) {
            'use strict';
            // registering components after bootstrap
            App.controller = $controllerProvider.register;
            App.directive = $compileProvider.directive;
            App.filter = $filterProvider.register;
            App.factory = $provide.factory;
            App.service = $provide.service;
            App.constant = $provide.constant;
            App.value = $provide.value;

    }]).config(['cfpLoadingBarProvider',
        function (cfpLoadingBarProvider) {
            cfpLoadingBarProvider.includeBar = true;
            cfpLoadingBarProvider.includeSpinner = false;
            cfpLoadingBarProvider.latencyThreshold = 500;
            cfpLoadingBarProvider.parentSelector = '#contents > #loading-bar';
    }]).controller('NullController', function () { });