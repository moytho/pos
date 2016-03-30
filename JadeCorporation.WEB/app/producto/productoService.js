'use strict';
app.factory('productoService', ['$http', function ($http) {
    var serviceBase = 'http://localhost:64486/';
    //var serviceBase = 'http://72.55.164.234/JadeAPI/';
    var productoServiceFactory = {};

    var _getProductos = function () {

        return $http.get(serviceBase + 'api/productos').then(function (results) {
            return results;
        });
    };

    var _getProducto = function (codigo) {

        return $http.get(serviceBase + 'api/productos/' + codigo).then(function (results) {
            return results;
        });
    };

    var _updateProducto = function (codigo, producto) {

        return $http.put(serviceBase + 'api/productos/' + codigo, producto).then(function (results) {
            return results;
        });
    };

    var _createProducto = function (producto) {

        return $http.post(serviceBase + 'api/productos', producto).then(function (results) {
            return results;
        });
    };

    var _deleteProducto = function (codigo, producto) {

        return $http.delete(serviceBase + 'api/productos/' + codigo).then(function (results) {
            return results;
        });
    };

    productoServiceFactory.getProductos = _getProductos;
    productoServiceFactory.getProducto = _getProducto;
    productoServiceFactory.createProducto = _createProducto;
    productoServiceFactory.updateProducto = _updateProducto;
    productoServiceFactory.deleteProducto = _deleteProducto;

    return productoServiceFactory;

}]);