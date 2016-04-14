var app = angular.module('AngularAuthApp', ['angular-confirm','ui.bootstrap', 'ngAnimate', 'ngRoute', 'toastr', 'LocalStorageModule', 'angular-loading-bar', 'ngMessages', 'angularFileUpload' ]);

app.config(function ($routeProvider,CONFIG) {

    $routeProvider.when("/home", {controller: "homeController",templateUrl: "app/sharedviews/home.html"});
    $routeProvider.when("/login", { redirectTo: function () { window.location = CONFIG.LOGIN_URL;}});
    $routeProvider.when("/registrarusuario", {controller: "signupController",templateUrl: "app/autenticacion/views/signup.html"});
    //un listado de todas las empresa GET api/empresas
    $routeProvider.when("/empresa", { controller: "empresaController", templateUrl: "app/empresa/views/empresa.html" });
    //una empresa especifica GET api/empresas/id?
    $routeProvider.when("/empresa/edit/:codigo", { controller: "empresaEditar", templateUrl: "app/empresa/views/empresa-view.html" });
    //
    $routeProvider.when("/empresa/create", { controller: "empresaCrear", templateUrl: "app/empresa/views/empresa-view.html" });
    $routeProvider.when("/sucursal", { controller: "sucursalController", templateUrl: "app/sucursal/views/sucursal.html" });
    $routeProvider.when("/sucursal/edit/:codigo", { controller: "sucursalEditar", templateUrl: "app/sucursal/views/sucursal-view.html" });
    $routeProvider.when("/sucursal/create", { controller: "sucursalCrear", templateUrl: "app/sucursal/views/sucursal-view.html" });

    $routeProvider.when("/sucursal", { controller: "sucursalController", templateUrl: "app/sucursal/views/sucursal.html" });
    $routeProvider.when("/sucursal/edit/:codigo", { controller: "sucursalEditar", templateUrl: "app/sucursal/views/sucursal-view.html" });
    $routeProvider.when("/sucursal/create", { controller: "sucursalCrear", templateUrl: "app/sucursal/views/sucursal-view.html" });

    $routeProvider.when("/producto", { controller: "productoController", templateUrl: "app/producto/views/producto.html" });
    $routeProvider.when("/producto/edit/:codigo", { controller: "productoEditar", templateUrl: "app/producto/views/producto-view.html" });
    $routeProvider.when("/producto/create", { controller: "productoCrear", templateUrl: "app/producto/views/producto-view.html" });
    $routeProvider.when("/producto/imagen/:codigo", { controller: "productoImagenes", templateUrl: "app/producto/views/productoImagenes-view.html" });
    $routeProvider.when("/producto/preciosespeciales/:codigo", { controller: "productoPreciosEspeciales", templateUrl: "app/producto/views/productoPreciosEspeciales-view.html" });

    $routeProvider.when("/productoMarca", { controller: "productoMarcaController", templateUrl: "app/productoMarca/views/productoMarca.html" });
    $routeProvider.when("/productoMarca/edit/:codigo", { controller: "productoMarcaEditar", templateUrl: "app/productoMarca/views/productoMarca-view.html" });
    $routeProvider.when("/productoMarca/create", { controller: "productoMarcaCrear", templateUrl: "app/productoMarca/views/productoMarca-view.html" });

    $routeProvider.when("/productoAbastecimiento", { controller: "productoAbastecimientoController", templateUrl: "app/productoAbastecimiento/views/productoAbastecimiento.html" });
    $routeProvider.when("/productoAbastecimiento/edit/:codigo", { controller: "productoAbastecimientoEditar", templateUrl: "app/productoAbastecimiento/views/productoAbastecimiento-view.html" });
    $routeProvider.when("/productoAbastecimiento/create", { controller: "productoAbastecimientoCrear", templateUrl: "app/productoAbastecimiento/views/productoAbastecimiento-view.html" });

    $routeProvider.when("/productoClasificacion", { controller: "productoClasificacionController", templateUrl: "app/productoClasificacion/views/productoClasificacion.html" });
    $routeProvider.when("/productoClasificacion/edit/:codigo", { controller: "productoClasificacionEditar", templateUrl: "app/productoClasificacion/views/productoClasificacion-view.html" });
    $routeProvider.when("/productoClasificacion/create", { controller: "productoClasificacionCrear", templateUrl: "app/productoClasificacion/views/productoClasificacion-view.html" });

    $routeProvider.when("/cliente", { controller: "clienteController", templateUrl: "app/cliente/views/cliente.html" });
    $routeProvider.when("/cliente/edit/:codigo", { controller: "clienteEditar", templateUrl: "app/cliente/views/cliente-view.html" });
    $routeProvider.when("/cliente/create", { controller: "clienteCrear", templateUrl: "app/cliente/views/cliente-view.html" });

    $routeProvider.when("/bodega", { controller: "bodegaController", templateUrl: "app/bodega/views/bodega.html" });
    $routeProvider.when("/bodega/edit/:codigo", { controller: "bodegaEditar", templateUrl: "app/bodega/views/bodega-view.html" });
    $routeProvider.when("/bodega/create", { controller: "bodegaCrear", templateUrl: "app/bodega/views/bodega-view.html" });

    $routeProvider.when("/clienteTipo", { controller: "clienteTipoController", templateUrl: "app/clienteTipo/views/clienteTipo.html" });
    $routeProvider.when("/clienteTipo/edit/:codigo", { controller: "clienteTipoEditar", templateUrl: "app/clienteTipo/views/clienteTipo-view.html" });
    $routeProvider.when("/clienteTipo/create", { controller: "clienteTipoCrear", templateUrl: "app/clienteTipo/views/clienteTipo-view.html" });

    $routeProvider.otherwise({ redirectTo: "/home" });
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});