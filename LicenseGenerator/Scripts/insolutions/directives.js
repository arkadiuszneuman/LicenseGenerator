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

            scope.loadervisible = false;

            scope.click = function () {
                scope.loadervisible = true;
            };
        }
    };
});

app.directive('inLoaderVisible', function () {

    return {
        transclude: true,
        restrict: 'A',
        scope: {
            inLoaderVisible: '=',
            inLoaderBackground: '=?'
        },
        template: '<div ng-class="{\'loader2-background\': inLoaderBackground && inLoaderVisible}"></div><div ng-transclude></div><div ng-show="inLoaderVisible" class="loader2" id="lrd1"></div>',
        link: function (scope, element, attrs) {

            scope.inLoaderBackground = true;
            
            var setLoaderPosition = function () {
                var loader = $(".loader2");

                loader.css({
                    'position': 'absolute',
                    'left': '50%',
                    'top': '50%',
                    'margin-left': -loader.outerWidth() / 2,
                    'margin-top': -loader.outerHeight() / 2,
                });
                //var width = loader.parent().width();
                //var height = loader.parent().height();
                //var top = loader.parent().position().top;
                //var left = loader.parent().position().left;

                //var topToSet = top + (height / 2) - ((loader.height() + 20) / 2);
                //var leftToSet = left + (width / 2) - ((loader.width() + 20) / 2);
                //loader.css({ "top": topToSet + "px", "left": leftToSet + "px" });
            }
            
            scope.$watch('inLoaderVisible', function (newVal, oldVal) {
                if (newVal) {
                    setLoaderPosition();
                }
            });
        }
    };
});