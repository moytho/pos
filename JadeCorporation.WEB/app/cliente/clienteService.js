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

    var _getClientePorNombre = function (nombre) {

        return $http.get(serviceBase + 'api/clientes/pornombre/' + nombre).then(function (results) {
            return results;
        });
    };

    var _getClientePorIdentificacion = function (identificacion) {

        return $http.get(serviceBase + 'api/clientes/poridentificacion/' + identificacion).then(function (results) {
            return results;
        });
    };

    var _getClientePorNombreEIdentificacion = function (busqueda) {

        return $http.get(serviceBase + 'api/clientespornombreeidentificacion/?busqueda=' + busqueda).then(function (results) {
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
    clienteServiceFactory.getClientePorNombre = _getClientePorNombre;
    clienteServiceFactory.getClientePorIdentificacion = _getClientePorIdentificacion;
    clienteServiceFactory.getClientePorNombreEIdentificacion = _getClientePorNombreEIdentificacion;
    clienteServiceFactory.createCliente = _createCliente;
    clienteServiceFactory.updateCliente = _updateCliente;
    clienteServiceFactory.deleteCliente = _deleteCliente;

    return clienteServiceFactory;

}]);