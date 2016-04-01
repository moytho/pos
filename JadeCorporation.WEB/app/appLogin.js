var app = angular.module('AngularAuthAppLogin', ['ngAnimate', 'ngRoute', 'toastr', 'LocalStorageModule', 'angular-loading-bar', 'ngMessages']);

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});