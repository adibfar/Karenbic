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
                    templateUrl: helper.basepath('Portal'),
                    controller: 'AppController',
                    resolve: helper.resolveFor('fastclick', 'modernizr', 'icons', 'animo', 'classyloader', 'toaster', 'whirl', 'tooltipster')
                })
                .state('app.design', {
                    url: '/design',
                    abstract: true,
                    templateUrl: helper.basepath('DesignPortal'),
                    controller: 'NullController'
                })
                .state('app.design.dashboard', {
                    url: '/dashboard',
                    templateUrl: helper.basepath('DesignPortal/Dashboard'),
                    controller: 'NullController'
                })
                .state('app.design.add-order', {
                    url: '/add-order',
                    templateUrl: helper.basepath('Order/Add'),
                    controller: 'AddOrderController',
                    resolve: helper.resolveFor('chosen', 'stepper', 'nicefileinput', 'jquery-resize', 'gridster',
                        'colorpicker', 'jquery-ui', 'jquery-ui-datepicker', 'persian-date')
                })
                .state('app.design.factor-list', {
                    url: '/factor-list',
                    templateUrl: helper.basepath('FactorOfDesignOrder/List'),
                    controller: 'FactorListOfDesignController',
                    resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker', 'persian-date', 'ngDialog')
                })
                .state('app.design.payment-preview', {
                    url: '/payment-preview/:id',
                    templateUrl: helper.basepath('DesignPayment/Preview'),
                    controller: 'DesignPaymentPreviewController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.design.checkout-payment', {
                    url: '/checkout-payment/:id',
                    templateUrl: helper.basepath('DesignPayment/Checkout'),
                    controller: 'DesignPaymentCheckoutController'
                })
                .state('app.design.error-payment', {
                    url: '/error-payment/:code',
                    templateUrl: helper.basepath('Payment/Error'),
                    controller: 'ErrorPaymentController'
                })
                .state('app.design.payment-list', {
                    url: '/payment-list',
                    templateUrl: helper.basepath('DesignPayment/List'),
                    controller: 'DesignPaymentListController',
                    resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker')
                })
                .state('app.print', {
                    url: '/print',
                    abstract: true,
                    templateUrl: helper.basepath('PrintPortal'),
                    controller: 'NullController'
                })
                .state('app.print.dashboard', {
                    url: '/dashboard',
                    templateUrl: helper.basepath('PrintPortal/Dashboard'),
                    controller: 'NullController'
                })
                .state('app.print.add-order', {
                    url: '/add-order',
                    templateUrl: helper.basepath('Order/Add'),
                    controller: 'AddOrderController',
                    resolve: helper.resolveFor('chosen', 'stepper', 'nicefileinput', 'jquery-resize', 'gridster',
                        'colorpicker', 'jquery-ui', 'jquery-ui-datepicker', 'persian-date')
                })
                .state('app.print.factor-list', {
                    url: '/factor-list',
                    templateUrl: helper.basepath('FactorOfPrintOrder/List'),
                    controller: 'FactorListOfPrintController',
                    resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker', 'persian-date', 'ngDialog')
                })
                .state('app.print.payment-preview', {
                    url: '/payment-preview/:id',
                    templateUrl: helper.basepath('PrintPayment/Preview'),
                    controller: 'PrintPaymentPreviewController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.print.checkout-payment', {
                    url: '/checkout-payment/:id',
                    templateUrl: helper.basepath('PrintPayment/Checkout'),
                    controller: 'PrintPaymentCheckoutController'
                })
                .state('app.print.error-payment', {
                    url: '/error-payment/:code',
                    templateUrl: helper.basepath('Payment/Error'),
                    controller: 'ErrorPaymentController'
                })
                .state('app.print.payment-list', {
                    url: '/payment-list',
                    templateUrl: helper.basepath('PrintPayment/List'),
                    controller: 'PrintPaymentListController',
                    resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker')
                })
                .state('app.print.order-list', {
                    url: '/order-list',
                    templateUrl: helper.basepath('PrintOrder/List'),
                    controller: 'PrintOrderListController',
                    resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker')
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