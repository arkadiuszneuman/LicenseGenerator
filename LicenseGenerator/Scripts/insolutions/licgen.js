var app = angular.module('licensegenerator', ['ui.bootstrap']);

angular.module('licensegenerator').controller('LicenseGeneratorController', function ($scope, datepickerPopupConfig, $filter, $http) {
    $scope.lic = {};

    new DatePickerCreator().configureDatePicker($scope, datepickerPopupConfig);
    new LicenseGeneratorButtonsCreator().configureButtons($scope);
    createDefaultLicense($scope, $filter);

    //new DropFileConfigurator().configureDropFiles($scope);
    //$scope.lic = { name: 'Arek' };
    $scope.getClients = function (val) {
        return $http.post(siteUrl + "Home/LoadClients", { clientValue: val }).then(function (response) {
            return response.data;
        });
    };

    $scope.onClientSelected = function ($item, $model, $label) {
        $scope.lic.nip = $model.Nip;
    };
});

function createDefaultLicense($scope, $filter) {
    var date = new Date();
    date.setMonth(date.getMonth() + 1);

    $scope.lic.date = $filter('date')(date, 'yyyy-MM-dd');
}
;

var DropFileConfigurator = (function () {
    function DropFileConfigurator() {
    }
    DropFileConfigurator.prototype.handleFileSelect = function (evt) {
        evt.stopPropagation();
        evt.preventDefault();

        var files = evt.dataTransfer.files;
        var file = files[0];

        var reader = new FileReader();

        reader.onload = (function (theFile) {
            return function (e) {
                var lines = e.target.result.split("\n");
                DropFileConfigurator.$scope.lic = { name: lines[0] };
            };
        })(file);

        reader.readAsText(file);
        //// files is a FileList of File objects. List some properties.
        //var output = [];
        //for (var i = 0, f; f = files[i]; i++) {
        //    output.push('<li><strong>', f.name, '</strong> (', f.type || 'n/a', ') - ',
        //        f.size, ' bytes, last modified: ',
        //        f.lastModifiedDate ? f.lastModifiedDate.toLocaleDateString() : 'n/a',
        //        '</li>');
        //}
        //document.getElementById('list').innerHTML = '<ul>' + output.join('') + '</ul>';
    };

    DropFileConfigurator.prototype.handleDragOver = function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        evt.dataTransfer.dropEffect = 'move'; // Explicitly show this is a copy.
    };

    DropFileConfigurator.prototype.configureDropFiles = function ($scope) {
        DropFileConfigurator.$scope = $scope;
        var dropZone = document.getElementById('drop_zone');
        dropZone.addEventListener('dragover', this.handleDragOver, false);
        dropZone.addEventListener('drop', this.handleFileSelect, false);
    };
    return DropFileConfigurator;
})();

var LicenseGeneratorButtonsCreator = (function () {
    function LicenseGeneratorButtonsCreator() {
    }
    LicenseGeneratorButtonsCreator.prototype.customFormatDate = function (dateObject, formatString) {
        var YYYY, YY, MMMM, MMM, MM, M, DDDD, DDD, DD, D, hhh, hh, h, mm, m, ss, s, ampm, AMPM, dMod, th;
        YY = ((YYYY = dateObject.getFullYear()) + "").slice(-2);
        MM = (M = dateObject.getMonth() + 1) < 10 ? ('0' + M) : M;
        MMM = (MMMM = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"][M - 1]).substring(0, 3);
        DD = (D = dateObject.getDate()) < 10 ? ('0' + D) : D;
        DDD = (DDDD = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"][dateObject.getDay()]).substring(0, 3);
        th = (D >= 10 && D <= 20) ? 'th' : ((dMod = D % 10) == 1) ? 'st' : (dMod == 2) ? 'nd' : (dMod == 3) ? 'rd' : 'th';
        formatString = formatString.replace("#YYYY#", YYYY).replace("#YY#", YY).replace("#MMMM#", MMMM).replace("#MMM#", MMM).replace("#MM#", MM).replace("#M#", M).replace("#DDDD#", DDDD).replace("#DDD#", DDD).replace("#DD#", DD).replace("#D#", D).replace("#th#", th);

        h = (hhh = dateObject.getHours());
        if (h == 0)
            h = 24;
        if (h > 12)
            h -= 12;
        hh = h < 10 ? ('0' + h) : h;
        AMPM = (ampm = hhh < 12 ? 'am' : 'pm').toUpperCase();
        mm = (m = dateObject.getMinutes()) < 10 ? ('0' + m) : m;
        ss = (s = dateObject.getSeconds()) < 10 ? ('0' + s) : s;
        return formatString.replace("#hhh#", hhh).replace("#hh#", hh).replace("#h#", h).replace("#mm#", mm).replace("#m#", m).replace("#ss#", ss).replace("#s#", s).replace("#ampm#", ampm).replace("#AMPM#", AMPM);
    };

    LicenseGeneratorButtonsCreator.prototype.createValidLicense = function (lic) {
        var license = angular.copy(lic);
        if (license.date != null) {
            license.date = this.customFormatDate(new Date(lic.date), "#YYYY#-#MM#-#DD# #hh#:#mm#:#ss#");
        }

        return license;
    };

    LicenseGeneratorButtonsCreator.prototype.getLicenseName = function (license) {
        var licenseName = license.name + "_" + license.nip;

        if (license.partnernip != null) {
            licenseName += "_" + license.partnernip;
        }

        return licenseName;
    };

    LicenseGeneratorButtonsCreator.prototype.configureButtons = function ($scope) {
        var that = this;

        $scope.generateLicense = function (lic) {
            var license = that.createValidLicense(lic);

            $.post(siteUrl + "Home/GenerateLicense", { license: license }, function (result) {
                //var blob = new Blob([result], { type: "example/binary" });
                saveToDisk(siteUrl + result, that.getLicenseName(license) + ".lic");
            });
        };

        $scope.generateTxtLicense = function (lic) {
            var license = that.createValidLicense(lic);

            $.post(siteUrl + "Home/GenerateLicense", { license: license }, function (result) {
                saveToDisk(siteUrl + result, that.getLicenseName(license) + "S.txt");
            });
        };

        $scope.generateDecryptedLicense = function (lic) {
            var license = that.createValidLicense(lic);

            $.post(siteUrl + "Home/GenerateDecryptedLicense", { license: license }, function (result) {
                var blob = new Blob([result], { type: "text/plain;charset=utf-8" });
                saveAs(blob, that.getLicenseName(license) + ".txt");
            });
        };
    };
    return LicenseGeneratorButtonsCreator;
})();

var DatePickerCreator = (function () {
    function DatePickerCreator() {
    }
    DatePickerCreator.prototype.configureDatePicker = function ($scope, datepickerPopupConfig) {
        $scope.format = 'dd MMMM yyyy';

        $scope.clear = function () {
            $scope.lic.date = null;
        };

        $scope.toggleMin = function () {
            $scope.minDate = $scope.minDate ? null : new Date();
        };
        $scope.toggleMin();

        $scope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.opened = true;
        };

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        // TRANSLATION
        datepickerPopupConfig.currentText = 'Dzisiaj';
        datepickerPopupConfig.clearText = 'Licencja nieograniczona';
        datepickerPopupConfig.closeText = 'Zamknij';
    };
    return DatePickerCreator;
})();

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
//# sourceMappingURL=licgen.js.map
