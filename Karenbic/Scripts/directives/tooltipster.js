/**=========================================================
 * Module: tooltipster.js
 * Initializes the tooltips plugin
 =========================================================*/

App.directive('tooltipster', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            if (attrs["tooltipsterTitle"] != undefined && attrs["tooltipsterTitle"] != null) {
                $(element).tooltipster({
                    content: $(attrs["tooltipsterTitle"]),
                    theme: 'tooltipster-light'
                });
            }
            else {
                $(element).tooltipster({
                    theme: 'tooltipster-light'
                });
            }
        }
    };
});