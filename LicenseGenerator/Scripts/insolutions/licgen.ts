declare var siteUrl;
declare function saveAs(blob, text);
declare function saveToDisk(fileURL, fileName);

declare var ngTableParams;
declare var data;

var secretEmptyKey = '[$empty$]';

app.controller('LicenseGeneratorController', ['$scope', 'datepickerPopupConfig', '$filter', '$http', '$timeout', '$upload', 'inDateFormatter', 'inLicenseFormatter',
    function ($scope, datepickerPopupConfig, $filter, $http, $timeout, $upload, inDateFormatter, inLicenseFormatter) {
        $scope.lic = {};
        $scope.lic.isForClient = false;
        $scope.lic.company2 = undefined;
        $scope.lic.isNipLikeCompany = true;
        $scope.newestVersion = "";

        new DatePickerCreator().configureDatePicker($scope, datepickerPopupConfig);
        new LicenseGeneratorButtonsCreator().configureButtons($scope, $http, inLicenseFormatter);
        createDefaultLicense($scope, $filter);

        $scope.getClients = function (val) {
            return $http.post(siteUrl + "Home/LoadClients", { clientValue: val })
                .success(function (response) {
                    if (response.success) {
                        return response.object;
                    }
                }).error(function (response) {
                    console.log(response);
                });
        };

        $scope.getProducts = function (val) {
            return $http.post(siteUrl + "Home/LoadProducts", { licenseName: val })
                .success(function (response) {
                    if (response.success) {
                        return response.object;
                    }
                }).error(function(response) {
                    console.log(response);
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
        }

        function getDemoLicenseDescription() {
            return "Licencja testowa ważna do ";
        }

        function getVersionLicenseDescription() {
            return "Licencja bezterminowa";
        }

        function getDateLicenseDescription() {
            return "Licencja z abonamentem ważnym do ";
        }

        $scope.getAddionalInfos = function () {
            var date = inDateFormatter.customFormatDate(new Date($scope.lic.date), "#YYYY#-#MM#-#DD#");
            return [getDemoLicenseDescription() + date,
                getVersionLicenseDescription(),
                getDateLicenseDescription() + date];
        }

        $scope.$watch('lic.date', function (newVal, OldVal) {
            var date = inDateFormatter.customFormatDate(new Date(newVal), "#YYYY#-#MM#-#DD#");

            if ($scope.lic.company2) {
                if ($scope.lic.company2.indexOf(getDemoLicenseDescription()) == 0) {
                    $scope.lic.company2 = getDemoLicenseDescription() + date;
                } else if ($scope.lic.company2.indexOf(getDateLicenseDescription()) == 0) {
                    $scope.lic.company2 = getDateLicenseDescription() + date;
                }
            }
        });

        $scope.onAddionalInfoFocus = function (e) {
            $timeout(function () {
                $(e.target).trigger('input');
                $(e.target).trigger('change'); // for IE
            });
        };

        $scope.stateComparator = function (state, viewValue) {
            return viewValue === secretEmptyKey || ('' + state).toLowerCase().indexOf(('' + viewValue).toLowerCase()) > -1;
        };

        $scope.fileSelected = function ($files, $event) {
            var file = $files[0];
            $scope.upload = $upload.upload({
                url: siteUrl + 'Home/LoadLicense', // upload.php script, node.js route, or servlet url
                //method: 'POST' or 'PUT',
                //headers: {'Authorization': 'xxx'}, // only for html5
                //withCredentials: true,
                data: { objectToUpload: file },
                file: file, // single file or a list of files. list is only for html5
                //fileName: 'doc.jpg' or ['1.jpg', '2.jpg', ...] // to modify the name of the file(s)
                //fileFormDataName: myFile, // file formData name ('Content-Disposition'), server side request form name
                // could be a list of names for multiple files (html5). Default is 'file'
                //formDataAppender: function(formData, key, val){}  // customize how data is added to the formData. 
                // See #40#issuecomment-28612000 for sample code

            }).success(function (data, status, headers, config) {
                    if (data.success === true) {
                        $scope.lic = data.object;
                    } else {
                        $scope.message = data.object;
                        $('#alertModal').modal('show');
                    }
                });
        }

        $scope.assignNewestVersion = function () {
            $scope.lic.programVersion = $scope.newestVersion;
        }

        $scope.productChanged = function (handler) {
            $http.post(siteUrl + "Home/GetProductNewestVersion", { programName: $scope.lic.name })
                .then(function (response) {
                    if (response.data.success) {
                        $scope.newestVersion = response.data.object;
                        $scope.lic.programVersion = "";
                        handler();
                    } else {
                        $scope.newestVersion = "";
                    }
                });
        }

        $scope.init = function (licenseProduct) {
            if (!angular.isUndefined(licenseProduct) && licenseProduct != null) {
                $scope.lic = licenseProduct.license;
                $scope.lic.name = licenseProduct.product;
                $scope.lic.programName = licenseProduct.product.programName;
                $scope.newestVersion = licenseProduct.product.version;
                $scope.lic.isNipLikeCompany = $scope.lic.company1 == $scope.lic.nip;
            }
        }
    }]);

function createDefaultLicense($scope, $filter) {
    var date = new Date();

    date.setDate(date.getDate() + 14);
    $scope.lic.date = $filter('date')(date, 'yyyy-MM-dd');
};

class DropFileConfigurator {
    private static $scope;

    private handleFileSelect(evt) {
        evt.stopPropagation();
        evt.preventDefault();

        var files = evt.dataTransfer.files; // FileList object.
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
    }

    private handleDragOver(evt) {
        evt.stopPropagation();
        evt.preventDefault();
        evt.dataTransfer.dropEffect = 'move'; // Explicitly show this is a copy.
    }

    configureDropFiles($scope) {

        DropFileConfigurator.$scope = $scope;
        var dropZone = document.getElementById('drop_zone');
        dropZone.addEventListener('dragover', this.handleDragOver, false);
        dropZone.addEventListener('drop', this.handleFileSelect, false);
    }
}

class LicenseGeneratorButtonsCreator {

    private getLicenseName(license) {
        var licenseName = license.name + "_" + license.nip;

        if (license.partnernip != null) {
            licenseName += "_" + license.partnernip;
        }

        return licenseName;
    }

    public configureButtons($scope, $http, inLicenseFormatter) {
        var that = this;

        $scope.generateLicense = function (lic) {

            var license = inLicenseFormatter.FormatLicense(lic);

            $.post(siteUrl + "Home/GenerateLicense", { licenseViewModel: license }, function (result) {
                //var blob = new Blob([result], { type: "example/binary" });
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
    }
}

class DatePickerCreator {
    configureDatePicker($scope, datepickerPopupConfig) {
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

            $scope.opened = false;
            $scope.opened = true;
        };

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1,
        };

        // TRANSLATION
        datepickerPopupConfig.currentText = 'Dzisiaj';
        datepickerPopupConfig.clearText = 'Licencja nieograniczona';
        datepickerPopupConfig.closeText = 'Zamknij';
    }
}