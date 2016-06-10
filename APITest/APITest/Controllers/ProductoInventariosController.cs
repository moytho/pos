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

        [Route("api/productoavenderporcodigo/")] //En este caso estaremos enviando SKU
        public IHttpActionResult GetProductoAVenderPorCodigo(string CodigoProducto)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
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
                                         where (
                                         (producto.CodigoEmpresa == conexion.CodigoEmpresa) &&
                                         (producto.Estado == true) &&
                                         (productoInventario.CodigoSucursal == conexion.CodigoSucursal) &&
                                         (producto.SKU == CodigoProducto)

                                         )
                                         orderby producto.Nombre
                                         select new ProductoInformacionCompleta
                                         {
                                             Cantidad=1,
                                             CodigoProducto = producto.CodigoProducto,
                                             CodigoLocal = producto.CodigoLocal,
                                             Marca = productomarca.Nombre,
                                             Clasificacion = productoclasificacion.Descripcion,
                                             Nombre = producto.Nombre,
                                             Sucursal = sucursal.Nombre,
                                             CodigoSucursal = sucursal.CodigoSucursal,
                                             Bodega = bodega.Nombre,
                                             Descripcion = producto.Descripcion,
                                             Existencia = productoInventario.Cantidad,
                                             ExistenciaMinima = producto.StockMinimo,
                                             Estado = producto.Estado,
                                             PrecioVenta = producto.PrecioVenta
                                         }).FirstOrDefault();
                        return Ok(productos);
                    }
                }
                catch (Exception exception)
                {
                    return InternalServerError();
                }
            }
            else return Unauthorized();
        }


        [Route("api/productoavenderpornombre/")]
        public IHttpActionResult GetProductoAVenderPorNombre(string Nombre)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
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
                                         where (
                                         (producto.CodigoEmpresa == conexion.CodigoEmpresa) &&
                                         (producto.Estado == true) &&
                                         (productoInventario.CodigoSucursal == conexion.CodigoSucursal) &&
                                         (producto.Nombre.Contains(Nombre))
                                         
                                         )
                                         orderby producto.Nombre
                                         select new ProductoInformacionCompleta
                                         {
                                             Cantidad=1,
                                             CodigoProducto = producto.CodigoProducto,
                                             CodigoLocal = producto.CodigoLocal,
                                             Marca = productomarca.Nombre,
                                             Clasificacion = productoclasificacion.Descripcion,
                                             Nombre = producto.Nombre,
                                             Sucursal = sucursal.Nombre,
                                             CodigoSucursal = sucursal.CodigoSucursal,
                                             Bodega = bodega.Nombre,
                                             Descripcion = producto.Descripcion,
                                             Existencia = productoInventario.Cantidad,
                                             ExistenciaMinima = producto.StockMinimo,
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

        [Route("api/productosapedir/")]
        public IHttpActionResult GetProductoInventariosEnEspera()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            //UserId="e5e7523c-8f71-4bd7-a96c-1a4a2b1fdc93";
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoInventariosController");
            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        db.SetConnectionString(connectionString);

                        var query = (from productoapedir in db.ProductoAPedirs
                                     join producto in db.Productoes on
                                         productoapedir.CodigoProducto equals producto.CodigoProducto
                                         into joinTable
                                     from b in
                                         joinTable.Where(c =>
                                             productoapedir.CodigoEmpresa == conexion.CodigoEmpresa &&
                                             productoapedir.CodigoSucursal == conexion.CodigoSucursal &&
                                             productoapedir.Estado == true
                                        ).DefaultIfEmpty()
                                     select new ProductoAPedirDTO { 
                                     CodigoEmpresa = productoapedir.CodigoEmpresa,
                                     CodigoSucursal = productoapedir.CodigoSucursal,
                                     CodigoProductoAPedir = productoapedir.CodigoProductoAPedir,
                                     CodigoProducto = productoapedir.CodigoProducto,
                                     Nombre = b.Nombre
                                     }).ToList();
                        

                        
                        return Ok(query);
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
         

        [Route("api/productoapedir/")]
         public IHttpActionResult PostProductoInventariosEnEspera(Producto producto)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            //UserId="e5e7523c-8f71-4bd7-a96c-1a4a2b1fdc93";
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoInventariosController");
            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        db.SetConnectionString(connectionString);

                        var query = (from productoapedir in db.ProductoAPedirs
                                     where (
                                     productoapedir.CodigoEmpresa == conexion.CodigoEmpresa &&
                                     productoapedir.CodigoSucursal== conexion.CodigoSucursal &&
                                     productoapedir.CodigoProductoAPedir == producto.CodigoProducto &&
                                     productoapedir.Estado == true
                                     )
                                         select productoapedir);
                        if (query != null)
                        {
                            return BadRequest("Error: El producto ya se encuentra en espera.");
                        }
                        
                        ProductoAPedir productoAPedir = new ProductoAPedir();
                        productoAPedir.CodigoEmpresa = conexion.CodigoEmpresa;
                        productoAPedir.CodigoSucursal = conexion.CodigoSucursal;
                        productoAPedir.Estado = true;
                        productoAPedir.CodigoProducto = producto.CodigoProducto;
                        productoAPedir.UserId = conexion.UserId;
                        productoAPedir.Sucursal = null;
                        productoAPedir.Producto = null;

                        productoAPedir.FechaCreacion = DateTime.Now;

                        db.ProductoAPedirs.Add(productoAPedir);
                        db.SaveChanges();
                    
                        return Ok();
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
            
            public IHttpActionResult GetProductoInventarios([FromUri]ProductoInventarioBusquedaFiltros filtro)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            //UserId="e5e7523c-8f71-4bd7-a96c-1a4a2b1fdc93";
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
                                         where (
                                         (producto.CodigoEmpresa == conexion.CodigoEmpresa) && 
                                         (producto.Estado == true) && 
                                         (productoInventario.CodigoSucursal==conexion.CodigoSucursal) &&
                                         (filtro.CodigoLocal == null || producto.CodigoLocal.Contains(filtro.CodigoLocal)) && //solo codigo local y nombre producto pueden ser 
                                         (filtro.SKU == null || producto.SKU.Contains(filtro.SKU)) && //solo codigo local y nombre producto pueden ser 
                                         (filtro.NombreProducto ==null || producto.Nombre.Contains(filtro.NombreProducto)) &&
                                         (filtro.CodigoBodega==0 || bodega.CodigoBodega== filtro.CodigoBodega) &&
                                         (filtro.CodigoProductoMarca == 0 || producto.CodigoProductoMarca == filtro.CodigoProductoMarca) &&
                                         (filtro.CodigoProductoClasificacion == 0 || producto.CodigoProductoClasificacion == filtro.CodigoProductoClasificacion) &&
                                         (filtro.CodigoProductoSubClasificacion == 0 || producto.CodigoProductoSubClasificacion == filtro.CodigoProductoSubClasificacion) &&
                                         (filtro.CodigoProductoAbastecimiento == 0 || producto.CodigoProductoAbastecimiento == filtro.CodigoProductoAbastecimiento)
                                         
                                         )
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
