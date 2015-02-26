app.controller('MailController', ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {
    
    $('#mailsender').on('shown.bs.modal', function () {
        $('#emails').focus();
    })

    $scope.initMail = function () {
        $scope.mail = {};

        var programName = "WPISZNAZWĘPROGRAMU";
        if (angular.isUndefined($scope.lic.programName) == false) {
            programName = $scope.lic.programName;
        }
        else if (angular.isUndefined($scope.lic.name) == false) {
            programName = $scope.lic.name;
        }
        $scope.mail.title = "Licencja dla programu " + programName;
        $scope.mail.message = "Dzień dobry,\r\n\r\nw załączniku przesyłamy licencję dla programu " + programName + ".\r\n\r\nPozdrawiamy,\r\nzespół inSolutions.";

        $("#mailsender").modal();
    }
}]);