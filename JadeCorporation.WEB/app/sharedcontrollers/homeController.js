'use strict';
app.controller('homeController', ['$scope', 'authService', '$location', '$window',
    function ($scope, authService, $location,$window) {
        $scope.estaAutenticado = authService.authentication;
        if ($scope.estaAutenticado.isAuth == false) $location.path('/login');
        //Cerrar loading splash
        $window.loading_screen.finish();
}]);