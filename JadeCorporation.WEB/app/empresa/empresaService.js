'use strict';
app.factory('empresaService', ['$http', 'CONFIG', function ($http, CONFIG) {
    var serviceBase = CONFIG.SERVICE_BASE;
    var empresaServiceFactory = {};

    var _getEmpresas = function () {

        return $http.get(serviceBase + 'api/empresas').then(function (results) {
            return results;
        });
    };

    var _getEmpresa = function (codigo) {

        return $http.get(serviceBase + 'api/empresas/'+ codigo).then(function (results) {
            return results;
        });
    };

    var _updateEmpresa = function (codigo,empresa) {

        return $http.put(serviceBase + 'api/empresas/'+codigo,empresa).then(function (results) {
            return results;
        });
    };

    var _createEmpresa = function (empresa) {

        return $http.post(serviceBase + 'api/empresas',empresa).then(function (results) {
            return results;
        });
    };

    var _deleteEmpresa = function (codigo,empresa) {

        return $http.delete(serviceBase + 'api/empresas/' + codigo).then(function (results) {
            return results;
        });
    };

    empresaServiceFactory.getEmpresas = _getEmpresas;
    empresaServiceFactory.getEmpresa = _getEmpresa;
    empresaServiceFactory.createEmpresa = _createEmpresa;
    empresaServiceFactory.updateEmpresa = _updateEmpresa;
    empresaServiceFactory.deleteEmpresa = _deleteEmpresa;

    return empresaServiceFactory;

}]);