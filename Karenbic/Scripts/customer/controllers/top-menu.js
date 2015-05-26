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
}]);