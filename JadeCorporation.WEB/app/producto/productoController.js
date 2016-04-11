'use strict';

app.controller('productoImagenes', ['$location', '$scope', '$http', '$timeout', '$upload', '$routeParams', 'CONFIG', 'toastr', function ($location,$scope, $http, $timeout, $upload, $routeParams, CONFIG, toastr) {
    var CodigoProducto = $routeParams.codigo;
    $scope.upload = [];
    $scope.fileUploadObj = { CodigoProducto: CodigoProducto, Descripcion: "Test string 2" };
    //upload a file
    $scope.onFileSelect = function ($files) {
        //$files: an array of files selected, each file has name, size, and type.
        for (var i = 0; i < $files.length; i++) {
            var $file = $files[i];
            (function (index) {
                $scope.upload[index] = $upload.upload({
                    url: CONFIG.SERVICE_BASE+"/api/producto/uploadimagen/"+CodigoProducto, // webapi url
                    method: "POST",
                    data: { fileUploadObj: $scope.fileUploadObj },
                    file: $file
                }).progress(function (evt) {
                    // get upload percentage
                    console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
                }).success(function (data, status, headers, config) {
                    // file is uploaded successfully
                    if (status == 200) {
                        toastr.success("Datos creados correctamente", data);
                        $location.path('/producto/edit/'+CodigoProducto);
                    } else {
                        toastr.error("Ha sucedido un error", "Porfavor seleccione otra imagen");
                    }
                    //console.log(data);
                }).error(function (data, status, headers, config) {
                    // file failed to upload
                    console.log(data);
                });
            })(i);
        }
    }

    $scope.abortUpload = function (index) {
        $scope.upload[index].abort();
    }
}]);

app.controller('productoController', ['$scope', '$window', 'productoService', 'toastr',
    function ($scope, $window,productoService, toastr) {
    
    $scope.productos = [];
    
    productoService.getProductos().then(function (results) {
        $scope.productos = results.data;

    }, function (error) {
        console.log(error);
    });
    //Cerrar loading splash
    //$window.loading_screen.finish();
}]);

app.controller('productoEditar', ['$scope', '$window', 'productoService', 'productoMarcaService', 'productoClasificacionService', 'productoAbastecimientoService', '$routeParams', '$location', 'toastr', 'CONFIG', function ($scope, $window, productoService, productoMarcaService, productoClasificacionService, productoAbastecimientoService, $routeParams, $location, toastr, CONFIG) {
    $scope.editar = true;
    $scope.service_base = CONFIG.SERVICE_BASE;
    var codigo = $routeParams.codigo;
    var codigoProductoImagen = 0;
    $scope.productoMarcas = [];
    $scope.productoClasificaciones = [];
    $scope.productoAbastecimientos = [];
    $scope.productoImagenes = [];
    $scope.producto = {};

    $scope.eliminarProductoImagen = function (codigoProductoImagen, index) {
        productoService.deleteProductoImagen(codigoProductoImagen).then(function (results) {
            if (results.status == 200) {
                toastr.success("Imagen eliminada correctamente de este producto", "Correcto");
                //Mostramos todas las imagenes nuevamente
                productoService.getProductoImagenes(codigo).then(function (results) {
                    $scope.productoImagenes = results.data;

                }, function (error) {
                    console.log(error);
                });
            }
            
        }, function (error) {
            console.log(error);
        });
        //$('#divProductoImagen' + index).fadeOut('slow');
    };

    $scope.convertirProductoImagenPrincipal = function (codigoProductoImagen, index) {
        productoService.setProductoImagenConvertirPrincipal(codigoProductoImagen,codigo).then(function (results) {
            if (results.status == 200) {
                toastr.success("Imagen establecida correctamente como principal de este producto", "Correcto");
                //Mostramos todas las imagenes nuevamente
                productoService.getProductoImagenes(codigo).then(function (results) {
                    $scope.productoImagenes = results.data;

                }, function (error) {
                    console.log(error);
                });
            }
        }, function (error) {
            console.log(error);
        });
        //$('#principal' + index).fadeIn('fast');
        //$('#secundaria' + index).fadeOut('slow');
    };

    productoService.getProductoImagenes(codigo).then(function (results) {
        $scope.productoImagenes = results.data;
    }, function (error) {
        console.log(error);
    });
    productoClasificacionService.getProductoClasificaciones().then(function (results) {
        $scope.productoClasificaciones = results.data;

        productoMarcaService.getProductoMarcas().then(function (results) {
            $scope.productoMarcas = results.data;

            productoAbastecimientoService.getProductoAbastecimientos().then(function (results) {
                $scope.productoAbastecimientos = results.data;

                productoService.getProducto(codigo).then(function (results) {
                    $scope.producto = results.data[0];
                }, function (error) {
                    console.log(error);
                });
            }, function (error) {
                console.log(error);
            });
        }, function (error) {
            console.log(error);
        });
    }, function (error) {
        console.log(error);
    });

    $scope.goBack = function () {
        $location.path('/producto');
    };

    $scope.update = function () {

        productoService.updateProducto(codigo, $scope.producto).then(function (results) {
            if (results.status == 204) {
                toastr.success("Datos actualizados correctamente", "Correcto");
                $location.path('/producto');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

    $scope.delete = function () {
        productoService.deleteProducto(codigo).then(function (results) {
            if (results.status == 200) {
                toastr.success("Datos eliminados correctamente", "Correcto");
                $location.path('/producto');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            }
        }, function (error) {
            console.log(error);
        });
    };

}]);

app.controller('productoCrear', ['$scope', '$window', 'productoService', 'productoMarcaService', 'productoClasificacionService', 'productoAbastecimientoService', '$routeParams', '$location', 'toastr', function ($scope, $window, productoService, productoMarcaService, productoClasificacionService, productoAbastecimientoService,$routeParams, $location, toastr) {
    $scope.editar = false;
    $scope.productoMarcas = [];
    $scope.productoClasificaciones = [];
    $scope.productoAbastecimientos = [];
    $scope.producto = {};

    productoClasificacionService.getProductoClasificaciones().then(function (results) {
        $scope.productoClasificaciones = results.data;
        
    }, function (error) {
        console.log(error);
    });

    productoMarcaService.getProductoMarcas().then(function (results) {
        $scope.productoMarcas = results.data;

    }, function (error) {
        console.log(error);
    });

    productoService.getProductoAbastecimientos().then(function (results) {
        $scope.productoAbastecimientos = results.data;
    }, function (error) {
        console.log(error);
    });

    $scope.create = function () {
        productoService.createProducto($scope.producto).then(function (results) {
            if (results.status == 201) {
                toastr.success("Datos creados correctamente", "Correcto");
                $location.path('/producto');
            } else {
                toastr.error("Ha sucedido un error", "Error");
            } 
        }, function (error) {
            console.log(error);
        });
    };

    $scope.goBack = function () {
        $location.path('/producto');
    };

}]);