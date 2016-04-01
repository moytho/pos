'use strict';
app.controller('sucursalController', ['$scope', 'sucursalService', 'toastr',
    function ($scope, sucursalService, toastr) {
    
    $scope.sucursales = [];
    
    sucursalService.getSucursales().then(function (results) {
        $scope.sucursales = results.data;

    }, function (error) {
        console.log(error);
    });
    //Cerrar loading splash
    //$window.loading_screen.finish();
}]);

app.controller('sucursalEditar', ['$scope', 'sucursalService', '$routeParams', '$location', 'toastr', function ($scope, sucursalService, $routeParams, $location, toastr) {
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
            if (results.status == 204) {
                toastr.success("Datos actualizados correctamente", "Correcto");
                $location.path('/sucursal');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        sucursalService.deleteSucursal(codigo).then(function (results) {
            if (results.status == 200) {
                toastr.success("Datos eliminados correctamente", "Correcto");
                $location.path('/sucursal');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

}]);

app.controller('sucursalCrear', ['$scope', 'sucursalService', '$routeParams', '$location', 'toastr', function ($scope, sucursalService, $routeParams, $location, toastr) {
    $scope.editar = false;
    var serviceBase = 'http://localhost:64486/';
    $scope.sucursal = {};

    $scope.create = function () {
        sucursalService.createSucursal($scope.sucursal).then(function (results) {
            if (results.status == 201) {
                toastr.success("Datos creados correctamente", "Correcto");
                $location.path('/sucursal');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.goBack = function () {
        $location.path('/sucursal');
    };

}]);