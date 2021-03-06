﻿'use strict';
app.factory('sucursalService', ['$http', 'CONFIG', function ($http, CONFIG) {
    var serviceBase = CONFIG.SERVICE_BASE;
    var sucursalServiceFactory = {};

    var _getSucursales = function () {

        return $http.get(serviceBase + 'api/sucursales').then(function (results) {
            return results;
        });
    };

    var _getSucursal = function (codigo) {

        return $http.get(serviceBase + 'api/sucursales/' + codigo).then(function (results) {
            return results;
        });
    };

    var _updateSucursal = function (codigo, sucursal) {

        return $http.put(serviceBase + 'api/sucursales/' + codigo, sucursal).then(function (results) {
            return results;
        });
    };

    var _createSucursal = function (sucursal) {

        return $http.post(serviceBase + 'api/sucursales', sucursal).then(function (results) {
            return results;
        });
    };

    var _deleteSucursal = function (codigo, sucursal) {

        return $http.delete(serviceBase + 'api/sucursales/' + codigo).then(function (results) {
            return results;
        });
    };

    sucursalServiceFactory.getSucursales = _getSucursales;
    sucursalServiceFactory.getSucursal = _getSucursal;
    sucursalServiceFactory.createSucursal = _createSucursal;
    sucursalServiceFactory.updateSucursal = _updateSucursal;
    sucursalServiceFactory.deleteSucursal = _deleteSucursal;

    return sucursalServiceFactory;

}]);