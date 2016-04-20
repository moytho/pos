'use strict';
app.factory('productoInventarioService', ['$http', 'CONFIG', function ($http, CONFIG) {
    var serviceBase = CONFIG.SERVICE_BASE;
    var productoInventarioServiceFactory = {};

    var _getProductos = function (filter) {

        return $http.get(serviceBase + 'api/productoinventarios/', { params: filter }).then(function (results) {
            return results;
        });
    };

    productoInventarioServiceFactory.getProductos = _getProductos;

    return productoInventarioServiceFactory;

}]);