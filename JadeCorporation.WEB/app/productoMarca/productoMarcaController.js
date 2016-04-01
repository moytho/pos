'use strict';
app.controller('productoMarcaController', ['$scope', 'productoMarcaService', 'toastr',
    function ($scope, productoMarcaService, toastr) {

        $scope.productoMarcas = [];
        productoMarcaService.getProductoMarcas().then(function (results) {

            $scope.productoMarcas = results.data;
        
        }, function (error) {
            console.log(error);
        });
        //Cerrar loading splash
        //$window.loading_screen.finish();
    }]);

app.controller('productoMarcaEditar', ['$scope', 'productoMarcaService', '$routeParams', '$location','toastr', function ($scope, productoMarcaService, $routeParams, $location,toastr) {
    $scope.editar = true;
    var codigo = $routeParams.codigo;

    $scope.productoMarca = {};

    productoMarcaService.getProductoMarca(codigo).then(function (results) {
        $scope.productoMarca = results.data[0];
    },
        function (error) {
            console.log(error);
        });

    $scope.goBack = function () {
        $location.path('/productoMarca');
    };

    $scope.update = function () {
        productoMarcaService.updateProductoMarca(codigo, $scope.productoMarca).then(function (results) {
            if (results.status == 204) {
                toastr.success("Datos actualizados correctamente", "Correcto");
                $location.path('/productoMarca');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
            
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        productoMarcaService.deleteProductoMarca(codigo).then(function (results) {
            if (results.status == 200) {
                toastr.success("Datos eliminados correctamente", "Correcto");
                $location.path('/productoMarca');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

}]);

app.controller('productoMarcaCrear', ['$scope', 'productoMarcaService', '$routeParams', '$location','toastr', function ($scope, productoMarcaService, $routeParams, $location,toastr) {
    $scope.editar = false;
    $scope.productoMarca = {};

    $scope.create = function () {
        productoMarcaService.createProductoMarca($scope.productoMarca).then(function (results) {
            if (results.status == 201) {
                toastr.success("Datos creados correctamente", "Correcto");
                $location.path('/productoMarca');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.goBack = function () {
        $location.path('/productoMarca');
    };

}]);