App.directive('stepper', function () {
    return {
        require: '?ngModel',
        link: function (scope, elm, attr, ngModel) {
            var obj = {};
            obj.type = 'int';
            obj.allowWheel = true;
            var min = 1;
            var max = 1000;

            if (attr["numberType"] != null && (attr["numberType"] == 'int' || attr["numberType"] == 'float')) {
                obj.type = attr["numberType"];
            }

            if (attr["min"] != null) {
                min = attr["min"];
            }
            if (attr["max"] != null) {
                max = attr["max"];
            }
            obj.limit = [min, max];
            obj.onStep = function (value, up) {
                scope.$apply(function () {
                    ngModel.$setViewValue(value);
                });
            };

            $(elm[0]).stepper(obj);
        }
    };
});