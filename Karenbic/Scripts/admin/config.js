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
                .state('app.design.add-form', {
                    url: '/add-form',
                    templateUrl: helper.basepath('Form/Add'),
                    controller: 'AddFormController',
                    resolve: helper.resolveFor('ddlist', 'chosen', 'stepper', 'nicefileinput', 'jquery-resize', 'gridster', 'ngDialog', 'colorpicker')
                })
                .state('app.design.edit-form', {
                    url: '/edit-form/:id',
                    templateUrl: helper.basepath('Form/Edit'),
                    controller: 'EditFormController',
                    resolve: helper.resolveFor('ddlist', 'chosen', 'stepper', 'nicefileinput', 'jquery-resize', 'gridster', 'ngDialog', 'colorpicker')
                })
                .state('app.design.forms-list', {
                    url: '/forms-list',
                    templateUrl: helper.basepath('Form/List'),
                    controller: 'FormsListController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.design.customer-group', {
                    url: '/customer-group',
                    templateUrl: helper.basepath('CustomerGroup/Index'),
                    controller: 'CustomerGroupController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.design.customer', {
                    url: '/customer',
                    templateUrl: helper.basepath('Customer/List'),
                    controller: 'CustomerController',
                    resolve: helper.resolveFor('ngDialog', 'chosen')
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
                .state('app.design.price-list', {
                    url: '/price-list',
                    templateUrl: helper.basepath('PriceList/Index'),
                    controller: 'PriceListController',
                    resolve: helper.resolveFor('ngDialog', 'nicefileinput', 'stepper')
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
                .state('app.print.add-form', {
                    url: '/add-form',
                    templateUrl: helper.basepath('Form/Add'),
                    controller: 'AddFormController',
                    resolve: helper.resolveFor('ddlist', 'chosen', 'stepper', 'nicefileinput', 'jquery-resize', 'gridster', 'ngDialog', 'colorpicker')
                })
                .state('app.print.edit-form', {
                    url: '/edit-form/:id',
                    templateUrl: helper.basepath('Form/Edit'),
                    controller: 'EditFormController',
                    resolve: helper.resolveFor('ddlist', 'chosen', 'stepper', 'nicefileinput', 'jquery-resize', 'gridster', 'ngDialog', 'colorpicker')
                })
                .state('app.print.forms-list', {
                    url: '/forms-list',
                    templateUrl: helper.basepath('Form/List'),
                    controller: 'FormsListController',
                    resolve: helper.resolveFor('ngDialog')
                })
                .state('app.print.customer-group', {
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
                .state('app.print.price-list', {
                    url: '/price-list',
                    templateUrl: helper.basepath('PriceList/Index'),
                    controller: 'PriceListController',
                    resolve: helper.resolveFor('ngDialog', 'nicefileinput', 'stepper')
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