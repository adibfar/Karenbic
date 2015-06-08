/**=========================================================
 * Module: tooltipster.js
 * Initializes the tooltips plugin
 =========================================================*/

App.directive('tooltipster', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            $(element).tooltipster({
                content: $(attrs["tooltipsterTitle"]),
                theme: 'tooltipster-light'
            });
        }
    };
});
