'use strict';
app.factory('productoService', ['$http', 'CONFIG', function ($http, CONFIG) {
    var serviceBase = CONFIG.SERVICE_BASE;
    var productoServiceFactory = {};

    var _setProductoImagenConvertirPrincipal = function (CodigoProductoImagen,CodigoProducto) {

        return $http.get(serviceBase + 'api/productoimagenconvertirprincipal?CodigoProductoImagen=' + CodigoProductoImagen + '&CodigoProducto='+CodigoProducto).then(function (results) {
            return results;
        });
    };

    var _getProductoImagenes = function (codigo) {

        return $http.get(serviceBase + 'api/productoimagenes/'+ codigo).then(function (results) {
            return results;
        });
    };
    //Aunque es una eliminacion que vamos hacer no necesitamos enviar $http.delete
    var _deleteProductoImagen = function (codigo) {

        return $http.get(serviceBase + 'api/productoimageneliminar/' + codigo).then(function (results) {
            return results;
        });
    };

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
    productoServiceFactory.setProductoImagenConvertirPrincipal = _setProductoImagenConvertirPrincipal;
    productoServiceFactory.getProductoImagenes = _getProductoImagenes;
    productoServiceFactory.getProducto = _getProducto;
    productoServiceFactory.createProducto = _createProducto;
    productoServiceFactory.updateProducto = _updateProducto;
    productoServiceFactory.deleteProducto = _deleteProducto;
    productoServiceFactory.deleteProductoImagen = _deleteProductoImagen;

    return productoServiceFactory;

}]);