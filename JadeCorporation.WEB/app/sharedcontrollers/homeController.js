'use strict';
app.controller('homeController', ['$scope', 'authService', '$location', '$window','CONFIG',
    function ($scope, authService, $location, $window, CONFIG) {
        $scope.configuracion = CONFIG;
        if (authService.authentication.isAuth == false) $location.path('/login');
        $window.loading_screen.finish();
}]);