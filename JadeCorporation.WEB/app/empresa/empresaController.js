'use strict';
app.controller('empresaController', ['$rootScope', '$location', '$scope', 'empresaService', 'toastr', function ($rootScope, $location, $scope, empresaService, toastr) {
    
    $scope.empresas = [];
    
    empresaService.getEmpresas().then(function (results) {
        $scope.empresas = results.data;

    }, function (error) {
        console.log(error);
    });
    
}]);

app.controller('empresaEditar', ['$scope', 'empresaService', '$routeParams', '$location', 'toastr', function ($scope, empresaService, $routeParams, $location, toastr) {
    $scope.editar = true;
    var codigo = $routeParams.codigo;

    $scope.empresa = {};

    empresaService.getEmpresa(codigo).then(function (results) {
        $scope.empresa = results.data[0];
        console.log($scope.empresa);
    }, function (error) {
        console.log(error);
    });

    $scope.goBack = function () {
        $location.path('/empresa');
    };

    $scope.update = function () {
        console.log($scope.empresa);
        empresaService.updateEmpresa(codigo,$scope.empresa).then(function (results) {
            if (results.status == 204) {
                toastr.success("Datos actualizados correctamente", "Correcto");
                $location.path('/empresa');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        empresaService.deleteEmpresa(codigo).then(function (results) {
            if (results.status == 200) {
                toastr.success("Datos eliminados correctamente", "Correcto");
                $location.path('/empresa');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });      
    };

}]);

app.controller('empresaCrear', ['$scope', 'empresaService', '$routeParams', '$location', 'toastr', function ($scope, empresaService, $routeParams, $location, toastr) {
    $scope.editar = false;
    $scope.empresa = {};

    $scope.create = function () {
        empresaService.createEmpresa($scope.empresa).then(function (results) {
            if (results.status == 201) {
                toastr.success("Datos creados correctamente", "Correcto");
                $location.path('/empresa');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };
    
    $scope.goBack = function () {
        $location.path('/empresa');
    };

}]);