/**=========================================================
 * Module: chosen-select.js
 * Initializes the chose select plugin
 =========================================================*/

App.directive('chosen', function() {
    var linker = function (scope, element, attrs) {
        var list = attrs['chosen'];

        scope.$watch(list, function () {
            element.trigger('liszt:updated');
            element.trigger("chosen:updated");
        }, true);

        scope.$watch(attrs["ngModel"], function () {
            element.trigger('liszt:updated');
            element.trigger("chosen:updated");
        }, true);

        element.chosen({
            width: attrs['width'],
            disable_search_threshold: 10
        });
    };

    return {
        restrict: 'A',
        link: linker
    };
});