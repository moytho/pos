var app = angular.module('AngularAuthAppLogin', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar']);

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});