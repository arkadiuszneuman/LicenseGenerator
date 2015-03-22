app.filter("distinct", function() {
    return function(data, propertyName) {
        if (angular.isArray(data) && angular.isString(propertyName)) {
            var results = [];
            var keys = {};
            for (var i = 0; i < data.length; ++i) {
                var val = data[i][propertyName];
                if (angular.isUndefined(keys[val])) {
                    keys[val] = true;
                    results.push(val);
                }
            }
        } else {
            return data;
        }
    }
});

app.filter('monthName', [function () {
    return function (monthNumber) {
        if (angular.isNumber(monthNumber)) {
            var monthNames = [
                'Styczeń', 'Luty', 'Marzec', 'Kwiecień', 'Maj', 'Czerwiec',
                'Lipiec', 'Sierpień', 'Wrzesień', 'Październik', 'Listopad', 'Grudzień'
            ];
            return monthNames[monthNumber - 1];
        } else {
            return monthNumber;
        }
    }
}]);