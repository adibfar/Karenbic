/**=========================================================
 * Module: jquery-colorbox.js
 * Initializes the jquery-colorbox plugin
 =========================================================*/

App.directive('jqueryColorbox', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            if(attrs["rel"] == null)
                $(element).colorbox();
            else
                $(element).colorbox({
                    rel: attrs["rel"] 
                });
        }
    };
});