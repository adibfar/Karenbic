/**=========================================================
 * Module: chosen-select.js
 * Initializes the chose select plugin
 =========================================================*/

App.directive('chosen', function() {
    var linker = function (scope, element, attrs) {
        var list = attrs['chosen'];

        scope.$watch(list, function () {
            setTimeout(function () {
                element.trigger('liszt:updated');
                element.trigger("chosen:updated");
            }, 100);
            element.trigger('liszt:updated');
            element.trigger("chosen:updated");
        }, true);

        scope.$watch(attrs["ngModel"], function () {
            element.trigger('liszt:updated');
            element.trigger("chosen:updated");
        }, true);

        element.chosen({
            width: attrs['width'],
            disable_search_threshold: 10,
            placeholder_text_single: 'انتخاب کنید',
            placeholder_text_multiple: 'انتخاب کنید'
        });
    };

    return {
        restrict: 'A',
        link: linker
    };
});