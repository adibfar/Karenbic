/**=========================================================
 * Module: top-menu.js
 * Top Menu of Application
 =========================================================*/
App.controller('TopMenuController', ['$rootScope', '$scope', '$state', '$location', '$timeout', 'APP_Portal_Menu',
    function ($rootScope, $scope, $state, $location, $timeout, portalMenu) {

        $scope.itemMenuClicked = function ($event, menuItem) {

            if ($scope.clickedItem == menuItem) {
                if ($scope.topHeaderExpand == true) {
                    
                    $('.top-header').find('.sub-menu').hide();

                    $('.top-header').stop(true, true).animate({ 'height': 92 }, 300);
                }
                $scope.topHeaderExpand = false;
                $scope.clickedItem = null;
                return;
            }
            $scope.clickedItem = menuItem;

            if ($scope.topHeaderExpand == true &&
                menuItem.submenu != null && menuItem.submenu.length != 0) {
                
                $('.top-header').find('.sub-menu').hide();

                $('.top-header').stop(true, true).animate({ 'height': 92 }, function () {

                    $('.top-header').stop(true, true).animate({ 'height': 125 }, 300, function () {
                        $($event.currentTarget).find('.sub-menu').fadeIn();
                    });

                });
            }
            else if ($scope.topHeaderExpand == true &&
                (menuItem.submenu == null || menuItem.submenu.length == 0)) {
                
                $('.top-header').find('.sub-menu').hide();

                $('.top-header').stop(true, true).animate({ 'height': 92 }, 300, function () {
                    $scope.topHeaderExpand = false;
                });
            }
            else if ($scope.topHeaderExpand == false &&
                menuItem.submenu != null && menuItem.submenu.length != 0) {
                
                $('.top-header').stop(true, true).animate({ 'height': 125 }, 300, function () {
                    $($event.currentTarget).find('.sub-menu').fadeIn();
                });

                $scope.topHeaderExpand = true;
            }
            else if ($scope.topHeaderExpand == false &&
                (menuItem.submenu == null || menuItem.submenu.length == 0)) {
                
            }
        };

        $scope.isActive = function (item) {
            if (!item) return;

            if (!item.sref || item.sref == '#') {
                var foundActive = false;

                if (item.submenu != null) {
                    _.each(item.submenu, function (subMenuItem) {
                        if ($scope.isActive(subMenuItem)) foundActive = true;
                    });
                }
                if (item.activedmenu != null) {
                    _.each(item.activedmenu, function (activedMenuItem) {
                        if ($scope.isActive(activedMenuItem)) foundActive = true;
                    });
                }

                return foundActive;
            }
            else
                return $state.is(item.sref);
        };

        $scope.topHeaderExpand = false;
        $scope.clickedItem = null;

        if ($scope.isDesignPortal() == true) $scope.menuItems = portalMenu.menuItems_design;
        else if ($scope.isPrintPortal() == true) $scope.menuItems = portalMenu.menuItems_print;

        //configuration notification
        $.connection.hub.logging = true;

        var connection = $.hubConnection();
        connection.url = 'http://localhost:22182/signalr';
        var proxy = connection.createHubProxy('customerNotification');

        _.each($scope.menuItems, function (menuItem) {
            if (menuItem.notification_get_fn)
                proxy.on(menuItem.notification_get_fn, function (count) {
                    menuItem.notification_count = count;
                    $scope.$apply();
                });

            if (menuItem.notification_new_fn)
                proxy.on(menuItem.notification_new_fn, function () {
                    menuItem.notification_count++;
                    $scope.$apply();
                });

            if (menuItem.notification_minus_fn)
                proxy.on(menuItem.notification_minus_fn, function () {
                    menuItem.notification_count--;
                    $scope.$apply();
                });

            _.each(menuItem.submenu, function (subMenuItem) {

                if (subMenuItem.notification_get_fn)
                    proxy.on(subMenuItem.notification_get_fn, function (count) {
                        subMenuItem.notification_count = count;
                        $scope.$apply();
                    });

                if (subMenuItem.notification_new_fn)
                    proxy.on(subMenuItem.notification_new_fn, function () {
                        subMenuItem.notification_count++;
                        $scope.$apply();
                    });

                if (subMenuItem.notification_minus_fn)
                    proxy.on(subMenuItem.notification_minus_fn, function () {
                        subMenuItem.notification_count--;
                        $scope.$apply();
                    });
            });
        });

        connection.start().done(function () {
            _.each($scope.menuItems, function (menuItem) {
                if (menuItem.notification_get_fn) proxy.invoke(menuItem.notification_get_fn);

                _.each(menuItem.submenu, function (subMenuItem) {
                    if (subMenuItem.notification_get_fn) proxy.invoke(subMenuItem.notification_get_fn);
                });
            });
        });
}]);