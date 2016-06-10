'use strict';
app.factory('productoInventarioService', ['$http', 'CONFIG', function ($http, CONFIG) {
    var serviceBase = CONFIG.SERVICE_BASE;
    var productoInventarioServiceFactory = {};

    var _agregarProductoAPedir = function (producto) {
        return $http.post(serviceBase + 'api/productoapedir/', producto).then(function (results) {
            return results;
        });
    };

    var _getProductosAPedir = function () {
        return $http.get(serviceBase + 'api/productosapedir/').then(function (results) {
            return results;
        });
    };

    var _getProductos = function (filter) {

        return $http.get(serviceBase + 'api/productoinventarios/', { params: filter }).then(function (results) {
            return results;
        });
    };

    var _getProductos = function (filter) {

        return $http.get(serviceBase + 'api/productoinventarios/', { params: filter }).then(function (results) {
            return results;
        });
    };

    var _getProductosByNombre = function (nombre) {

        return $http.get(serviceBase + 'api/productoavenderpornombre/?Nombre=' + nombre).then(function (results) {
            return results;
        });
    };

    var _getProductoBySKU = function (codigoProducto) {

        return $http.get(serviceBase + 'api/productoavenderporcodigo/?CodigoProducto='+ codigoProducto).then(function (results) {
            return results;
        });
    };

    productoInventarioServiceFactory.getProductos = _getProductos;
    productoInventarioServiceFactory.getProductoBySKU = _getProductoBySKU;
    productoInventarioServiceFactory.getProductosByNombre = _getProductosByNombre;
    productoInventarioServiceFactory.agregarProductoAPedir = _agregarProductoAPedir;
    productoInventarioServiceFactory.getProductosAPedir = _getProductosAPedir;
    return productoInventarioServiceFactory;

}]);