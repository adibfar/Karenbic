App.directive('cropit', function () {
    return {
        restrict: 'A',
        scope: {
            image: '='
        },
        link: function (scope, element, attrs, ngModelCtrl) {
            var options = {
                minZoom: 'fit',
                maxZoom: 5
            };
            $(element).cropit(options);

            scope.$watch("image", function () {
               $(element).cropit('imageSrc', attrs["imageSrc"]);
            }, true);
        }
    }
});