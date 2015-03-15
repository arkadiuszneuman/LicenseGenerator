var secretEmptyKey = '[$empty$]';

app.controller('LicenseGeneratorController', [
    '$scope', 'datepickerPopupConfig', '$filter', '$http', '$timeout', '$upload', 'inDateFormatter', 'inLicenseFormatter',
    function ($scope, datepickerPopupConfig, $filter, $http, $timeout, $upload, inDateFormatter, inLicenseFormatter) {
        $scope.lic = {};
        $scope.lic.company2 = undefined;
        $scope.lic.isNipLikeCompany = true;
        $scope.newestVersion = "";

        new DatePickerCreator().configureDatePicker($scope, datepickerPopupConfig);
        new LicenseGeneratorButtonsCreator().configureButtons($scope, $http, inLicenseFormatter);
        createDefaultLicense($scope, $filter);

        $scope.getClients = function (val) {
            return $http.post(siteUrl + "Home/LoadClients", { clientValue: val }).then(function (response) {
                if (response.data.success) {
                    return response.data.object;
                }
            });
        };

        $scope.getProducts = function (val) {
            return $http.post(siteUrl + "Home/LoadProducts", { licenseName: val }).then(function (response) {
                if (response.data.success) {
                    return response.data.object;
                }
            });
        };

        $scope.onClientSelected = function ($item, $model, $label) {
            $scope.lic.nip = $model.nip;
            $scope.lic.company1 = $model.name;
            $scope.lic.isNipLikeCompany = false;
        };

        $scope.onProductSelected = function ($item, $model, $label) {
            $scope.newestVersion = $model.version;
            $scope.lic.programName = $model.programName;
            $scope.lic.programVersion = "";
        };

        $scope.onNipLostFocus = function () {
            if (!angular.isUndefined($scope.lic.nip) && $scope.lic.isNipLikeCompany) {
                $scope.lic.company1 = $scope.lic.nip;
            }
        };

        $scope.getAddionalInfos = function () {
            var date = inDateFormatter.customFormatDate(new Date($scope.lic.date), "#YYYY#-#MM#-#DD#");
            return [
                "Licencja testowa ważna do " + date,
                "Licencja bezterminowa",
                "Licencja z abonamentem ważnym do " + date];
        };

        $scope.onAddionalInfoFocus = function (e) {
            $timeout(function () {
                $(e.target).trigger('input');
                $(e.target).trigger('change');
            });
        };

        $scope.stateComparator = function (state, viewValue) {
            return viewValue === secretEmptyKey || ('' + state).toLowerCase().indexOf(('' + viewValue).toLowerCase()) > -1;
        };

        $scope.fileSelected = function ($files, $event) {
            var file = $files[0];
            $scope.upload = $upload.upload({
                url: siteUrl + 'Home/LoadLicense',
                data: { objectToUpload: file },
                file: file
            }).success(function (data, status, headers, config) {
                if (data.success === true) {
                    $scope.lic = data.object;
                } else {
                    $scope.message = data.object;
                    $('#alertModal').modal('show');
                }
            });
        };

        $scope.assignNewestVersion = function () {
            $scope.lic.programVersion = $scope.newestVersion;
        };

        $scope.productChanged = function (handler) {
            $http.post(siteUrl + "Home/GetProductNewestVersion", { programName: $scope.lic.name }).then(function (response) {
                if (response.data.success) {
                    $scope.newestVersion = response.data.object;
                    $scope.lic.programVersion = "";
                    handler();
                } else {
                    $scope.newestVersion = "";
                }
            });
        };

        $scope.init = function (licenseProduct) {
            if (!angular.isUndefined(licenseProduct) && licenseProduct != null) {
                $scope.lic = licenseProduct.license;
                $scope.lic.name = licenseProduct.product;
                $scope.lic.programName = licenseProduct.product.programName;
                $scope.newestVersion = licenseProduct.product.version;
                $scope.lic.isNipLikeCompany = $scope.lic.company1 == $scope.lic.nip;
            }
        };
    }]);

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
    };

    DropFileConfigurator.prototype.handleDragOver = function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        evt.dataTransfer.dropEffect = 'move';
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
    LicenseGeneratorButtonsCreator.prototype.getLicenseName = function (license) {
        var licenseName = license.name + "_" + license.nip;

        if (license.partnernip != null) {
            licenseName += "_" + license.partnernip;
        }

        return licenseName;
    };

    LicenseGeneratorButtonsCreator.prototype.configureButtons = function ($scope, $http, inLicenseFormatter) {
        var that = this;

        $scope.generateLicense = function (lic) {
            var license = inLicenseFormatter.FormatLicense(lic);

            $.post(siteUrl + "Home/GenerateLicense", { licenseViewModel: license }, function (result) {
                saveToDisk(siteUrl + result, that.getLicenseName(license) + ".lic");
            });
        };

        $scope.generateTxtLicense = function (lic) {
            var license = inLicenseFormatter.FormatLicense(lic);

            $("#btnGenerateLicense").button("loading");
            $.post(siteUrl + "Home/GenerateLicense", { licenseViewModel: license }, function (result) {
                $("#btnGenerateLicense").button("reset");
                if (result.success === true) {
                    saveToDisk(siteUrl + result.object, that.getLicenseName(license) + "S.txt");
                }
            });
        };

        $scope.generateDecryptedLicense = function (lic) {
            var license = inLicenseFormatter.FormatLicense(lic);

            $("#btnGenerateLicense").button("loading");
            $.post(siteUrl + "Home/GenerateDecryptedLicense", { licenseViewModel: license }, function (result) {
                $("#btnGenerateLicense").button("reset");
                var blob = new Blob([result], { type: "text/plain;charset=utf-8" });
                saveAs(blob, that.getLicenseName(license) + ".txt");
            });
        };

        $scope.generateZippedLicense = function (lic) {
            var license = inLicenseFormatter.FormatLicense(lic);

            $("#btnGenerateLicense").button("loading");
            $.post(siteUrl + "Home/GenerateZippedLicense", { licenseViewModel: license }, function (result) {
                $("#btnGenerateLicense").button("reset");
                saveToDisk(siteUrl + result, that.getLicenseName(license) + ".zip");
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

        datepickerPopupConfig.currentText = 'Dzisiaj';
        datepickerPopupConfig.clearText = 'Licencja nieograniczona';
        datepickerPopupConfig.closeText = 'Zamknij';
    };
    return DatePickerCreator;
})();
