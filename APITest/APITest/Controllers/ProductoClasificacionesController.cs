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
    public class ProductoClasificacionesController : ApiController
    {
        private JadeCore1Entities db = new JadeCore1Entities();
        private string connectionString = "";
        private string UserId = "";

        // GET api/ProductoClasificaciones
        public IHttpActionResult GetProductoClasificacions()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            //Esta clase nos retornara 
            //- empresa a la que pertenece el usuario y 
            //-su respectiva cadena de conexion
            //-tambien si posee permiso para realizar esta accion
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ScursalessController");
            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        db.SetConnectionString(connectionString);
                        //Consulta para mostrar todas las sucursales pertenecientes a esta empresa
                        List<ProductoClasificacion> productoClasificaciones = (from productoClasificacion in db.ProductoClasificacions
                                                                               where (productoClasificacion.CodigoEmpresa == conexion.CodigoEmpresa && productoClasificacion.Estado == true)
                                                     orderby productoClasificacion.Descripcion
                                                     select productoClasificacion).ToList();
                        //Convirtiendo la Clase sucursal a sucursalDTO
                        //Esto con el afan de ocultar ciertos campos de nuestro objeto, si fuera el caso
                        Mapper.CreateMap<ProductoClasificacion, ProductoClasificacionDTO>();
                        List<ProductoClasificacionDTO> productoClasificacionesDTO = Mapper.Map<List<ProductoClasificacion>, List<ProductoClasificacionDTO>>(productoClasificaciones);
                        return Ok(productoClasificacionesDTO);
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

        // GET api/ProductoClasificaciones/5
        //[ResponseType(typeof(ProductoClasificacion))]
        public IHttpActionResult GetProductoClasificacion(int id)
        {
            //return Ok();
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoClasificacionesController");

            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        //una manera diferente de obtener la(s) sucursal(es) y convertir a la clase SucursalDTO
                        var query = (from productoClasificacion in db.ProductoClasificacions
                                     where (productoClasificacion.CodigoProductoClasificacion == id && productoClasificacion.CodigoEmpresa == conexion.CodigoEmpresa)
                                     orderby productoClasificacion.Descripcion
                                     select new ProductoClasificacionDTO()
                                     {
                                         CodigoEmpresa = productoClasificacion.CodigoEmpresa,
                                         CodigoProductoClasificacion = productoClasificacion.CodigoProductoClasificacion,
                                         CodigoClasificacionPadre = productoClasificacion.CodigoClasificacionPadre,
                                         Descripcion = productoClasificacion.Descripcion,
                                         Estado = productoClasificacion.Estado
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

        // PUT api/ProductoClasificaciones/5
        public IHttpActionResult PutProductoClasificacion(int id, ProductoClasificacion productoclasificacion)
        {
            if (id != productoclasificacion.CodigoProductoClasificacion)
            {
                return BadRequest();
            }
            var user = base.ControllerContext.RequestContext.Principal.Identity;
            ClaseConexion conexion = new ClaseConexion(user.GetUserId().ToString(), this.GetType().FullName.ToString(), "ProductoMarcasController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
                    productoclasificacion.CodigoEmpresa = conexion.CodigoEmpresa;
                    productoclasificacion.Estado = true;
                    
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
 
                    //antes de actualizar verificamos que el usuario pertenezca a la misma empresa de la sucursal
                    if (!ProductoClasificacionBelongsToYourCompany(id,conexion.NameConnectionString, conexion.CodigoEmpresa))
                    {
                        return NotFound();
                    }
            
                    db.Entry(productoclasificacion).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductoClasificacionExists(id,conexion.NameConnectionString))
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

        // POST api/ProductoClasificaciones
        [ResponseType(typeof(ProductoClasificacion))]
        public IHttpActionResult PostProductoClasificacion(ProductoClasificacion productoclasificacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoMarcasController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
                    productoclasificacion.CodigoEmpresa = conexion.CodigoEmpresa;
                    productoclasificacion.Estado = true;
                    
                    db.ProductoClasificacions.Add(productoclasificacion);
                    db.SaveChanges();
                    Mapper.CreateMap<ProductoClasificacion, ProductoClasificacionDTO>();
                    ProductoClasificacionDTO productoClasificacionDTO = Mapper.Map<ProductoClasificacion, ProductoClasificacionDTO>(productoclasificacion);
                    
                    return CreatedAtRoute("DefaultApi", new { id = productoclasificacion.CodigoProductoClasificacion }, productoclasificacion);
                }
            }
            else return Unauthorized();
        }

        // DELETE api/ProductoClasificaciones/5
        [ResponseType(typeof(ProductoClasificacion))]
        public IHttpActionResult DeleteProductoClasificacion(int id)
        {
              UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoMarcasController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
          
                    ProductoClasificacion productoclasificacion = db.ProductoClasificacions.Find(id,conexion.CodigoEmpresa);
                    if (productoclasificacion == null)
                    {
                        return NotFound();
                    }
                    productoclasificacion.Estado = false;
                    //db.ProductoClasificacions.Remove(productoclasificacion);
                    db.SaveChanges();

                    Mapper.CreateMap<ProductoClasificacion, ProductoClasificacionDTO>();
                    ProductoClasificacionDTO productoClasificacionDTO = Mapper.Map<ProductoClasificacion, ProductoClasificacionDTO>(productoclasificacion);

                    return Ok(productoClasificacionDTO);
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

        private bool ProductoClasificacionExists(int id, string nameConnectionString)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.ProductoClasificacions.Count(e => e.CodigoProductoClasificacion == id) > 0;
            }
        }

        private bool ProductoClasificacionBelongsToYourCompany(int id, string nameConnectionString,int CodigoEmpresa)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.ProductoClasificacions.Count(e => e.CodigoEmpresa== CodigoEmpresa && e.CodigoProductoClasificacion==id) > 0;
            }
        }
    }
}