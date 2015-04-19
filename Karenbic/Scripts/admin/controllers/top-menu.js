/**=========================================================
 * Module: top-menu.js
 * Top Menu of Application
 =========================================================*/
App.controller('TopMenuController', ['$rootScope', '$scope', '$state', '$location', '$timeout',
    function ($rootScope, $scope, $state, $location, $timeout) {

        $scope.itemMenuClicked = function ($event) {

            if ($($event.currentTarget).hasClass('clicked')) return;

            $('.top-header').find('.menu-item').removeClass('clicked');
            $($event.currentTarget).addClass('clicked');

            if($('.top-header').hasClass('expand') == true && 
                $($event.currentTarget).find('.sub-menu').length != 0) {

                $('.top-header').find('.sub-menu').hide();

                $('.top-header').stop(true, true).animate({ 'height': 92 }, function () {

                    $('.top-header').stop(true, true).animate({ 'height': 125 }, 300, function () {
                        $($event.currentTarget).find('.sub-menu').fadeIn();
                    });

                });

            }

            else if ($('.top-header').hasClass('expand') == true &&
                $($event.currentTarget).find('.sub-menu').length == 0) {

                $('.top-header').find('.sub-menu').hide();

                $('.top-header').stop(true, true).animate({ 'height': 92 }, 300, function () {
                    $('.top-header').removeClass('expand');
                });

            }

            else if($('.top-header').hasClass('expand') == false && 
                $($event.currentTarget).find('.sub-menu').length != 0) {

                $('.top-header').stop(true, true).animate({ 'height': 125 }, 300, function () {
                    $($event.currentTarget).find('.sub-menu').fadeIn();
                });

                $('.top-header').addClass('expand');

            }

            else if ($('.top-header').hasClass('expand') == false &&
                $($event.currentTarget).find('.sub-menu').length == 0) {

            }
        };

        $scope.isActive = function (uri) {
            if ($state.is(uri)) {
                return true;
            }
            return false;
        };
}]);