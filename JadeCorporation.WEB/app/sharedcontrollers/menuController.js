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
            },
            {
                "modulo": "Productos",
                "icon": "fa fa-th",
                "submodulo": [
                    { "Descripcion": "Inventario", "Url": "#/productoinventario" },
                    { "Descripcion": "Busqueda", "Url": "#/productobusqueda" },
                    { "Descripcion": "Transferir producto", "Url": "#/productotransferir" },
                    { "Descripcion": "Clasificacion", "Url": "#/productoclasificacion" },
                    { "Descripcion": "Bodega", "Url": "#/productobodega" }
                ]
            },
        ];
        console.log($scope.menu);

        //$scope.activeValue;
        $scope.clickedPage = function (value) {
            $scope.activeValue = value;
            // other oeprations
        };
    }
}]);



