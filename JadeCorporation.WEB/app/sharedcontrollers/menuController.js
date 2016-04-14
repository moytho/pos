'use strict';
app.controller('menuController', ['$scope', 'authService', '$location', function ($scope, authService, $location) {
    $scope.estaAutenticado = authService.authentication;
    if ($scope.estaAutenticado.isAuth == true)
    {

        $scope.modulos = [
            {
                "modulo": "Dashboard",
                "icon": "fa fa-home",
                "submodulo": []
            },
                        {
                            "modulo": "Productos",
                            "icon": "fa fa-th",
                            "submodulo": [
                                { "Descripcion": "Producto", "Url": "#/producto" },
                                { "Descripcion": "Bodega", "Url": "#/bodega" },
                                { "Descripcion": "Inventario", "Url": "#/productoInventario" },
                                { "Descripcion": "Busqueda", "Url": "#/productobusqueda" },
                                { "Descripcion": "Transferir producto", "Url": "#/productotransferir" },
                                { "Descripcion": "Clasificaciones", "Url": "#/productoClasificacion" },
                                { "Descripcion": "Marcas", "Url": "#/productoMarca" },
                                { "Descripcion": "Canales de Abastecimientos", "Url": "#/productoAbastecimiento" }
                            ]
                        },
            {
                "modulo": "Clientes",
                "icon": "fa fa-laptop",
                "submodulo": [
                    { "Descripcion": "Informacion de clientes", "Url": "#/cliente" },
                    { "Descripcion": "Tipos de cliente", "Url": "#/clienteTipo" },
                    { "Descripcion": "Cuentas por cobrar", "Url": "#/cuentasPorCobrar" }
                ]
            },
            {
                "modulo": "Proveedores",
                "icon": "fa fa-laptop",
                "submodulo": [
                    { "Descripcion": "Informacion de proveedores", "Url": "#/proveedor" },
                    { "Descripcion": "Cuentas por pagar", "Url": "#/cuentasPorPagar" }
                ]
            },
            {
                "modulo": "Datos Generales",
                "icon": "fa fa-laptop",
                "submodulo": [
                    { "Descripcion": "Datos de mi empresa", "Url": "#/empresa" },
                    { "Descripcion": "Mis sucursales", "Url": "#/sucursal" },
                    { "Descripcion": "Configuracion", "Url": "#/configuracion" }
                ]
            },
            {
                "modulo": "Reportes",
                "icon": "fa fa-book",
                "submodulo": [
                    { "Descripcion": "Ventas diarias", "Url": "#/reporteventadiaria" },
                    { "Descripcion": "Ventas mensuales", "Url": "#/reporteventamensual" },
                    { "Descripcion": "Ventas por rango de fecha", "Url": "#/reporteventarangodefecha" },
                    { "Descripcion": "Ventas por agencia", "Url": "#/reporteventaporagencia" },
                    { "Descripcion": "Ventas por usuario", "Url": "#/reporteventaporusuario" }
                ]
            },
            {
                "modulo": "Usuarios",
                "icon": "fa fa-cogs",
                "submodulo": [
                    { "Descripcion": "Crear Usuario", "Url": "#/registrarusuario" },
                    { "Descripcion": "Actualizar Datos", "Url": "#/usuarioactualizarinformacion" },
                    { "Descripcion": "Ver Mensajes", "Url": "#/usuariomensaje" },
                    { "Descripcion": "Actualizar contrasena", "Url": "#/usuarioactualizarcontrasena" },
                    { "Descripcion": "Caja de Cobro", "Url": "#/usuariocaja" },
                    { "Descripcion": "Notificaciones", "Url": "#/usuarionotificacion" }
                ]
            }

        ];
        console.log($scope.menu);

        //$scope.activeValue;
        $scope.clickedPage = function (value) {
            $scope.activeValue = value;
            // other oeprations
        };
    }
}]);



