'use strict';
app.controller('sucursalController', ['$scope', 'sucursalService', 'authService',
    function ($scope, sucursalService, authService) {
    
    var serviceBase = 'http://localhost:64486/';


    $scope.sucursales = [];
    
    sucursalService.getSucursales().then(function (results) {
        $scope.sucursales = results.data;

    }, function (error) {
        console.log(error);
    });
    //Cerrar loading splash
    //$window.loading_screen.finish();
}]);

app.controller('sucursalEditar', ['$scope', 'sucursalService', '$routeParams', '$location', function ($scope, sucursalService, $routeParams, $location) {
    $scope.editar = true;
    var serviceBase = 'http://localhost:64486/';
    var codigo = $routeParams.codigo;

    
    $scope.sucursal = {};

    sucursalService.getSucursal(codigo).then(function (results) {
        console.log(results);
        $scope.sucursal = results.data[0];
        console.log($scope.sucursal);
    }, function (error) {
        console.log(error);
    });

    $scope.goBack = function () {
        $location.path('/sucursal');
    };

    $scope.update = function () {
        console.log($scope.sucursal);
        sucursalService.updateSucursal(codigo, $scope.sucursal).then(function (results) {
            $location.path('/sucursal');
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        sucursalService.deleteSucursal(codigo).then(function (results) {
            $location.path('/sucursal');
        }, function (error) {
            console.log(error);
        });
    };

}]);

app.controller('sucursalCrear', ['$scope', 'sucursalService', '$routeParams', '$location', function ($scope, sucursalService, $routeParams, $location) {
    $scope.editar = false;
    var serviceBase = 'http://localhost:64486/';
    $scope.sucursal = {};

    $scope.create = function () {
        console.log("hi from sucursalCrear");
        console.log($scope.sucursal);
        sucursalService.createSucursal($scope.sucursal).then(function (results) {
            $location.path('/sucursal');
        }, function (error) {
            console.log(error);
        });
    };

    $scope.goBack = function () {
        $location.path('/sucursal');
    };

}]);