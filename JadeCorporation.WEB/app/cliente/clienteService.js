'use strict';
app.factory('clienteService', ['$http', 'CONFIG', function ($http, CONFIG) {
    var serviceBase = CONFIG.SERVICE_BASE;
    var clienteServiceFactory = {};

    var _getClientes = function () {

        return $http.get(serviceBase + 'api/clientes').then(function (results) {
            return results;
        });
    };

    var _getCliente = function (codigo) {

        return $http.get(serviceBase + 'api/clientes/' + codigo).then(function (results) {
            return results;
        });
    };

    var _updateCliente = function (codigo, cliente) {

        return $http.put(serviceBase + 'api/clientes/' + codigo, cliente).then(function (results) {
            return results;
        });
    };

    var _createCliente = function (cliente) {

        return $http.post(serviceBase + 'api/clientes', cliente).then(function (results) {
            return results;
        });
    };

    var _deleteCliente = function (codigo, cliente) {

        return $http.delete(serviceBase + 'api/clientes/' + codigo).then(function (results) {
            return results;
        });
    };

    clienteServiceFactory.getClientes = _getClientes;
    clienteServiceFactory.getCliente = _getCliente;
    clienteServiceFactory.createCliente = _createCliente;
    clienteServiceFactory.updateCliente = _updateCliente;
    clienteServiceFactory.deleteCliente = _deleteCliente;

    return clienteServiceFactory;

}]);