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
            $(element).colorpicker({
                customClass: 'colorpicker-2x',
                container: $(element),
                format: 'hex',
                sliders: {
                    saturation: {
                        maxLeft: 200,
                        maxTop: 200
                    },
                    hue: {
                        maxTop: 200
                    },
                    alpha: {
                        maxTop: 200
                    }
                }
            });

            $(element).colorpicker().on('changeColor.colorpicker', function (event) {
                scope.ngModel = event.color.toHex();
                scope.$apply();
            });
        }
    };
});
