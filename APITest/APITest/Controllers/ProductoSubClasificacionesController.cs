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
    public class ProductoSubClasificacionesController : ApiController
    {
        private JadeCore1Entities db = new JadeCore1Entities();
        private string connectionString = "";
        private string UserId = "";

[Route("api/productosubclasificacionesporsuclasificacion/{id?}")]
        public IHttpActionResult GetProductoSubClasificacionesPorClasificacion(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            //UserId = "e5e7523c-8f71-4bd7-a96c-1a4a2b1fdc93";

            //Esta clase nos retornara 
            //- empresa a la que pertenece el usuario y 
            //-su respectiva cadena de conexion
            //-tambien si posee permiso para realizar esta accion
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoSubController");
            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        db.SetConnectionString(connectionString);
                        //Consulta para mostrar todas las sucursales pertenecientes a esta empresa
                        List<ProductoSubClasificacion> productoSubClasificaciones = (from productoSubClasificacion in db.ProductoSubClasificacions
                                                                                     where (productoSubClasificacion.CodigoEmpresa == conexion.CodigoEmpresa &&
                                                                                     productoSubClasificacion.Estado == true &&
                                                                                     productoSubClasificacion.CodigoProductoClasificacion == id)
                                                                                     orderby productoSubClasificacion.Descripcion
                                                                                     select productoSubClasificacion).ToList();
                        //Convirtiendo la Clase sucursal a sucursalDTO
                        //Esto con el afan de ocultar ciertos campos de nuestro objeto, si fuera el caso
                        Mapper.CreateMap<ProductoSubClasificacion, ProductoSubClasificacionDTO>();
                        List<ProductoSubClasificacionDTO> productoSubClasificacionesDTO = Mapper.Map<List<ProductoSubClasificacion>, List<ProductoSubClasificacionDTO>>(productoSubClasificaciones);
                        return Ok(productoSubClasificacionesDTO);
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

        // GET api/ProductoSubClasificaciones
        public IHttpActionResult GetProductoSubClasificacions()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            //UserId = "e5e7523c-8f71-4bd7-a96c-1a4a2b1fdc93";
            
            //Esta clase nos retornara 
            //- empresa a la que pertenece el usuario y 
            //-su respectiva cadena de conexion
            //-tambien si posee permiso para realizar esta accion
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoSubClasificacionesController");
            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        db.SetConnectionString(connectionString);
                        //Consulta para mostrar todas las sucursales pertenecientes a esta empresa
                        List<ProductoSubClasificacionDTO> productoSubClasificaciones = (from productoSubClasificacion in db.ProductoSubClasificacions
                                                                                     join productoClasificacion in db.ProductoClasificacions
                                                                                     on productoSubClasificacion.CodigoProductoClasificacion equals productoClasificacion.CodigoProductoClasificacion
                                                                                     into joinTable
                                                                                     from b in joinTable.Where (condicion=> productoSubClasificacion.CodigoEmpresa == conexion.CodigoEmpresa && productoSubClasificacion.Estado == true)
                                                                               orderby productoSubClasificacion.Descripcion
                                                                                        select new ProductoSubClasificacionDTO
                                                                                        {
                                                                                        CodigoEmpresa=productoSubClasificacion.CodigoEmpresa,
                                                                                        CodigoProductoClasificacion=productoSubClasificacion.CodigoProductoClasificacion,
                                                                                        CodigoProductoSubClasificacion= productoSubClasificacion.CodigoProductoSubClasificacion,
                                                                                        Descripcion= productoSubClasificacion.Descripcion,
                                                                                        ProductoClasificacionDescripcion= b.Descripcion,
                                                                                        Estado = productoSubClasificacion.Estado
                                                                                       
                                                                               }).ToList();
                        //Convirtiendo la Clase sucursal a sucursalDTO
                        //Esto con el afan de ocultar ciertos campos de nuestro objeto, si fuera el caso
                        //Mapper.CreateMap<ProductoSubClasificacion, ProductoSubClasificacionDTO>();
                        //List<ProductoSubClasificacionDTO> productoSubClasificacionesDTO = Mapper.Map<List<ProductoSubClasificacion>, List<ProductoSubClasificacionDTO>>(productoSubClasificaciones);
                        return Ok(productoSubClasificaciones);
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

        // GET api/ProductoSubClasificaciones/5
        //[ResponseType(typeof(ProductoSubClasificacion))]
        public IHttpActionResult GetProductoSubClasificacion(int id)
        {
            //UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            //UserId = "e5e7523c-8f71-4bd7-a96c-1a4a2b1fdc93";
            
            //Esta clase nos retornara 
            //- empresa a la que pertenece el usuario y 
            //-su respectiva cadena de conexion
            //-tambien si posee permiso para realizar esta accion
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoSubController");
            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        db.SetConnectionString(connectionString);
                        //Consulta para mostrar todas las sucursales pertenecientes a esta empresa
                        List<ProductoSubClasificacion> productoSubClasificaciones = (from productoSubClasificacion in db.ProductoSubClasificacions
                                                                               where (productoSubClasificacion.CodigoEmpresa == conexion.CodigoEmpresa && 
                                                                               productoSubClasificacion.Estado == true && 
                                                                               productoSubClasificacion.CodigoProductoSubClasificacion== id)
                                                                               orderby productoSubClasificacion.Descripcion
                                                                               select productoSubClasificacion).ToList();
                        //Convirtiendo la Clase sucursal a sucursalDTO
                        //Esto con el afan de ocultar ciertos campos de nuestro objeto, si fuera el caso
                        Mapper.CreateMap<ProductoSubClasificacion, ProductoSubClasificacionDTO>();
                        List<ProductoSubClasificacionDTO> productoSubClasificacionesDTO = Mapper.Map<List<ProductoSubClasificacion>, List<ProductoSubClasificacionDTO>>(productoSubClasificaciones);
                        return Ok(productoSubClasificacionesDTO);
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

        // PUT api/ProductoSubClasificaciones/5
        public IHttpActionResult PutProductoSubClasificacion(int id, ProductoSubClasificacion productosubclasificacion)
        {
            if (id != productosubclasificacion.CodigoProductoClasificacion)
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
                    productosubclasificacion.CodigoEmpresa = conexion.CodigoEmpresa;
                    productosubclasificacion.Estado = true;

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    //antes de actualizar verificamos que el usuario pertenezca a la misma empresa de la sucursal
                    if (!ProductoSubClasificacionBelongsToYourCompany(id, conexion.NameConnectionString, conexion.CodigoEmpresa))
                    {
                        return NotFound();
                    }

                    db.Entry(productosubclasificacion).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductoSubClasificacionExists(id, conexion.NameConnectionString))
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

        // POST api/ProductoSubClasificaciones
        [ResponseType(typeof(ProductoSubClasificacion))]
        public IHttpActionResult PostProductoSubClasificacion(ProductoSubClasificacion productosubclasificacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoSubClasificacionController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
                    productosubclasificacion.CodigoEmpresa = conexion.CodigoEmpresa;
                    productosubclasificacion.Estado = true;

                    db.ProductoSubClasificacions.Add(productosubclasificacion);
                    db.SaveChanges();
                    Mapper.CreateMap<ProductoSubClasificacion, ProductoSubClasificacionDTO>();
                    ProductoSubClasificacionDTO productoClasificacionDTO = Mapper.Map<ProductoSubClasificacion, ProductoSubClasificacionDTO>(productosubclasificacion);

                    return CreatedAtRoute("DefaultApi", new { id = productosubclasificacion.CodigoProductoSubClasificacion }, productosubclasificacion);
                }
            }
            else return Unauthorized();
        }

        // DELETE api/ProductoSubClasificaciones/5
        
        [ResponseType(typeof(ProductoSubClasificacion))]
        [HttpPost]
        [Route("api/deleteproductosubclasificaciones/{id?}")]
        public IHttpActionResult DeleteProductoSubClasificacion(int id, ProductoSubClasificacion productosubclasificacion)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoMarcasController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {

                    productosubclasificacion.CodigoEmpresa = conexion.CodigoEmpresa;
                    productosubclasificacion.Estado = false;
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    //antes de actualizar verificamos que el usuario pertenezca a la misma empresa de la sucursal
                    if (!ProductoSubClasificacionBelongsToYourCompany(id, conexion.NameConnectionString, conexion.CodigoEmpresa))
                    {
                        return NotFound();
                    }

                    db.Entry(productosubclasificacion).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductoSubClasificacionExists(id, conexion.NameConnectionString))
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductoSubClasificacionExists(int id, string nameConnectionString)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.ProductoSubClasificacions.Count(e => e.CodigoProductoSubClasificacion == id) > 0;
            }
        }

        private bool ProductoSubClasificacionBelongsToYourCompany(int id, string nameConnectionString, int CodigoEmpresa)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.ProductoSubClasificacions.Count(e => e.CodigoEmpresa == CodigoEmpresa && e.CodigoProductoSubClasificacion == id) > 0;
            }
        }
    }
}