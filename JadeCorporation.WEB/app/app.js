var app = angular.module('AngularAuthApp', ['ngAnimate','ngRoute', 'LocalStorageModule', 'angular-loading-bar']);

app.config(function ($routeProvider) {

    $routeProvider.when("/home", {controller: "homeController",templateUrl: "app/sharedviews/home.html"});
    $routeProvider.when("/login", {redirectTo: function () { window.location = "/login.html"; }});
    $routeProvider.when("/registrarusuario", {controller: "signupController",templateUrl: "app/autenticacion/views/signup.html"});
    //un listado de todas las empresa GET api/empresas
    $routeProvider.when("/empresa", { controller: "empresaController", templateUrl: "app/empresa/views/empresa.html" });
    //una empresa especifica GET api/empresas/id?
    $routeProvider.when("/empresa/edit/:codigo", { controller: "empresaEditar", templateUrl: "app/empresa/views/empresa-view.html" });
    //
    $routeProvider.when("/empresa/create", { controller: "empresaCrear", templateUrl: "app/empresa/views/empresa-view.html" });
    $routeProvider.when("/sucursal", { controller: "sucursalController", templateUrl: "app/sucursal/views/sucursal.html" });
    $routeProvider.when("/sucursal/edit/:codigo", { controller: "sucursalEditar", templateUrl: "app/sucursal/views/sucursal-view.html" });
    $routeProvider.when("/sucursal/create", { controller: "sucursalCrear", templateUrl: "app/sucursal/views/sucursal-view.html" });
    $routeProvider.otherwise({ redirectTo: "/home" });
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});