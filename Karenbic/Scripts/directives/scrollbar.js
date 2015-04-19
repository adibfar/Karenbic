App.directive('scrollbar', function () {
    return {
        restrict: 'A',
        scope: {
            scrollbarEventOnTotalScroll: '&'
        },
        link: function (scope, elm, attr, ngModel) {

            var option = {
                theme: "dark-thin",
                scrollButtons: {
                    enable: false
                },
                advanced: {
                    autoScrollOnFocus: false,
                    updateOnContentResize: true
                },
                callbacks: {

                }
            };

            if (scope.scrollbarEventOnTotalScroll != null) {
                option.callbacks.onTotalScroll = function () {
                    scope.scrollbarEventOnTotalScroll();
                };
            }

            $(elm[0]).mCustomScrollbar(option);
        }
    };
});