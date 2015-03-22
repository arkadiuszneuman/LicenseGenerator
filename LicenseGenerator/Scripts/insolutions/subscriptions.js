var app = angular.module('licensegenerator');

app.controller('SubscriptionsController', ['$scope', '$http', '$filter', 'ngTableParams', function ($scope, $http, $filter, ngTableParams) {

    $scope.isLoading = true;
    $scope.filter = "";
    $scope.selectedYear = "Wszystkie lata";

    $scope.tableParams = new ngTableParams({
        page: 1,            // show first page
        count: 15          // count per page
    }, {
        counts: [],
        getData: function ($defer, params) {

            $scope.isLoading = true;
            $http.post('Subscriptions/LoadSubscriptions', { filter: $scope.filter, page: params.page(), countPerPage: params.count() }).
                success(function (data, status, headers, config) {

                    $scope.licenses = data.licenses;
                    $scope.years = data.years;
                    $scope.years.push("Wszystkie lata");
                    $scope.months = data.months;

                    params.total(data.count); // set total for recalc pagination
                    $defer.resolve($scope.licenses);

                    $scope.isLoading = false;
                });
        }
    });

    $scope.filterChanged = function (filter) {
        $scope.filter = filter;
        $scope.tableParams.reload();
    }

    $scope.setYear = function (year) {
        $scope.selectedYear = year;
    }
}]);