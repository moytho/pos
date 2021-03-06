﻿'use strict';
app.factory('clienteTipoService', ['$http', 'CONFIG', function ($http, CONFIG) {
    var serviceBase = CONFIG.SERVICE_BASE;
    var clienteTipoServiceFactory = {};

    var _getClienteTipos = function () {

        return $http.get(serviceBase + 'api/clientetipos').then(function (results) {
            return results;
        });
    };

    var _getClienteTipo = function (codigo) {

        return $http.get(serviceBase + 'api/clientetipos/' + codigo).then(function (results) {
            return results;
        });
    };

    var _updateClienteTipo = function (codigo, clientTipo) {

        return $http.put(serviceBase + 'api/clientetipos/' + codigo, clientTipo).then(function (results) {
            return results;
        });
    };

    var _createClienteTipo = function (clientTipo) {

        return $http.post(serviceBase + 'api/clientetipos', clientTipo).then(function (results) {
            return results;
        });
    };

    var _deleteClienteTipo = function (codigo, clientTipo) {

        return $http.delete(serviceBase + 'api/clientetipos/' + codigo).then(function (results) {
            return results;
        });
    };

    clienteTipoServiceFactory.getClienteTipos = _getClienteTipos;
    clienteTipoServiceFactory.getClienteTipo = _getClienteTipo;
    clienteTipoServiceFactory.createClienteTipo = _createClienteTipo;
    clienteTipoServiceFactory.updateClienteTipo = _updateClienteTipo;
    clienteTipoServiceFactory.deleteClienteTipo = _deleteClienteTipo;

    return clienteTipoServiceFactory;

}]);