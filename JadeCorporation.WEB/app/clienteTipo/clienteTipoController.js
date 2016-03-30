'use strict';
app.controller('clienteTipoController', ['$scope', 'clienteTipoService', 
    function ($scope, clienteTipoService) {
    
    $scope.clienteTipos = [];
    
    clienteTipoService.getClienteTipos().then(function (results) {
        $scope.clienteTipos = results.data;

    }, function (error) {
        console.log(error);
    });
    //Cerrar loading splash
    //$window.loading_screen.finish();
}]);

app.controller('clienteTipoEditar', ['$scope', 'clienteTipoService', '$routeParams', '$location', function ($scope, clienteTipoService, $routeParams, $location) {
    $scope.editar = true;
    var codigo = $routeParams.codigo;
    
    $scope.clienteTipo = {};

        clienteTipoService.getClienteTipo(codigo).then(function (results) {
            $scope.clienteTipo = results.data[0];
               
        },function (error) {
        console.log(error);
    });

    $scope.goBack = function () {
        $location.path('/clienteTipo');
    };

    $scope.update = function () {
        console.log($scope.clienteTipo);
        clienteTipoService.updateClienteTipo(codigo, $scope.clienteTipo).then(function (results) {
            $location.path('/clienteTipo');
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        clienteTipoService.deleteClienteTipo(codigo).then(function (results) {
            $location.path('/clienteTipo');
        }, function (error) {
            console.log(error);
        });
    };

}]);

app.controller('clienteTipoCrear', ['$scope', 'clienteTipoService', '$routeParams', '$location', function ($scope, clienteTipoService, $routeParams, $location) {
    $scope.editar = false;
    $scope.clienteTipo = {};

    $scope.create = function () {
        clienteTipoService.createClienteTipo($scope.clienteTipo).then(function (results) {
            $location.path('/clienteTipo');
        }, function (error) {
            console.log(error);
        });
    };

    $scope.goBack = function () {
        $location.path('/clienteTipo');
    };

}]);