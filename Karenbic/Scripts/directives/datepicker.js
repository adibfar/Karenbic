App.directive('jqueryDatepicker', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        scope: {
            ngModel: '='
        },
        link: function (scope, element, attrs, ngModelCtrl) {
            $(function () {
                $(element).datepicker({
                    dateFormat: 'yy/mm/dd',
                    onSelect: function (date) {
                        scope.ngModel = date;
                        scope.$apply();
                    }
                }).change(function () {
                    validateDatepicker(this, 1);
                });;
            });
        }
    }
});

function validateDatepicker(target, autocorrect) {
    function getYMDValue(ymd, parts, dateFormat) {
        return parseInt(parts[parseInt(dateFormat.replace(/[\/]/g, '').indexOf(ymd) / 2)], 10);
    }
    function simpleValidation() {
        try {
            dp.Date = dp._get(inst, 'calendar');
            var parsedDate = dp.parseDate(dateFormat, target.value, inst, inst.settings);
            if (!dp._isInRange(inst, parsedDate)) target.value = '';
        } catch (ex) {
            target.value = '';
        }
    }

    var dp = jQuery.datepicker;
    var inst = dp._getInst(target);
    var dateFormat = dp._get(inst, 'dateFormat');
    if (!autocorrect || (dateFormat != 'dd/mm/yy' && dateFormat != 'mm/dd/yy' && dateFormat != 'yy/mm/dd')) {
        simpleValidation();
        return;
    }
    var firstLength = dateFormat[0] == 'y' ? 4 : 2;
    var splitted = target.value.replace(/[,.-]/g, '/').split('/');
    if (splitted.length == 1 && splitted[0].length > firstLength) {
        splitted[1] = splitted[0].substr(firstLength);
        splitted[0] = splitted[0].substr(0, firstLength);
        if (splitted[1].length > 2) {
            splitted[2] = splitted[1].substr(2);
            splitted[1] = splitted[1].substr(0, 2);
        }
    }
    var d = getYMDValue('d', splitted, dateFormat) || 0;
    var m = getYMDValue('m', splitted, dateFormat) || 0;
    var y = getYMDValue('y', splitted, dateFormat) || 0;
    var value = '';
    if (y || m || d) {
        var t;
        if (y <= 31 && d > 31) { t = d; d = y; y = t; }
        if (d <= 12 && m > 12) { t = d; d = m; m = t; }
        var calendar = dp._get(inst, 'calendar') || Date;
        var shortYearCutoff = dp._get(inst, 'shortYearCutoff') || 99;
        var now = new calendar();
        var thisYear = now.getFullYear();
        if (d == 0 && y != 0) { d = y; y = 0; }
        if (y <= 0 || y > 9999) y = thisYear;
        if (y < 100) y += thisYear - thisYear % 100 + (y <= shortYearCutoff ? 0 : -100);
        m = m || (now.getMonth() + 1);
        d = d || now.getDate();
        var date = new calendar(y, m - 1, d);
        var min = dp._get(inst, 'minDate');
        var max = dp._get(inst, 'maxDate');
        if (min && dp._compareDate(date, '<', min)) date = min;
        if (max && dp._compareDate(date, '>', max)) date = max;
        if (min && max && dp._compareDate(min, '>', max)) {
            value = '';
        } else {
            d = date.getDate();
            m = date.getMonth() + 1;
            y = date.getFullYear();
            if (d < 10) d = '0' + d;
            if (m < 10) m = '0' + m;
            if (y < 1000) y = '0' + y;
            value = dateFormat.replace('dd', d).replace('mm', m).replace('yy', y);
        }
    }
    target.value = value;
    simpleValidation();
}

function fixPersianString(s) {
    return !s ? null : s.replace(/\u0660/g, '\u06F0').replace(/\u0661/g, '\u06F1').replace(/\u0662/g, '\u06F2').replace(/\u0663/g, '\u06F3').replace(/\u0664/g, '\u06F4').replace(/\u0665/g, '\u06F5').replace(/\u0666/g, '\u06F6').replace(/\u0667/g, '\u06F7').replace(/\u0668/g, '\u06F8').replace(/\u0669/g, '\u06F9').replace(/\u0643/g, '\u06A9').replace(/\u0649/g, '\u06CC').replace(/\u064A/g, '\u06CC').replace(/\u06C0/g, '\u0647\u0654');
}