'use strict';
app.factory('bodegaService', ['$http', 'CONFIG', function ($http, CONFIG) {
    var serviceBase = CONFIG.SERVICE_BASE;
    var bodegaServiceFactory = {};

    var _getBodegas = function () {

        return $http.get(serviceBase + 'api/bodegas').then(function (results) {
            return results;
        });
    };

    var _getBodega = function (codigo) {

        return $http.get(serviceBase + 'api/bodegas/' + codigo).then(function (results) {
            return results;
        });
    };

    var _updateBodega = function (codigo, bodega) {

        return $http.put(serviceBase + 'api/bodegas/' + codigo, bodega).then(function (results) {
            return results;
        });
    };

    var _createBodega = function (bodega) {

        return $http.post(serviceBase + 'api/bodegas', bodega).then(function (results) {
            return results;
        });
    };

    var _deleteBodega = function (codigo, bodega) {

        return $http.delete(serviceBase + 'api/bodegas/' + codigo).then(function (results) {
            return results;
        });
    };

    bodegaServiceFactory.getBodegas = _getBodegas;
    bodegaServiceFactory.getBodega = _getBodega;
    bodegaServiceFactory.createBodega = _createBodega;
    bodegaServiceFactory.updateBodega = _updateBodega;
    bodegaServiceFactory.deleteBodega = _deleteBodega;

    return bodegaServiceFactory;

}]);