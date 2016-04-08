'use strict';
app.controller('loginController', ['$scope', '$location', 'authService','CONFIG', function ($scope, $location, authService,CONFIG) {

    $scope.configuracion = CONFIG;

    if (authService.authentication.isAuth) window.location.href = CONFIG.HOME_URL;

    $scope.loginData = {
        Email: "",
        Password: ""
    };

    $scope.message = "";

    $scope.login = function () {
        $scope.message = '';
        authService.login($scope.loginData).then(function (response) {
            //alert(CONFIG.HOME_URL);
            window.location.href = CONFIG.HOME_URL;
            
        
        },
         function (err) {
             console.log(err);
             $scope.message = err.error_description;
         });
    };

}]);