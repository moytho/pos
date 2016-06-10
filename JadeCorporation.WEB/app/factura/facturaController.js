app.controller('facturaControllerCreate',
    ['$scope', '$window', 'productoInventarioService', 'facturaService','clienteService', '$routeParams', '$location', 'toastr',
        function
            ($scope, $window, productoInventarioService, facturaService,clienteService,$routeParams, $location, toastr) {

            $scope.total = 0;
            $scope.subTotal = 0;
            $scope.descuento = 0;
            $scope.productos = [];
            $scope.productosBusqueda = [];
            $scope.clientes = [];
            $scope.factura = {
                FacturaDetalles: [],
                cliente: {},
                subtotal:0,
                descuento:0,
                total:0
            };

            debugger;

            var tab = 'producto';
            $('.nav-tabs a[href="#' + tab + '"]').tab('show');

    $scope.editar = false;

    $scope.buscarProducto = function (busqueda) {
        console.clear();
        console.log(busqueda);
        productoInventarioService.getProductosByNombre(busqueda).then(function (results) {
            $scope.productosBusqueda = results.data;
        }, function (error) {
            console.log(error);
        });
        return $scope.productosBusqueda;
    };

    $scope.agregarProducto = function (row) {
        $scope.agregarProductoADetalle(row);
    };

    $scope.agregarClienteAFactura = function (row) {
        $scope.factura.cliente=row;
    };

    $scope.calcularTotales = function () {
        //debugger;
        var total = 0;
        for (var i = 0; i < $scope.factura.FacturaDetalles.length; i++) {
            var producto = $scope.factura.FacturaDetalles[i];
            total += (producto.PrecioVenta * producto.Cantidad);
        }
        $scope.factura.subtotal = total;
        $scope.factura.descuento = 0;
        $scope.factura.total = $scope.factura.subtotal - $scope.factura.descuento;
    };

    $scope.agregarProductoADetalle = function (producto) {
        var existe = false;
        for (var i = 0; i < $scope.factura.FacturaDetalles.length; i++) {
            var productoAgregado = $scope.factura.FacturaDetalles[i];
            if (producto.CodigoProducto == productoAgregado.CodigoProducto) {
                $scope.factura.FacturaDetalles[i].Cantidad++;
                existe = true;
            } 
        }
        if (!existe) {
            $scope.factura.FacturaDetalles.push(producto);
        }
        $scope.calcularTotales();
        console.log($scope.factura);
    };

    //SKU number
    $scope.buscarProductoPorCodigo = function (busqueda) {
        console.log(busqueda);
        productoInventarioService.getProductoBySKU(busqueda).then(function (results) {
            $scope.productosBusqueda = results.data;
            if ($scope.productosBusqueda == null) {
                toastr.info("No se ha encontrado el producto", "Informacion");
            } else {
                $scope.agregarProductoADetalle($scope.productosBusqueda)
            }
        }, function (error) {
            console.log(error);
        }); 
    };

    $scope.getClientePorNombreEIdentificacion = function (busqueda) {
        clienteService.getClientePorNombreEIdentificacion(busqueda).then(function (results) {
            $scope.clientes = results.data;
        }, function (results) {
            toastr.error("Ha sucedido un error inesperado", "Error");
        });
        
        return $scope.clientes;
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
        $scope.factura.FacturaDetalles = $scope.productos;
            facturaService.createFactura($scope.factura).then(function (results) {
                if (results.status == 200) {
                    toastr.success("El pedido se ha creado correctamente. ", "Correcto");
                } else {
                    toastr.error("Ha sucedido un error", "Error");
                }
            }, function (results) {
                    toastr.error("Ha sucedido un error inesperado", "Error");
        });
    };

    //$scope.cargarProductosAPedir();

}]);