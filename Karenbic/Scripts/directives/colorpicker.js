/**=========================================================
 * Module: filestyle.js
 * Initializes the bootstrap color picker plugin
 =========================================================*/

App.directive('colorpicker', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        scope: {
            ngModel: '='
        },
        link: function (scope, element, attrs) {
            var options = $(element).data();

            $(element).colorpicker(options);

            $(element).colorpicker().on('changeColor.colorpicker', function (event) {
                scope.ngModel = event.color.toHex();
                scope.$apply();
            });
        }
    };
});
