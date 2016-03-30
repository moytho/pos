'use strict';
app.controller('productoAbastecimientoController', ['$scope', 'productoAbastecimientoService',
    function ($scope, productoAbastecimientoService) {

        $scope.productoAbastecimientos = [];

        productoAbastecimientoService.getProductoAbastecimientos().then(function (results) {
            $scope.productoAbastecimientos = results.data;

        }, function (error) {
            console.log(error);
        });
        //Cerrar loading splash
        //$window.loading_screen.finish();
    }]);

app.controller('productoAbastecimientoEditar', ['$scope', 'productoAbastecimientoService', '$routeParams', '$location', function ($scope, productoAbastecimientoService, $routeParams, $location) {
    $scope.editar = true;
    var codigo = $routeParams.codigo;

    $scope.productoAbastecimiento = {};

    productoAbastecimientoService.getProductoAbastecimiento(codigo).then(function (results) {
        $scope.productoAbastecimiento = results.data[0];
    },
        function (error) {
            console.log(error);
        });

    $scope.goBack = function () {
        $location.path('/productoAbastecimiento');
    };

    $scope.update = function () {
        productoAbastecimientoService.updateProductoAbastecimiento(codigo, $scope.productoAbastecimiento).then(function (results) {
            $location.path('/productoAbastecimiento');
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        productoAbastecimientoService.deleteProductoAbastecimiento(codigo).then(function (results) {
            $location.path('/productoAbastecimiento');
        }, function (error) {
            console.log(error);
        });
    };

}]);

app.controller('productoAbastecimientoCrear', ['$scope', 'productoAbastecimientoService', '$routeParams', '$location', function ($scope, productoAbastecimientoService, $routeParams, $location) {
    $scope.editar = false;
    $scope.productoAbastecimiento = {};

    $scope.create = function () {
        productoAbastecimientoService.createProductoAbastecimiento($scope.productoAbastecimiento).then(function (results) {
            $location.path('/productoAbastecimiento');
        }, function (error) {
            console.log(error);
        });
    };

    $scope.goBack = function () {
        $location.path('/productoAbastecimiento');
    };

}]);