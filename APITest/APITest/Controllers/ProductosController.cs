using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using APITest.Models;
using System.Web;
using APITest.Conexion;
using System.Configuration;
using AutoMapper;

namespace APITest.Controllers
{
    [Authorize]
    public class ProductosController : ApiController
    {
        private JadeCore1Entities db = new JadeCore1Entities();
        private string connectionString = "";
        private string UserId = "";
        // GET api/Productos
        //Este tendra que ser modificado, para consultar globalmente (todas las sucursales) o
        //solo la sucursal local
        [AllowAnonymous]
        [Route("api/productotipos")]
        public IHttpActionResult GetProductoAbastecimientoes()
        {
            var query = (from productoTipo in db.ProductoTipoes
                         orderby productoTipo.Descripcion
                         select new ProductoTipoDTO()
                         {
                             CodigoProductoTipo = productoTipo.CodigoProductoTipo,
                             Descripcion= productoTipo.Descripcion
                         }).ToList();
            if (query == null)
            {
                return NotFound();
            }

            return Ok(query);
        }
        public IHttpActionResult GetProductoes()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            //Esta clase nos retornara 
            //- empresa a la que pertenece el usuario y 
            //-su respectiva cadena de conexion
            //-tambien si posee permiso para realizar esta accion
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductosController");
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
                                                    where (producto.CodigoEmpresa == conexion.CodigoEmpresa && producto.Estado == true)
                                                    orderby producto.Nombre
                                                    select new ProductoInformacion { 
                                                        CodigoProducto= producto.CodigoProducto, 
                                                        CodigoLocal=producto.CodigoLocal,
                                                        Marca=productomarca.Nombre, 
                                                        Clasificacion=productoclasificacion.Descripcion,
                                                        Nombre=producto.Nombre,
                                                        PrecioCosto=producto.PrecioCosto,
                                                        Estado=producto.Estado,
                                                        PrecioVenta=producto.PrecioVenta
  
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

        // GET api/Productos/5
        [ResponseType(typeof(Producto))]
        public IHttpActionResult GetProducto(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "SucursalesController");

            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        //una manera diferente de obtener la(s) sucursal(es) y convertir a la clase SucursalDTO
                        var query = (from producto in db.Productoes
                                     where (producto.CodigoProducto == id && producto.CodigoEmpresa == conexion.CodigoEmpresa)
                                     orderby producto.Nombre
                                     select new ProductoDTO()
                                     {
                                         CodigoEmpresa = producto.CodigoEmpresa,
                                         CodigoProducto = producto.CodigoProducto,
                                         Nombre = producto.Nombre,
                                         CodigoLocal = producto.CodigoLocal,
                                         CodigoProductoMarca = producto.CodigoProductoMarca,
                                         CodigoProductoClasificacion = producto.CodigoProductoClasificacion,
                                         Descripcion = producto.Descripcion,
                                         PrecioCosto=producto.PrecioCosto,
                                         PrecioVenta=producto.PrecioVenta,
                                         CodigoProductoTipo=producto.CodigoProductoTipo,
                                         Estado = producto.Estado
                                     }).ToList();
                        if (query == null)
                        {
                            return NotFound();
                        }

                        return Ok(query);
                    }
                }
                catch (Exception exception)
                {
                    return InternalServerError();
                }
            }
            else return Unauthorized();
        }

        // PUT api/Productos/5
        public IHttpActionResult PutProducto(int id, Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != producto.CodigoProducto)
            {
                return BadRequest();
            }
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "SucursalessController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
                    producto.CodigoEmpresa = conexion.CodigoEmpresa;
                    db.Entry(producto).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductoExists(id,conexion.NameConnectionString))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            else return Unauthorized();
        }

        // POST api/Productos
        //[ResponseType(typeof(Producto))]
        public IHttpActionResult PostProducto(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "SucursalesController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {

                    producto.CodigoEmpresa = conexion.CodigoEmpresa;
                    producto.Estado = true;
                    //convirtiendo el objecto clasificacion a null para que no haga un insert de este objeto
                    producto.ProductoClasificacion = null;
                    //convirtiendo el objecto marca a null para que no haga un insert de este objeto
                    producto.ProductoMarca = null;
                    db.Productoes.Add(producto);
                    db.SaveChanges();
                    Mapper.CreateMap<Producto, ProductoDTO>();
                    ProductoDTO productoDTO = Mapper.Map<Producto, ProductoDTO>(producto);
                    return CreatedAtRoute("DefaultApi", new { id = producto.CodigoProducto }, productoDTO);
                }
            }
            else return Unauthorized();
        }

        // DELETE api/Productos/5
        [ResponseType(typeof(Producto))]
        public IHttpActionResult DeleteProducto(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "SucursalessController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
                    Producto producto = db.Productoes.Find(id);
                    if (producto == null)
                    {
                        return NotFound();
                    }

                    producto.Estado=false;
                    //db.Productoes.Remove(producto);
                    db.SaveChanges();
                    Mapper.CreateMap<Producto, ProductoDTO>();
                    ProductoDTO productoDTO = Mapper.Map<Producto, ProductoDTO>(producto);
                    
                    return Ok(productoDTO);
                    //}
                }
            }
            else return Unauthorized();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductoExists(int id, string nameConnectionString)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.Productoes.Count(e => e.CodigoProducto == id) > 0;
            }
        }

        private bool ProductoBelongsToYourCompany(int id, string nameConnectionString, int CodigoEmpresa)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.Productoes.Count(e => e.CodigoProducto == id && e.CodigoEmpresa == CodigoEmpresa) > 0;
            }
        }
    }
}