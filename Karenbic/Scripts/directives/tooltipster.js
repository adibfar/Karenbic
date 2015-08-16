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
                    theme: 'tooltipster-light',
                    trigger: 'custom'
                }).on('focus', function () {
                    $(this).tooltipster('show');
                }).on('blur', function () {
                    $(this).tooltipster('hide');
                }).on('hover', function () {
                    $(this).tooltipster('show');
                }).on('mouseleave', function () {
                    if ($(element).is(':focus') == false)
                        $(this).tooltipster('hide');
                });
            }
            else {
                $(element).tooltipster({
                    theme: 'tooltipster-light'
                }).on('focus', function () {
                    $(this).tooltipster('show');
                }).on('blur', function () {
                    $(this).tooltipster('hide');
                }).on('hover', function () {
                    $(this).tooltipster('show');
                }).on('mouseleave', function () {
                    if ($(element).is(':focus') == false)
                        $(this).tooltipster('hide');
                });
            }
        }
    };
});