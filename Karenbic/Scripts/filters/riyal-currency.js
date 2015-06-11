App.filter('riyal', function () {
    return function (str) {
        var separator = ",";
        var output = escape(str).replace(new RegExp(separator, "g"), "");
        var regexp = new RegExp("\\B(\\d{3})(" + separator + "|$)");
        do {
            output = output.replace(regexp, separator + "$1");
        }
        while (output.search(regexp) >= 0)
        return output;
    };
});