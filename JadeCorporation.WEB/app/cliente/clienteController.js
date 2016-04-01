'use strict';
app.controller('clienteController', ['$scope', 'clienteService', 'toastr',
    function ($scope, clienteService, toastr) {
    
    $scope.clientes = [];
    
    clienteService.getClientes().then(function (results) {
        $scope.clientes = results.data;

    }, function (error) {
        console.log(error);
    });
    //Cerrar loading splash
    //$window.loading_screen.finish();
}]);

app.controller('clienteEditar', ['$scope', 'clienteService', '$routeParams', '$location', 'toastr', function ($scope, clienteService, $routeParams, $location, toastr) {
    $scope.editar = true;
    var codigo = $routeParams.codigo;
    
    $scope.cliente = {};

        clienteService.getCliente(codigo).then(function (results) {
            $scope.cliente = results.data[0];
               
        },function (error) {
        console.log(error);
    });

    $scope.goBack = function () {
        $location.path('/cliente');
    };

    $scope.update = function () {
        console.log($scope.cliente);
        clienteService.updateCliente(codigo, $scope.cliente).then(function (results) {
            if (results.status == 204) {
                toastr.success("Datos actualizados correctamente", "Correcto");
                $location.path('/cliente');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        clienteService.deleteCliente(codigo).then(function (results) {
            if (results.status == 200) {
                toastr.success("Datos eliminados correctamente", "Correcto");
                $location.path('/cliente');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

}]);

app.controller('clienteCrear', ['$scope', 'clienteService', '$routeParams', '$location', 'toastr', function ($scope, clienteService, $routeParams, $location, toastr) {
    $scope.editar = false;
    $scope.cliente = {};

    $scope.create = function () {
        clienteService.createCliente($scope.cliente).then(function (results) {
            if (results.status == 201) {
                toastr.success("Datos creados correctamente", "Correcto");
                $location.path('/cliente');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.goBack = function () {
        $location.path('/cliente');
    };

}]);