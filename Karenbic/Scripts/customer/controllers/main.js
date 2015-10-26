/*=========================================================
 * Module: main.js
 * Main Application Controller
 =========================================================*/

App.controller('AppController',
  ['$rootScope', '$scope', '$state', '$window', '$localStorage', '$timeout',
      'colors', 'browser', 'cfpLoadingBar', '$http', 'APP_BASE_URI', '$sce', '$modal',
  function ($rootScope, $scope, $state, $window, $localStorage, $timeout,
            colors, browser, cfpLoadingBar, $http, baseUri, $sce, $modal) {
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
    $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
        if ($('#contents > #loading-bar').length) {// check if bar container exists
            $('#contents > #loading-bar').css({
                'left': ($(window).width() - 120) / 2,
                'top': ($(window).height() - 40) / 2,
            });

            $scope.showLoadingOverlay = true;

            thBar = $timeout(function () {
                cfpLoadingBar.start();
            }, 0); // sets a latency Threshold
        }

        //Hidden Menu
        $('.mobile-menu').animate({ 'left': '-260px' }, 200);
        $scope.mobileMenuToggled = false;
    });

      // Hook success
    $rootScope.$on('$stateChangeSuccess', function(event, toState, toParams, fromState, fromParams) {
        event.targetScope.$watch("$viewContentLoaded", function () {
          $timeout.cancel(thBar);
          cfpLoadingBar.complete();

          $timeout(function () {
              $scope.showLoadingOverlay = false;
          }, 500);
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


      //Mobile Menu Toggle
    $scope.mobileMenuToggled = false;
    $scope.toggleMobileMenu = function () {
        if ($scope.mobileMenuToggled == false) {
            $('.mobile-menu').animate({ 'left': '0' }, 300);
            $scope.mobileMenuToggled = true;
        }
        else {
            $('.mobile-menu').animate({ 'left': '-260px' }, 300);
            $scope.mobileMenuToggled = false;
        }
    };

    
    // Allows to use branding color with interpolation
    // {{ colorByName('primary') }}
    $scope.colorByName = colors.byName;

    //set current portal
    if ($state.includes('app.design')) {
        $rootScope.app.portal = 'design';
    }
    else {
        $rootScope.app.portal = 'print';
    }

      //get user info
    $scope.user = {};
    $scope.getUserInfo = function () {
        $scope.fetchUserInfoLoading = true;
        $http.get(baseUri + 'Profile/GetProfile')
        .success(function (data, status, headers, config) {
            $scope.user = data;
            $scope.fetchUserInfoLoading = false;
        }).error(function (data, status, headers, config) {
            if (status == 403) {
                window.location = "/Account/Login";
            }
            else {
                toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
            }
            $scope.fetchUserInfoLoading = false;
        });
    };
    $scope.getUserInfo();

      //get show message
    $scope.getUnReadMessages = function () {
        $http.get(baseUri + 'ReceiveMessage/GetAllUnReadMessage')
        .success(function (data, status, headers, config) {
            $scope.messages = data.Data;
            //Convert Text
            _.each($scope.messages, function (item) {
                item.Text2 = $sce.trustAsHtml(item.Text);
            });
            //Open Popup

            if ($scope.messages.length > 0) {
                var modalInstance = $modal.open({
                    templateUrl: '/UnReadMessageModalContent.html',
                    controller: UnReadMessagesCtrl,
                    size: 'lg',
                    resolve: {
                        messages: function () {
                            return _.clone($scope.messages);
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                }, function () {
                });
            }
        }).error(function (data, status, headers, config) {
            if (status == 403) {
                window.location = "/Account/Login";
            }
        });
    };
    var UnReadMessagesCtrl = ['$scope', '$http', '$modalInstance', 'messages', function ($scope, $http, $modalInstance, messages) {
        $scope.messages = messages;

        _.each($scope.messages, function (message) {
            $http.post(baseUri + 'ReceiveMessage/MarkAsRead',
            {
                id: message.Id,
            }).
            success(function (data, status, headers, config) {
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
            });
        });

        $scope.close = function () {
            $modalInstance.dismiss('cancel');
        };
    }];
    $scope.getUnReadMessages();

      //get help text
    $scope.ShowFaqModal = function () {
        var modalInstance = $modal.open({
            templateUrl: '/FAQModalContent.html',
            controller: FAQCtrl,
            size: 'lg'
        });

        modalInstance.result.then(function (result) {
        }, function () {
        });
    };
    var FAQCtrl = ['$scope', '$http', '$modalInstance', function ($scope, $http, $modalInstance) {
        $scope.help = '';

        $scope.fetchLoading = true;
        $http.get(baseUri + 'Home/PublicHelpText')
        .success(function (data, status, headers, config) {
            $scope.help = $sce.trustAsHtml(data);
            $scope.fetchLoading = false;
        }).error(function (data, status, headers, config) {
            if (status == 403) {
                window.location = "/Account/Login";
            }
            else {
                toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
            }
            $scope.fetchLoading = false;
        });

        $scope.close = function () {
            $modalInstance.dismiss('cancel');
        };
    }];
}]);
