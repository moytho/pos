'use strict';
app.controller('productoController', ['$scope', 'productoService', 
    function ($scope, productoService) {
    
    $scope.productos = [];
    
    productoService.getProductos().then(function (results) {
        $scope.productos = results.data;

    }, function (error) {
        console.log(error);
    });
    //Cerrar loading splash
    //$window.loading_screen.finish();
}]);

app.controller('productoEditar', ['$scope', 'productoService', 'productoMarcaService', 'productoClasificacionService', '$routeParams', '$location', function ($scope, productoService, productoMarcaService, productoClasificacionService, $routeParams, $location) {
    $scope.editar = true;
    var codigo = $routeParams.codigo;
    
    $scope.productoMarcas = [];
    $scope.productoClasificaciones = [];
    $scope.producto = {};

    productoClasificacionService.getProductoClasificaciones().then(function (results) {
        $scope.productoClasificaciones = results.data;
        productoMarcaService.getProductoMarcas().then(function (results) {
            $scope.productoMarcas = results.data;
            productoService.getProducto(codigo).then(function (results) {
                console.log(results);
                $scope.producto = results.data[0];
                console.log($scope.producto);
            }, function (error) {
                console.log(error);
            });
        }, function (error) {
            console.log(error);
        });
    }, function (error) {
        console.log(error);
    });



    


    $scope.goBack = function () {
        $location.path('/producto');
    };

    $scope.update = function () {
        console.log($scope.producto);
        productoService.updateProducto(codigo, $scope.producto).then(function (results) {
            $location.path('/producto');
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        productoService.deleteProducto(codigo).then(function (results) {
            $location.path('/producto');
        }, function (error) {
            console.log(error);
        });
    };

}]);

app.controller('productoCrear', ['$scope', 'productoService', 'productoMarcaService', 'productoClasificacionService', '$routeParams', '$location', function ($scope, productoService, productoMarcaService, productoClasificacionService, $routeParams, $location) {
    $scope.editar = false;
    $scope.productoMarcas = [];
    $scope.productoClasificaciones = [];
    $scope.producto = {};

    productoClasificacionService.getProductoClasificaciones().then(function (results) {
        $scope.productoClasificaciones = results.data;

    }, function (error) {
        console.log(error);
    });

    productoMarcaService.getProductoMarcas().then(function (results) {
        $scope.productoMarcas = results.data;

    }, function (error) {
        console.log(error);
    });

    $scope.create = function () {
        console.log("hi from productoCrear");
        console.log($scope.producto);
        productoService.createProducto($scope.producto).then(function (results) {
            $location.path('/producto');
        }, function (error) {
            console.log(error);
        });
    };

    $scope.goBack = function () {
        $location.path('/producto');
    };

}]);