'use strict';
app.controller('clienteController', ['$scope', 'clienteService', 
    function ($scope, clienteService) {
    
    $scope.clientes = [];
    
    clienteService.getClientes().then(function (results) {
        $scope.clientes = results.data;

    }, function (error) {
        console.log(error);
    });
    //Cerrar loading splash
    //$window.loading_screen.finish();
}]);

app.controller('clienteEditar', ['$scope', 'clienteService', '$routeParams', '$location', function ($scope, clienteService, $routeParams, $location) {
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
            $location.path('/cliente');
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        clienteService.deleteCliente(codigo).then(function (results) {
            $location.path('/cliente');
        }, function (error) {
            console.log(error);
        });
    };

}]);

app.controller('clienteCrear', ['$scope', 'clienteService', '$routeParams', '$location', function ($scope, clienteService, $routeParams, $location) {
    $scope.editar = false;
    $scope.cliente = {};

    $scope.create = function () {
        clienteService.createCliente($scope.cliente).then(function (results) {
            $location.path('/cliente');
        }, function (error) {
            console.log(error);
        });
    };

    $scope.goBack = function () {
        $location.path('/cliente');
    };

}]);