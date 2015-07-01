/**=========================================================
 * Module: image-scale.js
 * Initializes the Image Scale plugin
 =========================================================*/

App.directive('imagescale', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {

            $(element).imageScale({
                scale: 'best-fit',
                align: 'center'
            });

        }
    };
});