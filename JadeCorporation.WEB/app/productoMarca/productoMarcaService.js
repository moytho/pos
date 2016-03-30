'use strict';
app.factory('productoMarcaService', ['$http', function ($http) {
    var serviceBase = 'http://localhost:64486/';
    //var serviceBase = 'http://72.55.164.234/JadeAPI/';
    var productoMarcaServiceFactory = {};

    var _getProductoMarcas = function () {

        return $http.get(serviceBase + 'api/productomarcas').then(function (results) {
            return results;
        });
    };

    var _getProductoMarca = function (codigo) {

        return $http.get(serviceBase + 'api/productomarcas/' + codigo).then(function (results) {
            return results;
        });
    };

    var _updateProductoMarca = function (codigo, productomarca) {

        return $http.put(serviceBase + 'api/productomarcas/' + codigo, productomarca).then(function (results) {
            return results;
        });
    };

    var _createProductoMarca = function (productomarca) {

        return $http.post(serviceBase + 'api/productomarcas', productomarca).then(function (results) {
            return results;
        });
    };

    var _deleteProductoMarca = function (codigo, productomarca) {

        return $http.delete(serviceBase + 'api/productomarcas/' + codigo).then(function (results) {
            return results;
        });
    };

    productoMarcaServiceFactory.getProductoMarcas = _getProductoMarcas;
    productoMarcaServiceFactory.getProductoMarca = _getProductoMarca;
    productoMarcaServiceFactory.createProductoMarca = _createProductoMarca;
    productoMarcaServiceFactory.updateProductoMarca = _updateProductoMarca;
    productoMarcaServiceFactory.deleteProductoMarca = _deleteProductoMarca;

    return productoMarcaServiceFactory;

}]);