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
    public class BodegasController : ApiController
    {
        private JadeCore1Entities db = new JadeCore1Entities();
        private string connectionString = "";
        private string UserId = "";

        // GET api/Bodegas
        public IHttpActionResult GetBodegas()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            //Esta clase nos retornara 
            //- empresa a la que pertenece el usuario y 
            //-su respectiva cadena de conexion
            //-tambien si posee permiso para realizar esta accion
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "BodegasController");
            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        //where (cliente.CodigoEmpresa == conexion.CodigoEmpresa && cliente.Estado == true)
                        //orderby cliente.NombreComercial

                        db.SetConnectionString(connectionString);
                        //Consulta para mostrar todas las sucursales pertenecientes a esta empresa
                        //Left join por si el cliente no tiene asignado un tipo de cliente especifico, mandamos el 
                        //objeto sin formato
                        List<BodegaInformacion> bodegas = (from bodega in db.Bodegas
                                                             join sucursal in db.Sucursals
                                                             on bodega.CodigoSucursal equals sucursal.CodigoSucursal
                                                             into joinTable //Agregar parametro sucursal a la que pertenece usuario
                                                from b in joinTable.Where(condicion => bodega.CodigoEmpresa == conexion.CodigoEmpresa && bodega.CodigoSucursal == conexion.CodigoSucursal  && bodega.Estado == true)
                                                             select new BodegaInformacion
                                                             {
                                                                 Nombre = bodega.Nombre,
                                                                 CodigoBodega = bodega.CodigoBodega,
                                                                 CodigoSucursal = bodega.CodigoSucursal,
                                                                 Sucursal = b.Nombre,
                                                                  CodigoEmpresa = bodega.CodigoEmpresa,
                                                                  Estado = bodega.Estado
                                                             }).ToList();
                        //Convirtiendo la Clase sucursal a sucursalDTO
                        //Esto con el afan de ocultar ciertos campos de nuestro objeto, si fuera el caso
                        //Mapper.CreateMap<Cliente, ClienteDTO>();
                        //List<ClienteDTO> clienteDTO = Mapper.Map<List<Cliente>, List<ClienteDTO>>(clientes);
                        return Ok(bodegas);
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

        // GET api/Bodegas/5
        [ResponseType(typeof(Bodega))]
        public IHttpActionResult GetBodega(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "BodegasController");

            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        //una manera diferente de obtener la(s) sucursal(es) y convertir a la clase SucursalDTO
                        var query = (from bodega in db.Bodegas //agregar parametro de sucursal a la que pertenece el usuario
                                     where (bodega.CodigoBodega == id && bodega.CodigoEmpresa == conexion.CodigoEmpresa && bodega.CodigoSucursal==conexion.CodigoSucursal)
                                     orderby bodega.Nombre
                                     select new BodegaDTO()
                                     {
                                         Nombre = bodega.Nombre,
                                         CodigoBodega = bodega.CodigoBodega,
                                         CodigoSucursal = bodega.CodigoSucursal,
                                         CodigoEmpresa = bodega.CodigoEmpresa,
                                         Estado = bodega.Estado
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

        // PUT api/Bodegas/5
        public IHttpActionResult PutBodega(int id, Bodega bodega)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bodega.CodigoBodega)
            {
                return BadRequest();
            }
            var user = base.ControllerContext.RequestContext.Principal.Identity;
            ClaseConexion conexion = new ClaseConexion(user.GetUserId().ToString(), this.GetType().FullName.ToString(), "BodegasController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
                    //antes de actualizar verificamos que el usuario pertenezca a la misma empresa de la sucursal
                    if (!BodegaBelongsToYou(id, conexion.NameConnectionString, conexion.CodigoEmpresa,conexion.CodigoSucursal))
                    {
                        return NotFound();
                    }
                    bodega.CodigoEmpresa = conexion.CodigoEmpresa;
                    bodega.CodigoSucursal = conexion.CodigoSucursal;
                    db.Entry(bodega).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BodegaExists(id, conexion.NameConnectionString))
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

        // POST api/Bodegas
        [ResponseType(typeof(Bodega))]
        public IHttpActionResult PostBodega(Bodega bodega)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "BodegasController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
                    bodega.CodigoEmpresa = conexion.CodigoEmpresa;
                    bodega.CodigoSucursal = conexion.CodigoSucursal;
                    bodega.Estado = true;


                    db.Bodegas.Add(bodega);
                    db.SaveChanges();
                    Mapper.CreateMap<Bodega, BodegaDTO>();
                    BodegaDTO bodegaDTO = Mapper.Map<Bodega, BodegaDTO>(bodega);

                    return CreatedAtRoute("DefaultApi", new { id = bodega.CodigoBodega}, bodegaDTO);
                }
            }
            else return Unauthorized();
        }

        // DELETE api/Bodegas/5
        [ResponseType(typeof(Bodega))]
        public IHttpActionResult DeleteBodega(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "BodegasController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {

                    Bodega bodega = db.Bodegas.Find(id,conexion.CodigoSucursal,conexion.CodigoEmpresa);
                    if (bodega == null)
                    {
                        return NotFound();
                    }

                    //if (BodegaBelongsToYou(id, conexion.NameConnectionString, conexion.CodigoEmpresa, conexion.CodigoSucursal))
                    //{

                        bodega.Estado = false;
                        //db.Clientes.Remove(cliente);
                        db.SaveChanges();
                        Mapper.CreateMap<Bodega, BodegaDTO>();
                        BodegaDTO bodegaDTO = Mapper.Map<Bodega, BodegaDTO>(bodega);

                        return Ok(bodegaDTO);
                    /*}
                    else {
                        return NotFound();
                    }*/
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

        private bool BodegaExists(int id, string nameConnectionString)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.Bodegas.Count(e => e.CodigoBodega == id) > 0;
            }
        }

        private bool BodegaBelongsToYou(int id, string nameConnectionString, int CodigoEmpresa,int CodigoSucursal)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.Bodegas.Count(e => e.CodigoEmpresa == CodigoEmpresa && e.CodigoBodega == id && e.CodigoSucursal == CodigoSucursal) > 0;
            }
        }
    }
}