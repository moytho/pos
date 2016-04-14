'use strict';
app.controller('bodegaController', ['$scope', 'bodegaService', 'toastr',
    function ($scope, bodegaService, toastr) {

        $scope.bodegas = [];
        bodegaService.getBodegas().then(function (results) {

            $scope.bodegas = results.data;
        
        }, function (error) {
            console.log(error);
        });
        //Cerrar loading splash
        //$window.loading_screen.finish();
    }]);

app.controller('bodegaEditar', ['$scope', 'bodegaService', '$routeParams', '$location', 'toastr',
                             function ($scope, bodegaService, $routeParams, $location, toastr) {
    $scope.editar = true;
    var codigo = $routeParams.codigo;

    $scope.bodega = {};

    bodegaService.getBodega(codigo).then(function (results) {
        $scope.bodega = results.data[0];
    },
        function (error) {
            console.log(error);
        });

    $scope.goBack = function () {
        $location.path('/bodega');
    };

    $scope.update = function () {
        bodegaService.updateBodega(codigo, $scope.bodega).then(function (results) {
            if (results.status == 204) {
                toastr.success("Datos actualizados correctamente", "Correcto");
                $location.path('/bodega');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
            
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        /*
        var dlg = dialogs.confirm('Please Confirm', 'Are you absolutely sure you want to delete?');
        dlg.result.then(function () {
            console.log("User has confirmed");//do something when user confirms
        }, function () {
            console.log("User has cancelled");//do something when user cancels. Can omit the 2nd function if no handling is required
        });
        */

        bodegaService.deleteBodega(codigo).then(function (results) {
            if (results.status == 200) {
                toastr.success("Datos eliminados correctamente", "Correcto");
                $location.path('/bodega');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

}]);

app.controller('bodegaCrear', ['$scope', 'bodegaService', '$routeParams', '$location','toastr', function ($scope, bodegaService, $routeParams, $location,toastr) {
    $scope.editar = false;
    $scope.bodega = {};

    $scope.create = function () {
        bodegaService.createBodega($scope.bodega).then(function (results) {
            if (results.status == 201) {
                toastr.success("Datos creados correctamente", "Correcto");
                $location.path('/bodega');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.goBack = function () {
        $location.path('/bodega');
    };

}]);