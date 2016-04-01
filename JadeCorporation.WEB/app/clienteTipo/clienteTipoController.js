'use strict';
app.controller('clienteTipoController', ['$scope', 'clienteTipoService','toastr', 
    function ($scope, clienteTipoService, toastr) {
    
    $scope.clienteTipos = [];
    
    clienteTipoService.getClienteTipos().then(function (results) {
        $scope.clienteTipos = results.data;

    }, function (error) {
        console.log(error);
    });
    //Cerrar loading splash
    //$window.loading_screen.finish();
}]);

app.controller('clienteTipoEditar', ['$scope', 'clienteTipoService', '$routeParams', '$location','toastr', function ($scope, clienteTipoService, $routeParams, $location,toastr) {
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
        clienteTipoService.updateClienteTipo(codigo, $scope.clienteTipo).then(function (results) {
            if (results.status == 204) {
                toastr.success("Datos actualizados correctamente", "Correcto");
                $location.path('/clienteTipo');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        clienteTipoService.deleteClienteTipo(codigo).then(function (results) {
            if (results.status == 200) {
                toastr.success("Datos eliminados correctamente", "Correcto");
                $location.path('/clienteTipo');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

}]);

app.controller('clienteTipoCrear', ['$scope', 'clienteTipoService', '$routeParams', '$location', 'toastr', function ($scope, clienteTipoService, $routeParams, $location, toastr) {
    $scope.editar = false;
    $scope.clienteTipo = {};

    $scope.create = function () {
        clienteTipoService.createClienteTipo($scope.clienteTipo).then(function (results) {
            if (results.status == 201) {
                toastr.success("Datos creados correctamente", "Correcto");
                $location.path('/clienteTipo');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.goBack = function () {
        $location.path('/clienteTipo');
    };

}]);