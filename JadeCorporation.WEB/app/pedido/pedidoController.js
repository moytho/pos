app.controller('pedidoControllerCreate',
    ['$scope', '$window', 'productoService', 'productoInventarioService', 'pedidoService', '$routeParams', '$location', 'toastr',
        function
            ($scope, $window, productoService, productoInventarioService, pedidoService,$routeParams, $location, toastr) {

    $scope.productos = [];
    $scope.pedido = {
           PedidoDetalles: []
    };

    $scope.cargarProductosAPedir = function () {
        productoInventarioService.getProductosAPedir().then(function (results) {
            $scope.productos = results.data;
        }, function (error) {
            console.log(error);
        });
    };

    $scope.eliminarProducto = function (productos,index) {
        productos.splice(index, 1);
        //posiblemente por si quisieramos eliminar esos productos en espera desde la bd
    };

    $scope.goBack = function () {
        $location.path('/productoInventario/')
    };

    $scope.addItem = function () {
        $scope.invoice.items.push({
            qty: 1,
            description: '',
            cost: 0
        });
    },

    $scope.removeItem = function (index) {
        $scope.invoice.items.splice(index, 1);
    },

    $scope.total = function () {
        var total = 0;
        angular.forEach($scope.invoice.items, function (item) {
            total += item.qty * item.cost;
        })

        return total;
    }

    $scope.create = function () {
        //agregamos los productos al detalle
        $scope.pedido.PedidoDetalles = $scope.productos;
            pedidoService.createPedido($scope.pedido).then(function (results) {
                if (results.status == 200) {
                    toastr.success("El pedido se ha creado correctamente. ", "Correcto");
                } else {
                    toastr.error("Ha sucedido un error", "Error");
                }
            }, function (results) {
                    toastr.error("Ha sucedido un error inesperado", "Error");
        });
    };

    $scope.cargarProductosAPedir();

}]);