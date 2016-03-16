'use strict';
app.controller('usuarioHeaderController', ['$scope', '$location', 'authService', function ($scope, $location, authService) {

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/login');
    }

    $scope.authentication = authService.authentication;
    //Cerrar loading splash
    //$window.loading_screen.finish();
}]);