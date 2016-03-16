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
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ScursalessController");   
            if (conexion.PoseePermiso==1) {
            try
            {
                connectionString=ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                 using(JadeCore1Entities  db = new JadeCore1Entities())
                   {
                     db.SetConnectionString(connectionString);
                     /*if (user.Name=="moytho@hotmail.com"){
                         var query = (from sucursal in db.Sucursals
                                      where (sucursal.Estado == true )
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
                         return Ok(query);
                     } else {*/
                        var query = (from sucursal in db.Sucursals where(sucursal.CodigoEmpresa== conexion.CodigoEmpresa && sucursal.Estado == true )
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
                         return Ok(query);
                     //}
                     
                   }
            }
            catch (Exception exception) {
                return InternalServerError();
            }
            }
            else return Unauthorized();
            
        }

        // GET api/Sucursal/5
        //[ResponseType(typeof(SucursalDTO))]
        public IHttpActionResult GetSucursal(int id)
        {
            //return Ok();
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
          ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "SucursalesController");
            
            if (conexion.PoseePermiso==1){
                try
                {
           //sucursal.CodigoEmpresa == conexion.CodigoEmpresa && 
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {

                        var query = (from sucursal in db.Sucursals where(sucursal.CodigoSucursal == id)
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

                        //SucursalDTO sucursalClase = new SucursalDTO();
                        //sucursalClase=query.;
                        //return Ok(db.Sucursals.Find(id));
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
                    if (!SucursalBelongsToYourCompany(id, conexion.NameConnectionString, conexion.CodigoEmpresa))
                    {
                        return NotFound();
                    }

                    sucursal.CodigoEmpresa = conexion.CodigoEmpresa;
                    db.Entry(sucursal).State = EntityState.Modified;

                    try
                    {

                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SucursalExists(id,conexion.NameConnectionString))
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
                    db.Sucursals.Add(sucursal);
                    db.SaveChanges();

                    return CreatedAtRoute("DefaultApi", new { id = sucursal.CodigoSucursal }, sucursal);
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
                    
                    if (!SucursalBelongsToYourCompany(id, conexion.NameConnectionString, conexion.CodigoEmpresa))
                    {
                        return NotFound();
                    }
                    else
                    {
                        Sucursal sucursal = db.Sucursals.Find(id);
                    
                        if (sucursal == null)
                        {
                        return NotFound();
                        }

                        sucursal.Estado = false;
                        //db.Sucursals.Remove(sucursal);
                        db.SaveChanges();

                        return Ok(sucursal);
                    }
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