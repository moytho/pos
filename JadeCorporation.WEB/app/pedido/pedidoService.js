'use strict';
app.factory('pedidoService', ['$http', 'CONFIG', function ($http, CONFIG) {
    var serviceBase = CONFIG.SERVICE_BASE;
    var pedidoServiceFactory = {};

    var _createPedido = function (pedido) {
        return $http.post(serviceBase + 'api/pedidos/', pedido).then(function (results) {
            return results;
        });
    };

    
    pedidoServiceFactory.createPedido = _createPedido;
    return pedidoServiceFactory;

}]);