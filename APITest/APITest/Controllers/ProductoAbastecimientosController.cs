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
using APITest.Conexion;
using System.Configuration;
using AutoMapper;
using System.Web;

namespace APITest.Controllers
{
    public class ProductoAbastecimientosController : ApiController
    {
        private JadeCore1Entities db = new JadeCore1Entities();
        private string connectionString = "";
        private string UserId = "";

        // GET api/ProductoAbastecimientos
        public IHttpActionResult GetProductoAbastecimientoes()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            //Esta clase nos retornara 
            //- empresa a la que pertenece el usuario y 
            //-su respectiva cadena de conexion
            //-tambien si posee permiso para realizar esta accion
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoAbastecimientosController");
            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        db.SetConnectionString(connectionString);
                        //Consulta para mostrar todas las sucursales pertenecientes a esta empresa
                        List<ProductoAbastecimiento> productoAbastecimientos = (from productoAbastecimiento in db.ProductoAbastecimientoes
                                                              where (productoAbastecimiento.CodigoEmpresa == conexion.CodigoEmpresa && productoAbastecimiento.Estado == true)
                                                              orderby productoAbastecimiento.Descripcion
                                                              select productoAbastecimiento).ToList();
                        //Convirtiendo la Clase sucursal a sucursalDTO
                        //Esto con el afan de ocultar ciertos campos de nuestro objeto, si fuera el caso
                        Mapper.CreateMap<ProductoAbastecimiento, ProductoAbastecimientoDTO>();
                        List<ProductoAbastecimientoDTO> productoAbastecimientoDTO = Mapper.Map<List<ProductoAbastecimiento>, List<ProductoAbastecimientoDTO>>(productoAbastecimientos);
                        return Ok(productoAbastecimientoDTO);
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

        // GET api/ProductoAbastecimientos/5
        //[AbastecimientoResponseType(typeof(ProductoAbastecimiento))]
        public IHttpActionResult GetProductoAbastecimiento(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
        ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoMarcasController");

        if (conexion.PoseePermiso == 1)
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
                    //una manera diferente de obtener la(s) sucursal(es) y convertir a la clase SucursalDTO
                    var query = (from productoAbastecimiento in db.ProductoAbastecimientoes
                                 where (productoAbastecimiento.CodigoProductoAbastecimiento == id && productoAbastecimiento.CodigoEmpresa == conexion.CodigoEmpresa)
                                 orderby productoAbastecimiento.Descripcion
                                 select new ProductoAbastecimientoDTO()
                                 {
                                     CodigoEmpresa = productoAbastecimiento.CodigoEmpresa,
                                     CodigoProductoAbastecimiento = productoAbastecimiento.CodigoProductoAbastecimiento,
                                     Descripcion = productoAbastecimiento.Descripcion,
                                     DiasAbastecimiento=productoAbastecimiento.DiasAbastecimiento,
                                     Estado = productoAbastecimiento.Estado
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

        // PUT api/ProductoAbastecimientos/5
        public IHttpActionResult PutProductoAbastecimiento(int id, ProductoAbastecimiento productoabastecimiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productoabastecimiento.CodigoProductoAbastecimiento)
            {
                return BadRequest();
            }
            var user = base.ControllerContext.RequestContext.Principal.Identity;
            ClaseConexion conexion = new ClaseConexion(user.GetUserId().ToString(), this.GetType().FullName.ToString(), "ProductoAbastecimientosController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
                    //antes de actualizar verificamos que el usuario pertenezca a la misma empresa de la sucursal
                    if (!ProductoAbastecimientoBelongsToYourCompany(id, conexion.NameConnectionString, conexion.CodigoEmpresa))
                    {
                        return NotFound();
                    }
                    productoabastecimiento.CodigoEmpresa = conexion.CodigoEmpresa;
                    db.Entry(productoabastecimiento).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductoAbastecimientoExists(id, conexion.NameConnectionString))
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

        // POST api/ProductoAbastecimientos
        //[ResponseType(typeof(ProductoAbastecimiento))]
        public IHttpActionResult PostProductoAbastecimiento(ProductoAbastecimiento productoabastecimiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoAbastecimientosController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
                    productoabastecimiento.CodigoEmpresa = conexion.CodigoEmpresa;
                    productoabastecimiento.Estado = true;


                    db.ProductoAbastecimientoes.Add(productoabastecimiento);
                    db.SaveChanges();
                    Mapper.CreateMap<ProductoAbastecimiento, ProductoAbastecimientoDTO>();
                    ProductoAbastecimientoDTO productoAbastecimientoDTO = Mapper.Map<ProductoAbastecimiento, ProductoAbastecimientoDTO>(productoabastecimiento);

                    return CreatedAtRoute("DefaultApi", new { id = productoabastecimiento.CodigoProductoAbastecimiento }, productoAbastecimientoDTO);
                }
            }
            else return Unauthorized();
        }

        // DELETE api/ProductoAbastecimientos/5
        //[ResponseType(typeof(ProductoAbastecimiento))]
        public IHttpActionResult DeleteProductoAbastecimiento(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoAbastecimientosController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {

                    ProductoAbastecimiento productoabastecimiento = db.ProductoAbastecimientoes.Find(id, conexion.CodigoEmpresa);
                    if (productoabastecimiento == null)
                    {
                        return NotFound();
                    }
                    productoabastecimiento.Estado = false;
                    //db.ProductoMarcas.Remove(productomarca);
                    db.SaveChanges();
                    Mapper.CreateMap<ProductoAbastecimiento, ProductoAbastecimientoDTO>();
                    ProductoAbastecimientoDTO productoAbastecimientoDTO = Mapper.Map<ProductoAbastecimiento, ProductoAbastecimientoDTO>(productoabastecimiento);

                    return Ok(productoAbastecimientoDTO);
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

        private bool ProductoAbastecimientoExists(int id, string nameConnectionString)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.ProductoAbastecimientoes.Count(e => e.CodigoProductoAbastecimiento == id) > 0;
            }
        }

        private bool ProductoAbastecimientoBelongsToYourCompany(int id, string nameConnectionString, int CodigoEmpresa)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.ProductoAbastecimientoes.Count(e => e.CodigoEmpresa == CodigoEmpresa && e.CodigoProductoAbastecimiento==id) > 0;
            }
        }
    }
}