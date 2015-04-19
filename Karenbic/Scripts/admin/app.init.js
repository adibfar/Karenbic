if (typeof $ === 'undefined') { throw new Error('This application\'s JavaScript requires jQuery'); }

/*=-=-=-=-=-=-= Application Start =-=-=-=-=-=-=*/
var App = angular.module('KarenbicApp', ['ngRoute', 'ngAnimate', 'ngStorage', 'ngCookies',
                                    'ui.bootstrap', 'ui.router', 'oc.lazyLoad',
                                    'cfp.loadingBar', 'angularFileUpload'])
            .run(["$rootScope", "$state", "$stateParams", function ($rootScope, $state, $stateParams) {
                    // Set reference to access them from any scope
                    $rootScope.$state = $state;
                    $rootScope.$stateParams = $stateParams;

                    // Scope Globals
                    $rootScope.app = {
                        portal: 'design'
                    };

                    $rootScope.user = {
                        name: '',
                        surname: '',
                        username: ''
                    };
                }
            ])
.filter('tel', function () { });