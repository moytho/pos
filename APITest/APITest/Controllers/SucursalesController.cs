using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using APITest.Models;
using System.Configuration;
using APITest.Conexion;
using System.Web.Http.Cors;
using System.Web;
using AutoMapper;
using System.Collections.Generic;

namespace APITest.Controllers
{
    [Authorize]
    public class SucursalesController : ApiController
    {
        private JadeCore1Entities db = new JadeCore1Entities();
        private string connectionString="";
        private string UserId = "";

        // GET api/Sucursales
        public IHttpActionResult GetSucursals()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            //Esta clase nos retornara 
            //- empresa a la que pertenece el usuario y 
            //-su respectiva cadena de conexion
            //-tambien si posee permiso para realizar esta accion
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ScursalessController");   
            if (conexion.PoseePermiso==1) {
            try
            {
                connectionString=ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                 using(JadeCore1Entities  db = new JadeCore1Entities())
                   {
                     db.SetConnectionString(connectionString);
                     //Consulta para mostrar todas las sucursales pertenecientes a esta empresa
                     List<Sucursal> sucursales = (from sucursal in db.Sucursals
                                      where (sucursal.CodigoEmpresa == conexion.CodigoEmpresa && sucursal.Estado == true)
                                      orderby sucursal.Nombre
                                      select sucursal).ToList();
                        //Convirtiendo la Clase sucursal a sucursalDTO
                        //Esto con el afan de ocultar ciertos campos de nuestro objeto, si fuera el caso
                         Mapper.CreateMap<Sucursal, SucursalDTO>();
                         List<SucursalDTO> sucursalesDTO=Mapper.Map<List<Sucursal>, List<SucursalDTO>>(sucursales);
                         return Ok(sucursalesDTO);
                   }
            }
            catch (Exception exception) {
                return InternalServerError();
            }
            }
                //si no posee permiso retornamos el estado Unauthorized 501
            else return Unauthorized();
            
        }

        // GET api/Sucursal/5
        //[ResponseType(typeof(SucursalDTO))]
        public IHttpActionResult GetSucursal(int id)
        {
  
          UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
          ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "SucursalesController");
            
            if (conexion.PoseePermiso==1){
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        //una manera diferente de obtener la(s) sucursal(es) y convertir a la clase SucursalDTO
                        var query = (from sucursal in db.Sucursals where(sucursal.CodigoSucursal == id && sucursal.CodigoEmpresa==conexion.CodigoEmpresa)
                                     orderby sucursal.Nombre
                                     select new SucursalDTO()
                                     {
                                         CodigoEmpresa = sucursal.CodigoEmpresa,
                                         CodigoSucursal = sucursal.CodigoSucursal,
                                         Nombre = sucursal.Nombre,
                                         Direccion = sucursal.Direccion,
                                         Telefono = sucursal.Telefono,
                                         Area = sucursal.Area,
                                         Estado = sucursal.Estado
                                     }).ToList();
                        if (query == null)
                        {
                            return NotFound();
                        }

                        return Ok(query);
                    }
                }
                catch (Exception exception) {
                    return InternalServerError();
                }
            }
            else return Unauthorized();
        }

        // PUT api/Sucursal/5
        public IHttpActionResult PutSucursal(int id, Sucursal sucursal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sucursal.CodigoSucursal)
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
                    if (!SucursalBelongsToYourCompany(id,conexion.NameConnectionString, conexion.CodigoEmpresa))
                    {
                        return NotFound();
                    }
                    //le asignamos el codigoEmpresa para asegurarnos de que se actualice con la empresa correspondiente
                    sucursal.CodigoEmpresa = conexion.CodigoEmpresa;
                    db.Entry(sucursal).State = EntityState.Modified;

                    try
                    {
                        //intente actualizar
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        //uno de los motivos por los que no logro actualizar es porque no existe
                        if (!SucursalExists(id,conexion.NameConnectionString))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    //no retornamos contenido
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            else return Unauthorized();
        }

        // POST api/Sucursal
        [ResponseType(typeof(Sucursal))]
        public IHttpActionResult PostSucursal(Sucursal sucursal)
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
                    sucursal.CodigoEmpresa = conexion.CodigoEmpresa;
                    sucursal.Estado = true;
                    //agregamos la sucursal
                    db.Sucursals.Add(sucursal);
                    db.SaveChanges();
                    //retornamos el codigo generado para la sucursal por si lo necesitaramos
                    Mapper.CreateMap<Sucursal, SucursalDTO>();
                    SucursalDTO sucursalDTO = Mapper.Map<Sucursal, SucursalDTO>(sucursal);
                    
                    return CreatedAtRoute("DefaultApi", new { id = sucursal.CodigoSucursal }, sucursalDTO);
                }
            }
            else return Unauthorized();
        }

        // DELETE api/Sucursal/5
        [ResponseType(typeof(Sucursal))]
        public IHttpActionResult DeleteSucursal(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "SucursalessController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {
                    //Validamos que la sucursal a dar de baja pertenece a la empresa del usuario
                    /*if (!SucursalBelongsToYourCompany(id, conexion.NameConnectionString, conexion.CodigoEmpresa))
                    {
                        return NotFound();
                    }
                    else
                    {*/
                        //seleccionamos la sucursal deseada, por tener llave compuesta tenemos que mandar el id y el codigoEmpresa a la que pertenece
                        Sucursal sucursal = db.Sucursals.Find(id,conexion.CodigoEmpresa);
                    
                        if (sucursal == null)
                        {
                        return NotFound();
                        }
                        //En vez de eliminarla de la bd solo le cambiamos su estado
                        sucursal.Estado = false;
                        //db.Sucursals.Remove(sucursal);
                        db.SaveChanges();
                        Mapper.CreateMap<Sucursal, SucursalDTO>();
                        SucursalDTO sucursalDTO = Mapper.Map<Sucursal, SucursalDTO>(sucursal);
                    
                        return Ok(sucursalDTO);
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

        private bool SucursalExists(int id,string nameConnectionString)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.Sucursals.Count(e => e.CodigoSucursal == id) > 0;
            }
        }

        private bool SucursalBelongsToYourCompany(int id, string nameConnectionString,int CodigoEmpresa)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.Sucursals.Count(e => e.CodigoSucursal == id && e.CodigoEmpresa== CodigoEmpresa) > 0;
            }
        }
    }
}

