'use strict';
app.controller('loginController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.estaAutenticado = authService.authentication;

    //if ($scope.estaAutenticado.isAuth == true) window.location.href = "http://72.55.164.234/JADEWebApp/#/home";
    if ($scope.estaAutenticado.isAuth == true) window.location.href = "http://localhost:3883/#/home";

    $scope.loginData = {
        Email: "",
        Password: ""
    };

    $scope.message = "";

    $scope.login = function () {
        authService.login($scope.loginData).then(function (response) {

            //$location.path('//#/home');
            //window.location.href = "http://72.55.164.234/JADEWebApp/#/home";
            window.location.href = "http://localhost:3883/#/home";
        
        },
         function (err) {
             console.log(err);
            // $scope.message = err.error_description;
         });
    };

}]);