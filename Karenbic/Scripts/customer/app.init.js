if (typeof $ === 'undefined') { throw new Error('This application\'s JavaScript requires jQuery'); }

/*=-=-=-=-=-=-= Application Start =-=-=-=-=-=-=*/
var App = angular.module('KarenbicApp', [
    'ngRoute',
    'ngAnimate',
    'ngStorage',
    'ngCookies',
    'ngSanitize',
    'ngResource',
    'ui.bootstrap',
    'ui.router',
    'oc.lazyLoad',
    'cfp.loadingBar',
    'angularFileUpload',
    'ui.utils'
]);

App.run(["$rootScope", "$state", "$stateParams", '$window', '$templateCache',
    function ($rootScope, $state, $stateParams, $window, $templateCache) {
        // Set reference to access them from any scope
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;
        $rootScope.$storage = $window.localStorage;

        // Scope Globals
        $rootScope.app = {
            portal: 'design',
            viewAnimation: 'ng-fadeIn'
        };

        $rootScope.user = {
            name: '',
            surname: '',
            username: ''
        };
    }]);