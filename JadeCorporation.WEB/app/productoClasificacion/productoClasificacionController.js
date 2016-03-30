'use strict';
app.controller('productoClasificacionController', ['$scope', 'productoClasificacionService', 
    function ($scope, productoClasificacionService) {
    
        $scope.productoClasificaciones = [];
    
        productoClasificacionService.getProductoClasificaciones().then(function (results) {
            $scope.productoClasificaciones = results.data;

        }, function (error) {
            console.log(error);
        });
    //Cerrar loading splash
    //$window.loading_screen.finish();
}]);

app.controller('productoClasificacionEditar', ['$scope', 'productoClasificacionService', '$routeParams', '$location', function ($scope, productoClasificacionService, $routeParams, $location) {
    $scope.editar = true;
    var codigo = $routeParams.codigo;

    $scope.productoClasificaciones = [];

    productoClasificacionService.getProductoClasificaciones().then(function (results) {
        $scope.productoClasificaciones = results.data;
        console.log($scope.productoClasificaciones);
    }, function (error) {
        console.log(error);
    });

    $scope.productoClasificacion = {};

    productoClasificacionService.getProductoClasificacion(codigo).then(function (results) {
        $scope.productoClasificacion = results.data[0];
        console.log($scope.productoClasificacion);
    },
        function (error) {
        console.log(error);
    });

    $scope.goBack = function () {
        $location.path('/productoClasificacion');
    };

    $scope.update = function () {
        console.log($scope.productoClasificacion);
        productoClasificacionService.updateProductoClasificacion(codigo, $scope.productoClasificacion).then(function (results) {
            $location.path('/productoClasificacion');
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        productoClasificacionService.deleteProductoClasificacion(codigo).then(function (results) {
            $location.path('/productoClasificacion');
        }, function (error) {
            console.log(error);
        });
    };

}]);

app.controller('productoClasificacionCrear', ['$scope', 'productoClasificacionService', '$routeParams', '$location', function ($scope, productoClasificacionService, $routeParams, $location) {
    $scope.editar = false;
    $scope.productoClasificacion = {};
    $scope.productoClasificaciones = [];

    productoClasificacionService.getProductoClasificaciones().then(function (results) {
        $scope.productoClasificaciones = results.data;

    }, function (error) {
        console.log(error);
    });
    $scope.create = function () {
        productoClasificacionService.createProductoClasificacion($scope.productoClasificacion).then(function (results) {
            $location.path('/productoClasificacion');
        }, function (error) {
            console.log(error);
        });
    };

    $scope.goBack = function () {
        $location.path('/productoClasificacion');
    };

}]);