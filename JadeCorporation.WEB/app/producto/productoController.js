'use strict';
app.controller('productoController', ['$scope','$window', 'productoService', 'toastr',
    function ($scope, $window,productoService, toastr) {
    
    $scope.productos = [];
    
    productoService.getProductos().then(function (results) {
        $scope.productos = results.data;

    }, function (error) {
        console.log(error);
    });
    //Cerrar loading splash
    //$window.loading_screen.finish();
}]);

app.controller('productoEditar', ['$scope','$window', 'productoService', 'productoMarcaService', 'productoClasificacionService', '$routeParams', '$location', 'toastr', function ($scope,$window, productoService, productoMarcaService, productoClasificacionService, $routeParams, $location, toastr) {
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
        productoService.updateProducto(codigo, $scope.producto).then(function (results) {
            if (results.status == 204) {
                toastr.success("Datos actualizados correctamente", "Correcto");
                $location.path('/producto');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        productoService.deleteProducto(codigo).then(function (results) {
            if (results.status == 200) {
                toastr.success("Datos eliminados correctamente", "Correcto");
                $location.path('/producto');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

}]);

app.controller('productoCrear', ['$scope', '$window','productoService', 'productoMarcaService', 'productoClasificacionService', '$routeParams', '$location', 'toastr', function ($scope,$window, productoService, productoMarcaService, productoClasificacionService, $routeParams, $location, toastr) {
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
        productoService.createProducto($scope.producto).then(function (results) {
            if (results.status == 201) {
                toastr.success("Datos creados correctamente", "Correcto");
                $location.path('/producto');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            } 
        }, function (error) {
            console.log(error);
        });
    };

    $scope.goBack = function () {
        $location.path('/producto');
    };

}]);