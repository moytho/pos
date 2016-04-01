'use strict';
app.factory('productoClasificacionService', ['$http', 'CONFIG', function ($http, CONFIG) {
    var serviceBase = CONFIG.SERVICE_BASE;
    var productoClasificacionServiceFactory = {};

    var _getProductoClasificaciones = function () {

        return $http.get(serviceBase + 'api/productoclasificaciones').then(function (results) {
            return results;
        });
    };

    var _getProductoClasificacion = function (codigo) {

        return $http.get(serviceBase + 'api/productoclasificaciones/' + codigo).then(function (results) {
            return results;
        });
    };

    var _updateProductoClasificacion = function (codigo, productoclasificacion) {

        return $http.put(serviceBase + 'api/productoclasificaciones/' + codigo, productoclasificacion).then(function (results) {
            return results;
        });
    };

    var _createProductoClasificacion = function (productoclasificacion) {

        return $http.post(serviceBase + 'api/productoclasificaciones', productoclasificacion).then(function (results) {
            return results;
        });
    };

    var _deleteProductoClasificacion = function (codigo, productoclasificacion) {

        return $http.delete(serviceBase + 'api/productoclasificaciones/' + codigo).then(function (results) {
            return results;
        });
    };

    productoClasificacionServiceFactory.getProductoClasificaciones = _getProductoClasificaciones;
    productoClasificacionServiceFactory.getProductoClasificacion = _getProductoClasificacion;
    productoClasificacionServiceFactory.createProductoClasificacion = _createProductoClasificacion;
    productoClasificacionServiceFactory.updateProductoClasificacion = _updateProductoClasificacion;
    productoClasificacionServiceFactory.deleteProductoClasificacion = _deleteProductoClasificacion;

    return productoClasificacionServiceFactory;

}]);