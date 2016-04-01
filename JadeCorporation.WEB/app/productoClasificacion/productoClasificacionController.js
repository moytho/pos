'use strict';
app.controller('productoClasificacionController', ['$scope', 'productoClasificacionService', 'toastr',
    function ($scope, productoClasificacionService, toastr) {
    
        $scope.productoClasificaciones = [];
    
        productoClasificacionService.getProductoClasificaciones().then(function (results) {
            $scope.productoClasificaciones = results.data;

        }, function (error) {
            console.log(error);
        });
    //Cerrar loading splash
    //$window.loading_screen.finish();
}]);

app.controller('productoClasificacionEditar', ['$scope', 'productoClasificacionService', '$routeParams', '$location', 'toastr', function ($scope, productoClasificacionService, $routeParams, $location, toastr) {
    $scope.editar = true;
    var codigo = $routeParams.codigo;

    $scope.productoClasificaciones = [];

    productoClasificacionService.getProductoClasificaciones().then(function (results) {
        $scope.productoClasificaciones = results.data;
        
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
        productoClasificacionService.updateProductoClasificacion(codigo, $scope.productoClasificacion).then(function (results) {
            if (results.status == 204) {
                toastr.success("Datos actualizados correctamente", "Correcto");
                $location.path('/productoClasificacion');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        productoClasificacionService.deleteProductoClasificacion(codigo).then(function (results) {
            if (results.status == 200) {
                toastr.success("Datos eliminados correctamente", "Correcto");
                $location.path('/productoClasificacion');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

}]);

app.controller('productoClasificacionCrear', ['$scope', 'productoClasificacionService', '$routeParams', '$location', 'toastr', function ($scope, productoClasificacionService, $routeParams, $location, toastr) {
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
            if (results.status == 201) {
                toastr.success("Datos creados correctamente", "Correcto");
                $location.path('/productoClasificacion');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.goBack = function () {
        $location.path('/productoClasificacion');
    };

}]);