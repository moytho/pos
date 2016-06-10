'use strict';
app.factory('facturaService', ['$http', 'CONFIG', function ($http, CONFIG) {
    var serviceBase = CONFIG.SERVICE_BASE;
    var facturaServiceFactory = {};

    var _createFactura = function (factura) {
        return $http.post(serviceBase + 'api/facturas/', factura).then(function (results) {
            return results;
        });
    };

    
    facturaServiceFactory.createFactura= _createFactura;
    return facturaServiceFactory;

}]);