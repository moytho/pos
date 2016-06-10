app.controller('productoInventarioController',
    ['$scope', '$window', 'productoService', 'productoMarcaService', 'productoClasificacionService',
        'productoAbastecimientoService', 'productoInventarioService', 'bodegaService', '$routeParams', '$location', 'toastr',
        function
            ($scope, $window, productoService, productoMarcaService, productoClasificacionService, productoAbastecimientoService,
            productoInventarioService, bodegaService, $routeParams, $location, toastr) {
    $scope.editar = false;
    $scope.productoMarcas = [];
    $scope.productoClasificaciones = [];
    $scope.productoSubClasificaciones = [];
    $scope.productoAbastecimientos = [];
    $scope.productoTipos = [];
    $scope.producto = {};
    $scope.bodegas = [];
    $scope.productos = [];

    $scope.buscar = function () {
        productoInventarioService.getProductos($scope.busqueda).then(function (results) {
            $scope.productos = results.data;
        }, function (error) {
            console.log(error);
        });
    };

    $scope.agregarProductoAPedir = function (productoAPedir) {
            productoInventarioService.agregarProductoAPedir(productoAPedir).then(function (results) {
                if (results.status == 200) {
                    toastr.success("El producto se ha agregado a la proxima lista de productos a pedir. ", "Correcto");
                } else {
                    toastr.error("Ha sucedido un error", "Error");
                }
            }, function (results) {
                if (results.status == 400) {
                    toastr.info("El producto ya ha sido agregado a la proxima lista de productos a pedir", "Information");
                }
                else {
                    toastr.error("Ha sucedido un error inesperado", "Error");
                }
            
        });
    };

    $scope.cargarSubClasificaciones = function () {
        productoClasificacionService.getProductoSubClasificacionesPorSuClasificacion($scope.busqueda.CodigoProductoClasificacion).then(function (results) {
            $scope.productoSubClasificaciones = results.data;
        }, function (error) {
            console.log(error);
        });
    };



    productoClasificacionService.getProductoClasificaciones().then(function (results) {
        $scope.productoClasificaciones = results.data;

        productoMarcaService.getProductoMarcas().then(function (results) {
            $scope.productoMarcas = results.data;

            productoAbastecimientoService.getProductoAbastecimientos().then(function (results) {
                $scope.productoAbastecimientos = results.data;

                bodegaService.getBodegas().then(function (results) {

                    $scope.bodegas = results.data;

                }, function (error) {
                    console.log(error);
                });

            }, function (error) {
                console.log(error);
            });
        }, function (error) {
            console.log(error);
        });
    }, function (error) {
        console.log(error);
    });

}]);