/**=========================================================
 * Module: ddlist-select.js
 * Initializes the ddlist select plugin
 =========================================================*/

App.directive('ddlist', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        scope: {
            list: '=',
            ngModel: '='
        },
        link: function (scope, element, attrs) {
            //on change list
            scope.$watch('list', function () {
                $(element).ddlist('setItemsSource', scope.list);
            }, true);

            //on model change
            scope.$watch('ngModel', function () {
                var i = _.indexOf(scope.list, scope.ngModel);
                if (i != -1 && scope.ngModel.value != $(element).data('ddlist').selectedValue) {
                    $(element).ddslick('select', { index: i });

                    //console.log($(element).data('ddlist').selectedValue);
                    //console.log(scope.ngModel);
                }
            }, true);

            //render element
            $(element).ddlist({
                width: '100%',
                itemsSource: scope.list,
                onSelected: function (data) {
                    scope.ngModel = _.find(scope.list, function (item) {
                        return item.value == data;
                    });
                    scope.$apply();
                }
            });
        }
    };
});