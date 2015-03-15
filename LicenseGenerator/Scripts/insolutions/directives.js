var INTEGER_REGEXP = /^\-?\d+$/;
app.directive('integer', function () {
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$validators.integer = function (modelValue, viewValue) {
                if (ctrl.$isEmpty(modelValue)) {
                    // consider empty models to be valid
                    return true;
                }

                if (INTEGER_REGEXP.test(viewValue)) {
                    // it is valid
                    return true;
                }

                // it is invalid
                return false;
            };
        }
    };
});

var NIP_REGEXP = /^((\d{3}[- ]\d{3}[- ]\d{2}[- ]\d{2})|(\d{3}[- ]\d{2}[- ]\d{2}[- ]\d{3}))$/;
app.directive('nip', function () {
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$validators.integer = function (modelValue, viewValue) {
                if (ctrl.$isEmpty(modelValue)) {
                    // consider empty models to be valid
                    return true;
                }


                if (NIP_REGEXP.test(viewValue)) {
                    // it is valid
                    return true;
                }

                if (viewValue.length == 10 && INTEGER_REGEXP.test(viewValue)) {
                    // it is valid
                    return true;
                }

                // it is invalid
                return false;
            };
        }
    };
});

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

app.directive('inMailSenderWindow', ['$http', 'inLicenseFormatter', 'toastr', function ($http, inLicenseFormatter, toastr) {

    return {
        restrict: 'E',
        templateUrl: siteUrl + '/Static/MailSender.html',
        link: function (scope, element, attrs) {
            $('#mailsender').on('shown.bs.modal', function () {
                $('#emails').focus();
            });

            scope.loadervisible = false;

            scope.sendMail = function (mail, lic) {
                scope.loadervisible = true;
                lic = inLicenseFormatter.FormatLicense(lic);

                $http.post(siteUrl + "MailSend/Send", { mail: mail, license: lic })
                      .then(function (response) {
                          if (response.data.success) {
                              $('#mailsender').modal('hide');
                              scope.loadervisible = false;
                              toastr.success('E-mail został poprawnie wysłany.');
                          } else {
                              toastr.error(response.data.object, 'Błąd podczas próby wysłania e-maila.');
                              scope.loadervisible = false;
                          }
                      });
            };
        }
    };
}]);

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
                    'margin-top': -loader.outerHeight() / 2
                });
            }

            scope.$watch('inLoaderVisible', function (newVal, oldVal) {
                if (newVal) {
                    setLoaderPosition();
                }
            });
        }
    };
});