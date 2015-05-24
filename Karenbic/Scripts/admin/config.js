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
                    resolve: helper.resolveFor('fastclick', 'modernizr', 'icons', 'animo', 'classyloader', 'toaster', 'whirl')
                })
                .state('app.design', {
                    url: '/design',
                    abstract: true,
                    templateUrl: helper.basepath('DesignAdmin'),
                    controller: 'NullController'
                    //resolve: resolveFor('moment', 'datetimepicker', 'toaster', 'mousewheel', 'scrollbar', 'inputmask')
                })
                .state('app.design.dashboard', {
                    url: '/dashboard',
                    templateUrl: helper.basepath('DesignAdmin/Dashboard'),
                    controller: 'NullController'
                    //resolve: resolveFor('moment', 'datetimepicker', 'toaster', 'mousewheel', 'scrollbar', 'inputmask')
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
                .state('app.print', {
                    url: '/print',
                    templateUrl: helper.basepath('PrintAdmin'),
                    controller: 'NullController'
                    //resolve: resolveFor('moment', 'datetimepicker', 'toaster', 'mousewheel', 'scrollbar', 'inputmask')
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
            cfpLoadingBarProvider.parentSelector = '#center-content > .loading-bar';
    }]).controller('NullController', function () { });