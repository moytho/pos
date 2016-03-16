'use strict';
app.controller('empresaController', ['$rootScope', '$location','$scope', 'empresaService', function ($rootScope,$location,$scope, empresaService) {
    
    $scope.empresas = [];
    
    empresaService.getEmpresas().then(function (results) {
        $scope.empresas = results.data;

    }, function (error) {
        console.log(error);
    });
    
}]);

app.controller('empresaEditar', ['$scope', 'empresaService', '$routeParams', '$location', function ($scope, empresaService, $routeParams, $location) {
    $scope.editar = true;
    var codigo = $routeParams.codigo;

    $scope.empresa = {};

    empresaService.getEmpresa(codigo).then(function (results) {
        $scope.empresa = results.data[0];
        console.log($scope.empresa);
    }, function (error) {
        console.log(error);
    });

    $scope.goBack = function () {
        $location.path('/empresa');
    };

    $scope.update = function () {
        console.log($scope.empresa);
        empresaService.updateEmpresa(codigo,$scope.empresa).then(function (results) {
            $location.path('/empresa');
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        empresaService.deleteEmpresa(codigo).then(function (results) {
            $location.path('/empresa');
        }, function (error) {
            console.log(error);
        });      
    };

}]);

app.controller('empresaCrear', ['$scope', 'empresaService', '$routeParams', '$location', function ($scope, empresaService, $routeParams, $location) {
    $scope.editar = false;
    $scope.empresa = {};

    $scope.create = function () {
        console.log("hi from empresaCrear");
        console.log($scope.empresa);
        empresaService.createEmpresa($scope.empresa).then(function (results) {
            $location.path('/empresa');
        }, function (error) {
            console.log(error);
        });
    };
    
    $scope.goBack = function () {
        $location.path('/empresa');
    };

}]);