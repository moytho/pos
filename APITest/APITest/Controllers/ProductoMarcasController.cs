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
    public class ProductoMarcasController : ApiController
    {
        private JadeCore1Entities db = new JadeCore1Entities();
        private string connectionString = "";
        private string UserId = "";

        // GET api/ProductoMarcas
        public IHttpActionResult GetProductoMarcas()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            //Esta clase nos retornara 
            //- empresa a la que pertenece el usuario y 
            //-su respectiva cadena de conexion
            //-tambien si posee permiso para realizar esta accion
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoMarcasController");
            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        db.SetConnectionString(connectionString);
                        //Consulta para mostrar todas las sucursales pertenecientes a esta empresa
                        List<ProductoMarca> productoMarcas = (from productoMarca in db.ProductoMarcas
                                                              where (productoMarca.CodigoEmpresa == conexion.CodigoEmpresa && productoMarca.Estado == true)
                                                              orderby productoMarca.Nombre
                                                              select productoMarca).ToList();
                        //Convirtiendo la Clase sucursal a sucursalDTO
                        //Esto con el afan de ocultar ciertos campos de nuestro objeto, si fuera el caso
                        Mapper.CreateMap<ProductoMarca, ProductoMarcaDTO>();
                        List<ProductoMarcaDTO> productoMarcaDTO = Mapper.Map<List<ProductoMarca>, List<ProductoMarcaDTO>>(productoMarcas);
                        return Ok(productoMarcaDTO);
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

        // GET api/ProductoMarcas/5
        //[ResponseType(typeof(ProductoMarca))]
        public IHttpActionResult GetProductoMarca(int id)
        {
            //return Ok();
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
                        var query = (from productoMarca in db.ProductoMarcas
                                     where (productoMarca.CodigoProductoMarca == id && productoMarca.CodigoEmpresa == conexion.CodigoEmpresa)
                                     orderby productoMarca.Nombre
                                     select new ProductoMarcaDTO()
                                     {
                                         CodigoEmpresa = productoMarca.CodigoEmpresa,
                                         CodigoProductoMarca = productoMarca.CodigoProductoMarca,
                                         Nombre = productoMarca.Nombre,
                                         Estado = productoMarca.Estado
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

        // PUT api/ProductoMarcas/5
        public IHttpActionResult PutProductoMarca(int id, ProductoMarca productomarca)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productomarca.CodigoProductoMarca)
            {
                return BadRequest();
            }
            var user = base.ControllerContext.RequestContext.Principal.Identity;
            ClaseConexion conexion = new ClaseConexion(user.GetUserId().ToString(), this.GetType().FullName.ToString(), "SucursalesController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
                    //antes de actualizar verificamos que el usuario pertenezca a la misma empresa de la sucursal
                    if (!ProductoMarcaBelongsToYourCompany(id, conexion.NameConnectionString, conexion.CodigoEmpresa))
                    {
                        return NotFound();
                    }
                    productomarca.CodigoEmpresa = conexion.CodigoEmpresa;
                    db.Entry(productomarca).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductoMarcaExists(id,conexion.NameConnectionString))
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

        // POST api/ProductoMarcas
        [ResponseType(typeof(ProductoMarca))]
        public IHttpActionResult PostProductoMarca(ProductoMarca productomarca)
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
                    productomarca.CodigoEmpresa = conexion.CodigoEmpresa;
                    productomarca.Estado = true;
            

                    db.ProductoMarcas.Add(productomarca);
                    db.SaveChanges();
                    Mapper.CreateMap<ProductoMarca, ProductoMarcaDTO>();
                    ProductoMarcaDTO productoMarcaDTO = Mapper.Map<ProductoMarca, ProductoMarcaDTO>(productomarca);
                    
                return CreatedAtRoute("DefaultApi", new { id = productomarca.CodigoProductoMarca }, productoMarcaDTO);
                }
            }
            else return Unauthorized();
        }

        // DELETE api/ProductoMarcas/5
        [ResponseType(typeof(ProductoMarca))]
        public IHttpActionResult DeleteProductoMarca(int id)
        {
              UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ProductoMarcasController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
          
                    ProductoMarca productomarca = db.ProductoMarcas.Find(id,conexion.CodigoEmpresa);
                    if (productomarca == null)
                    {
                        return NotFound();
                    }
                    productomarca.Estado = false;
                    //db.ProductoMarcas.Remove(productomarca);
                    db.SaveChanges();
                    Mapper.CreateMap<ProductoMarca, ProductoMarcaDTO>();
                    ProductoMarcaDTO productoMarcaDTO = Mapper.Map<ProductoMarca, ProductoMarcaDTO>(productomarca);
                    
                    return Ok(productoMarcaDTO);
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

        private bool ProductoMarcaExists(int id, string nameConnectionString)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.ProductoMarcas.Count(e => e.CodigoProductoMarca == id) > 0;
            }
        }

        private bool ProductoMarcaBelongsToYourCompany(int id, string nameConnectionString, int CodigoEmpresa)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.ProductoMarcas.Count(e => e.CodigoEmpresa == CodigoEmpresa && e.CodigoProductoMarca==id) > 0;
            }
        }
    }
}