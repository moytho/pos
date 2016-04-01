'use strict';
app.factory('productoAbastecimientoService', ['$http', 'CONFIG', function ($http, CONFIG) {
    var serviceBase = CONFIG.SERVICE_BASE;
    var productoAbastecimientoServiceFactory = {};

    var _getProductoAbastecimientos = function () {

        return $http.get(serviceBase + 'api/productoabastecimientos').then(function (results) {
            return results;
        });
    };

    var _getProductoAbastecimiento = function (codigo) {

        return $http.get(serviceBase + 'api/productoabastecimientos/' + codigo).then(function (results) {
            return results;
        });
    };

    var _updateProductoAbastecimiento = function (codigo, productomarca) {

        return $http.put(serviceBase + 'api/productoabastecimientos/' + codigo, productomarca).then(function (results) {
            return results;
        });
    };

    var _createProductoAbastecimiento = function (productomarca) {

        return $http.post(serviceBase + 'api/productoabastecimientos', productomarca).then(function (results) {
            return results;
        });
    };

    var _deleteProductoAbastecimiento = function (codigo, productomarca) {

        return $http.delete(serviceBase + 'api/productoabastecimientos/' + codigo).then(function (results) {
            return results;
        });
    };

    productoAbastecimientoServiceFactory.getProductoAbastecimientos = _getProductoAbastecimientos;
    productoAbastecimientoServiceFactory.getProductoAbastecimiento = _getProductoAbastecimiento;
    productoAbastecimientoServiceFactory.createProductoAbastecimiento = _createProductoAbastecimiento;
    productoAbastecimientoServiceFactory.updateProductoAbastecimiento = _updateProductoAbastecimiento;
    productoAbastecimientoServiceFactory.deleteProductoAbastecimiento = _deleteProductoAbastecimiento;

    return productoAbastecimientoServiceFactory;

}]);