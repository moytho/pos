using APITest.Conexion;
using APITest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace APITest.Controllers
{
    public class ProductoInventariosController : ApiController
    {
        private string connectionString = "";
        private string UserId = "";
        [AllowAnonymous]
        public IHttpActionResult GetProductoes()
        {
            //UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            UserId="e5e7523c-8f71-4bd7-a96c-1a4a2b1fdc93";
            //Esta clase nos retornara 
            //- empresa a la que pertenece el usuario y 
            //-su respectiva cadena de conexion
            //-tambien si posee permiso para realizar esta accion
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoInventariosController");
            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        db.SetConnectionString(connectionString);
                        //Consulta para mostrar todas las sucursales pertenecientes a esta empresa
                        var productos = (from producto in db.Productoes
                                         join productomarca in db.ProductoMarcas
                                         on producto.CodigoProductoMarca equals productomarca.CodigoProductoMarca
                                         join productoclasificacion in db.ProductoClasificacions
                                         on producto.CodigoProductoClasificacion equals productoclasificacion.CodigoProductoClasificacion
                                         join productoInventario in db.ProductoInventarios
                                         on producto.CodigoProducto equals productoInventario.CodigoProducto
                                         join bodega in db.Bodegas
                                         on productoInventario.CodigoBodega equals bodega.CodigoBodega
                                         join sucursal in db.Sucursals
                                         on productoInventario.CodigoSucursal equals sucursal.CodigoSucursal
                                         where (producto.CodigoEmpresa == conexion.CodigoEmpresa && producto.Estado == true && productoInventario.CodigoSucursal==conexion.CodigoSucursal)
                                         orderby producto.Nombre
                                         select new ProductoInformacionCompleta
                                         {
                                             CodigoProducto = producto.CodigoProducto,
                                             CodigoLocal = producto.CodigoLocal,
                                             Marca = productomarca.Nombre,
                                             Clasificacion = productoclasificacion.Descripcion,
                                             Nombre = producto.Nombre,
                                             Sucursal = sucursal.Nombre,
                                             CodigoSucursal = sucursal.CodigoSucursal,
                                             Bodega=bodega.Nombre,
                                             Descripcion = producto.Descripcion,
                                             Existencia = productoInventario.Cantidad,
                                             ExistenciaMinima=producto.StockMinimo,
                                             Estado = producto.Estado,
                                             
                                             PrecioVenta = producto.PrecioVenta

                                         }).ToList();
                        //Convirtiendo la Clase sucursal a sucursalDTO
                        //Esto con el afan de ocultar ciertos campos de nuestro objeto, si fuera el caso
                        //Mapper.CreateMap<ProductoYMas, ProductoYMasDTO>();
                        //List<ProductoYMasDTO> productosYMasDTO = Mapper.Map<List<ProductoYMas>, List<ProductoYMasDTO>>(productos);
                        return Ok(productos);
                    }
                }
                catch (Exception exception)
                {
                    return InternalServerError();
                }
            }
            //si no posee permiso retornamos el estado Unauthorized 501
            else return Unauthorized();

        }
    }
}
