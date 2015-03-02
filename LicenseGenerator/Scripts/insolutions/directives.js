app.directive('emptyTypeahead', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, modelCtrl) {
            // this parser run before typeahead's parser
            modelCtrl.$parsers.unshift(function (inputValue) {
                var value = (inputValue ? inputValue : secretEmptyKey); // replace empty string with secretEmptyKey to bypass typeahead-min-length check
                modelCtrl.$viewValue = value; // this $viewValue must match the inputValue pass to typehead directive
                return value;
            });

            // this parser run after typeahead's parser
            modelCtrl.$parsers.push(function (inputValue) {
                return inputValue === secretEmptyKey ? '' : inputValue; // set the secretEmptyKey back to empty string
            });
        }
    }
});

app.directive('inMailSenderWindow', function () {
    return {
        restrict: 'E',
        templateUrl: siteUrl + '/Static/MailSender.html',
        link: function (scope, element, attrs) {
            $('#mailsender').on('shown.bs.modal', function () {
                $('#emails').focus();
            });
        }
    };
});

app.directive('inLoader', function () {

    return {
        transclude: true,
        template: '<span ng-transclude></span><div class="loader2" id="lrd1"></div>',
        link: function (scope, element, attrs) {
            scope.list[0] // scope.list is the jqlite element, 
            // scope.list[0] is the native dom element

        }
    };
});