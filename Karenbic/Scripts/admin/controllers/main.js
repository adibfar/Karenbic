/*=========================================================
 * Module: main.js
 * Main Application Controller
 =========================================================*/

App.controller('AppController',
  ['$rootScope', '$scope', '$state', '$window', '$localStorage', '$timeout', 'colors', 'browser', 'cfpLoadingBar',
  function($rootScope, $scope, $state, $window, $localStorage, $timeout, colors, browser, cfpLoadingBar) {
      "use strict";

    $scope.isDesignPortal = function () {
        if ($state.includes('app.design')) {
            return true;
        }
        return false;
    }

    $scope.isPrintPortal = function () {
        if ($state.includes('app.print')) {
            return true;
        }
        return false;
    }

    // Loading bar transition
    var thBar;
    $rootScope.$on('$stateChangeStart', function(event, toState, toParams, fromState, fromParams) {
        if ($('#center-content > section').length) // check if bar container exists
          thBar = $timeout(function() {
            cfpLoadingBar.start();
          }, 0); // sets a latency Threshold
    });

      // Hook success
    $rootScope.$on('$stateChangeSuccess', function(event, toState, toParams, fromState, fromParams) {
        event.targetScope.$watch("$viewContentLoaded", function () {
          $timeout.cancel(thBar);
          cfpLoadingBar.complete();
        });

        // display new view from top
        $window.scrollTo(0, 0);

        if ($state.includes('app.design')) {
            $rootScope.app.portal = 'design';
        }
        else {
            $rootScope.app.portal = 'print';
        }
    });

    // Hook not found
    $rootScope.$on('$stateNotFound',
      function(event, unfoundState, fromState, fromParams) {
          console.log(unfoundState.to); // "lazy.state"
          console.log(unfoundState.toParams); // {a:1, b:2}
          console.log(unfoundState.options); // {inherit:false} + default options
      });

    
    // Allows to use branding color with interpolation
    // {{ colorByName('primary') }}
    $scope.colorByName = colors.byName;
}]);
