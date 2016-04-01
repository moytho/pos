'use strict';
app.controller('productoAbastecimientoController', ['$scope', 'productoAbastecimientoService','toastr',
    function ($scope, productoAbastecimientoService, toastr) {

        $scope.productoAbastecimientos = [];

        productoAbastecimientoService.getProductoAbastecimientos().then(function (results) {
            $scope.productoAbastecimientos = results.data;

        }, function (error) {
            console.log(error);
        });
        //Cerrar loading splash
        //$window.loading_screen.finish();
    }]);

app.controller('productoAbastecimientoEditar', ['$scope', 'productoAbastecimientoService', '$routeParams', '$location', 'toastr', function ($scope, productoAbastecimientoService, $routeParams, $location, toastr) {
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
            if (results.status == 204) {
                toastr.success("Datos actualizados correctamente", "Correcto");
                $location.path('/productoAbastecimiento');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        productoAbastecimientoService.deleteProductoAbastecimiento(codigo).then(function (results) {
            if (results.status == 200) {
                toastr.success("Datos eliminados correctamente", "Correcto");
                $location.path('/productoAbastecimiento');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

}]);

app.controller('productoAbastecimientoCrear', ['$scope', 'productoAbastecimientoService', '$routeParams', '$location', 'toastr', function ($scope, productoAbastecimientoService, $routeParams, $location, toastr) {
    $scope.editar = false;
    $scope.productoAbastecimiento = {};

    $scope.create = function () {
        productoAbastecimientoService.createProductoAbastecimiento($scope.productoAbastecimiento).then(function (results) {
            if (results.status == 201) {
                toastr.success("Datos creados correctamente", "Correcto");
                $location.path('/productoAbastecimiento');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.goBack = function () {
        $location.path('/productoAbastecimiento');
    };

}]);