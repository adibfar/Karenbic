App.directive('tableResponsive', function () {
    var linker = function (scope, element, attrs) {
        var $div = $('<div>')
            .css({
                'background-color': '#e2e0e0',
                'border': '1px solid #e2e0e0',
                'cursor': 'pointer',
                'font-family': 'BNAZANB',
                'font-size': '47px',
                'height': '32px',
                'left': '0',
                'line-height': '0',
                'padding': '7px 6px 0 0',
                'position': 'absolute',
                'top': '0',
                'width': '39px'
            })
            .addClass('visible-sm visible-xs')
            .html('...')
            .click(function () {
                $(element).animate({
                    scrollLeft: $(element).scrollLeft() - 100
                }, 'slow');
            });

        $(element)
            .css({
                'position': 'relative'
            })
            .append($div)
            .scroll(function () {
                $div.css({ 'left': $(element).scrollLeft() });
            });
    };

    return {
        restrict: 'C',
        link: linker
    };
});