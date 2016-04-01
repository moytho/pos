'use strict';
app.controller('signupController', ['$scope', '$location', '$timeout', 'authService','empresaService', function ($scope, $location, $timeout, authService,empresaService) {

    $scope.savedSuccessfully = false;
    $scope.message = "";

    $scope.registration = {
        Email: "",
        Password: "",
        ConfirmPassword: "",
        Empresa: {}
    };

    $scope.empresas = [];

    empresaService.getEmpresas().then(function (results) {
        $scope.empresas = results.data;

    }, function (error) {
        console.log(error);
    });

    $scope.signUp = function () {

        console.log($scope.registration);
        authService.saveRegistration($scope.registration).then(function (response) {

            $scope.savedSuccessfully = true;
            $scope.message = "El usuario ha sido registrado correctamente. Sera redireccionado a otra pagina en 2 segundos.";
            startTimer();

        },
         function (response) {
             var errors = [];
             for (var key in response.data.modelState) {
                 for (var i = 0; i < response.data.modelState[key].length; i++) {
                     errors.push(response.data.modelState[key][i]);
                 }
             }
             $scope.message = "No se ha completado la accion. Ha sucedido un error:" + errors.join(' ');
         });
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $location.path('/home');
        }, 2000);
    }

}]);