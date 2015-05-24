App.directive('nicefileinput', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            $(element).nicefileinput({
                label: ' '
            });
        }
    };
});