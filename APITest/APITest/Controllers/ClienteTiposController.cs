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
    public class ClienteTiposController : ApiController
    {
        private JadeCore1Entities db = new JadeCore1Entities();
        private string connectionString = "";
        private string UserId = "";
        // GET api/ClienteTipos
        [AllowAnonymous]
        public IHttpActionResult GetClienteTipoes()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            //Esta clase nos retornara 
            //- empresa a la que pertenece el usuario y 
            //-su respectiva cadena de conexion
            //-tambien si posee permiso para realizar esta accion
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ClienteTiposController");
            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        db.SetConnectionString(connectionString);
                        //Consulta para mostrar todas las sucursales pertenecientes a esta empresa
                        List<ClienteTipo> productoMarcas = (from clienteTipo in db.ClienteTipoes
                                                              where (clienteTipo.CodigoEmpresa == conexion.CodigoEmpresa && clienteTipo.Estado == true)
                                                              orderby clienteTipo.Descripcion
                                                              select clienteTipo).ToList();
                        //Convirtiendo la Clase sucursal a sucursalDTO
                        //Esto con el afan de ocultar ciertos campos de nuestro objeto, si fuera el caso
                        Mapper.CreateMap<ClienteTipo, ClienteTipoDTO>();
                        List<ClienteTipoDTO> productoMarcaDTO = Mapper.Map<List<ClienteTipo>, List<ClienteTipoDTO>>(productoMarcas);
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
        [AllowAnonymous]
        // GET api/ClienteTipos/5
        [ResponseType(typeof(ClienteTipo))]
        public IHttpActionResult GetClienteTipo(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ClienteTiposController");

            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        //una manera diferente de obtener la(s) sucursal(es) y convertir a la clase SucursalDTO
                        var query = (from clienteTipo in db.ClienteTipoes
                                     where (clienteTipo.CodigoClienteTipo == id && clienteTipo.CodigoEmpresa == conexion.CodigoEmpresa)
                                     orderby clienteTipo.Descripcion
                                     select new ClienteTipoDTO()
                                     {
                                         CodigoEmpresa = clienteTipo.CodigoEmpresa,
                                         CodigoClienteTipo = clienteTipo.CodigoClienteTipo,
                                         Descripcion = clienteTipo.Descripcion,
                                         Estado = clienteTipo.Estado
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

        // PUT api/ClienteTipos/5
        public IHttpActionResult PutClienteTipo(int id, ClienteTipo clientetipo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clientetipo.CodigoClienteTipo)
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
                    if (!ClienteTipoBelongsToYourCompany(id, conexion.NameConnectionString, conexion.CodigoEmpresa))
                    {
                        return NotFound();
                    }
                    clientetipo.CodigoEmpresa = conexion.CodigoEmpresa;
                    db.Entry(clientetipo).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ClienteTipoExists(id, conexion.NameConnectionString))
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

        // POST api/ClienteTipos
        [ResponseType(typeof(ClienteTipo))]
        public IHttpActionResult PostClienteTipo(ClienteTipo clientetipo)
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
                    clientetipo.CodigoEmpresa = conexion.CodigoEmpresa;
                    clientetipo.Estado = true;


                    db.ClienteTipoes.Add(clientetipo);
                    db.SaveChanges();
                    Mapper.CreateMap<ClienteTipo, ClienteTipoDTO>();
                    ClienteTipoDTO productoMarcaDTO = Mapper.Map<ClienteTipo, ClienteTipoDTO>(clientetipo);

                    return CreatedAtRoute("DefaultApi", new { id = clientetipo.CodigoClienteTipo }, productoMarcaDTO);
                }
            }
            else return Unauthorized();
        }

        // DELETE api/ClienteTipos/5
        [ResponseType(typeof(ClienteTipo))]
        public IHttpActionResult DeleteClienteTipo(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ClienteTiposController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {

                    ClienteTipo clientetipo = db.ClienteTipoes.Find(id, conexion.CodigoEmpresa);
                    if (clientetipo == null)
                    {
                        return NotFound();
                    }
                    clientetipo.Estado = false;
                    //db.ClienteTipoes.Remove(productomarca);
                    db.SaveChanges();
                    Mapper.CreateMap<ClienteTipo, ClienteTipoDTO>();
                    ClienteTipoDTO productoMarcaDTO = Mapper.Map<ClienteTipo, ClienteTipoDTO>(clientetipo);

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

        private bool ClienteTipoExists(int id, string nameConnectionString)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.ClienteTipoes.Count(e => e.CodigoClienteTipo == id) > 0;
            }
        }

        private bool ClienteTipoBelongsToYourCompany(int id, string nameConnectionString, int CodigoEmpresa)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.ClienteTipoes.Count(e => e.CodigoEmpresa == CodigoEmpresa && e.CodigoClienteTipo == id) > 0;
            }
        }
    }
}