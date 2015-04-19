/*=========================================================
 * Module: config.js
 * App routes and resources configuration
 =========================================================*/

App.config(['$stateProvider', '$urlRouterProvider', '$controllerProvider', '$compileProvider',
    '$filterProvider', '$provide', '$ocLazyLoadProvider', 'APP_REQUIRES',
    function ($stateProvider, $urlRouterProvider, $controllerProvider, $compileProvider,
        $filterProvider, $provide, $ocLazyLoadProvider, appRequires) {
        'use strict';

        App.controller = $controllerProvider.register;
        App.directive = $compileProvider.directive;
        App.filter = $filterProvider.register;
        App.factory = $provide.factory;
        App.service = $provide.service;
        App.constant = $provide.constant;
        App.value = $provide.value;

        // LAZY MODULES
        $ocLazyLoadProvider.config({
            debug: false,
            events: true,
            modules: appRequires.modules
        });

        // defaults to dashboard
        $urlRouterProvider.otherwise('/app/design/dashboard');

        // Application Routes   
        $stateProvider
            .state('app', {
                url: '/app',
                abstract: true,
                templateUrl: basepath('Admin'),
                controller: 'AppController',
                resolve: resolveFor('fastclick', 'modernizr', 'icons', 'animo', 'classyloader', 'toaster', 'csspiner')
            })
            .state('app.design', {
                url: '/design',
                abstract: true,
                templateUrl: basepath('DesignAdmin'),
                controller: 'NullController'
                //resolve: resolveFor('moment', 'datetimepicker', 'toaster', 'mousewheel', 'scrollbar', 'inputmask')
            })
            .state('app.design.dashboard', {
                url: '/dashboard',
                templateUrl: basepath('DesignAdmin/Dashboard'),
                controller: 'NullController'
                //resolve: resolveFor('moment', 'datetimepicker', 'toaster', 'mousewheel', 'scrollbar', 'inputmask')
            })
            .state('app.print', {
                url: '/print',
                templateUrl: basepath('PrintAdmin'),
                controller: 'NullController'
                //resolve: resolveFor('moment', 'datetimepicker', 'toaster', 'mousewheel', 'scrollbar', 'inputmask')
            });


        // Set here the base of the relative path
        // for all app views
        function basepath(uri) {
            return '/Admin/' + uri;
        }

        // Generates a resolve object by passing script names
        // previously configured in constant.APP_REQUIRES
        function resolveFor() {
            var _args = arguments;
            return {
                deps: ['$ocLazyLoad', '$q', function ($ocLL, $q) {
                    // Creates a promise chain for each argument
                    var promise = $q.when(1); // empty promise
                    for (var i = 0, len = _args.length; i < len; i++) {
                        promise = andThen(_args[i]);
                    }
                    return promise;

                    // creates promise to chain dynamically
                    function andThen(_arg) {
                        // also support a function that returns a promise
                        if (typeof _arg == 'function')
                            return promise.then(_arg);
                        else
                            return promise.then(function () {
                                // if is a module, pass the name. If not, pass the array
                                var whatToLoad = getRequired(_arg);
                                // simple error check
                                if (!whatToLoad) return $.error('Route resolve: Bad resource name [' + _arg + ']');
                                // finally, return a promise
                                return $ocLL.load(whatToLoad);
                            });
                    }
                    // check and returns required data
                    // analyze module items with the form [name: '', files: []]
                    // and also simple array of script files (for not angular js)
                    function getRequired(name) {
                        if (appRequires.modules)
                            for (var m in appRequires.modules)
                                if (appRequires.modules[m].name && appRequires.modules[m].name === name)
                                    return appRequires.modules[m];
                        return appRequires.scripts && appRequires.scripts[name];
                    }

                }]
            };
        }

    }]).config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeBar = true;
        cfpLoadingBarProvider.includeSpinner = false;
        cfpLoadingBarProvider.latencyThreshold = 500;
        cfpLoadingBarProvider.parentSelector = '#center-content > .loading-bar';
    }])
.controller('NullController', function () { });
