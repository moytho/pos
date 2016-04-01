'use strict';
app.controller('homeController', ['$scope', 'authService', '$location', '$window',
    function ($scope, authService, $location,$window) {
        if (authService.authentication.isAuth == false) $location.path('/login');
        $window.loading_screen.finish();
}]);