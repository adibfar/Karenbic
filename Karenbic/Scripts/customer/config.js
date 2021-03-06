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
            $urlRouterProvider.otherwise('/app/design/preorder');

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
                .state('app.design.preorder', {
                    url: '/preorder',
                    templateUrl: helper.basepath('DesignOrder/PreOrder'),
                    controller: 'PreDesignOrderController'
                })
                .state('app.design.add-order', {
                    url: '/add-order',
                    templateUrl: helper.basepath('Order/Add'),
                    controller: 'AddOrderController',
                    resolve: helper.resolveFor('chosen', 'stepper', 'nicefileinput', 'jquery-resize', 'gridster',
                        'colorpicker', 'jquery-ui', 'jquery-ui-datepicker', 'persian-date', 'ngDialog', 'bpopup')
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
                .state('app.design.final-payment-preview', {
                    url: '/final-payment-preview/:id',
                    templateUrl: helper.basepath('DesignFinalPayment/Preview'),
                    controller: 'DesignFinalPaymentPreviewController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.design.checkout-final-payment', {
                    url: '/checkout-final-payment/:id',
                    templateUrl: helper.basepath('DesignFinalPayment/Checkout'),
                    controller: 'DesignFinalPaymentCheckoutController'
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
                .state('app.design.financial-conflict-list', {
                    url: '/financial-conflict-list',
                    templateUrl: helper.basepath('FinancialConflict/List'),
                    controller: 'FinancialConflictListController',
                    resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker', 'ngDialog')
                })
                .state('app.design.financial-conflict-payment-preview', {
                    url: '/financial-conflict-payment-preview/:id',
                    templateUrl: helper.basepath('FinancialConflict/PaymentPreview'),
                    controller: 'FinancialConflictPaymentPreviewController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.design.order-list', {
                    url: '/order-list',
                    templateUrl: helper.basepath('DesignOrder/List'),
                    controller: 'DesignOrderListController',
                    resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker', 'ngDialog')
                })
                .state('app.design.show-order', {
                    url: '/show-order/:id',
                    templateUrl: helper.basepath('DesignOrder/Show'),
                    controller: 'ShowDesignOrderController',
                    resolve: helper.resolveFor('ngDialog', 'image-scale', 'cropit')
                })
                .state('app.design.price-list', {
                    url: '/price-list',
                    templateUrl: helper.basepath('PriceList/Index'),
                    controller: 'PriceListController',
                    resolve: helper.resolveFor('chosen')
                })
                .state('app.design.send-message-new', {
                    url: '/send-message-new',
                    templateUrl: helper.basepath('SendMessage/New'),
                    controller: 'NewSendMessageController',
                    resolve: helper.resolveFor('ngDialog', 'froala')
                })
                .state('app.design.send-message-list', {
                    url: '/send-message-list',
                    templateUrl: helper.basepath('SendMessage/List'),
                    controller: 'SendMessageListController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.design.receive-message-list', {
                    url: '/receive-message-list',
                    templateUrl: helper.basepath('ReceiveMessage/List'),
                    controller: 'ReceiveMessageListController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.design.change-profile', {
                    url: '/change-profile',
                    templateUrl: helper.basepath('Profile/Edit'),
                    controller: 'ChangeProfileController',
                    resolve: helper.resolveFor('chosen')
                })
                .state('app.design.change-password', {
                    url: '/change-password',
                    templateUrl: helper.basepath('Profile/ChangePassword'),
                    controller: 'ChangePasswordController'
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
                .state('app.print.preorder', {
                    url: '/preorder',
                    templateUrl: helper.basepath('PrintOrder/PreOrder'),
                    controller: 'PrePrintOrderController'
                })
                .state('app.print.add-order', {
                    url: '/add-order',
                    templateUrl: helper.basepath('Order/Add'),
                    controller: 'AddOrderController',
                    resolve: helper.resolveFor('chosen', 'stepper', 'nicefileinput', 'jquery-resize', 'gridster',
                        'colorpicker', 'jquery-ui', 'jquery-ui-datepicker', 'persian-date', 'ngDialog', 'bpopup')
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
                .state('app.print.financial-conflict-list', {
                    url: '/financial-conflict-list',
                    templateUrl: helper.basepath('FinancialConflict/List'),
                    controller: 'FinancialConflictListController',
                    resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker', 'ngDialog')
                })
                .state('app.print.financial-conflict-payment-preview', {
                    url: '/financial-conflict-payment-preview/:id',
                    templateUrl: helper.basepath('FinancialConflict/PaymentPreview'),
                    controller: 'FinancialConflictPaymentPreviewController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.print.order-list', {
                    url: '/order-list',
                    templateUrl: helper.basepath('PrintOrder/List'),
                    controller: 'PrintOrderListController',
                    resolve: helper.resolveFor('jquery-ui', 'jquery-ui-datepicker', 'image-scale')
                })
                .state('app.print.price-list', {
                    url: '/price-list',
                    templateUrl: helper.basepath('PriceList/Index'),
                    controller: 'PriceListController',
                    resolve: helper.resolveFor('chosen')
                })
                .state('app.print.send-message-new', {
                    url: '/send-message-new',
                    templateUrl: helper.basepath('SendMessage/New'),
                    controller: 'NewSendMessageController',
                    resolve: helper.resolveFor('ngDialog', 'froala')
                })
                .state('app.print.send-message-list', {
                    url: '/send-message-list',
                    templateUrl: helper.basepath('SendMessage/List'),
                    controller: 'SendMessageListController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.print.receive-message-list', {
                    url: '/receive-message-list',
                    templateUrl: helper.basepath('ReceiveMessage/List'),
                    controller: 'ReceiveMessageListController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.print.change-profile', {
                    url: '/change-profile',
                    templateUrl: helper.basepath('Profile/Edit'),
                    controller: 'ChangeProfileController',
                    resolve: helper.resolveFor('chosen')
                })
                .state('app.print.change-password', {
                    url: '/change-password',
                    templateUrl: helper.basepath('Profile/ChangePassword'),
                    controller: 'ChangePasswordController'
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