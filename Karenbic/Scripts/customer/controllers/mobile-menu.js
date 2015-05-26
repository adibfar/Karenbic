/**=========================================================
 * Module: mobile-menu.js
 * Top Menu of Application
 =========================================================*/
App.controller('MobileMenuController', ['$rootScope', '$scope', '$state', '$location', '$timeout', 'APP_Portal_Menu',
    function ($rootScope, $scope, $state, $location, $timeout, portalMenu) {

        $scope.itemMenuClicked = function ($event, menuItem) {

            if ($scope.clickedItem == menuItem) {
                if (menuItem.submenu != null && menuItem.submenu.length != 0) {
                    $($event.currentTarget).find('.sub-menu').stop(true, true).slideUp(300);
                    $scope.menuExpanded == false;
                }
                $scope.clickedItem = null;
                return;
            }

            $scope.clickedItem = menuItem;

            if (menuItem.submenu != null && menuItem.submenu.length != 0) {
                if ($scope.menuExpanded == true) {
                    $('.mobile-menu').find('.sub-menu')
                        .not($($event.currentTarget).find('.sub-menu')).stop(true, true).slideUp(200, function () {
                        $($event.currentTarget).find('.sub-menu').stop(true, true).slideDown(200);
                    });
                }
                else {
                    $($event.currentTarget).find('.sub-menu').stop(true, true).slideDown(300);
                    $scope.menuExpanded = true;
                }
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

        $scope.clickedItem = null;
        $scope.menuExpanded = false;

        if ($scope.isDesignPortal() == true) $scope.menuItems = portalMenu.menuItems_design;
        else if ($scope.isPrintPortal() == true) $scope.menuItems = portalMenu.menuItems_print;
    }]);