using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using APITest.Models;
using APITest.Conexion;
using System.Web;

namespace APITest.Controllers
{
    [Authorize]
    public class EmpresasController : ApiController
    {
        private ServiceDBEntities db = new ServiceDBEntities();
        private string UserId = "";

        // GET api/Empresas
        public IHttpActionResult GetEmpresas()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "EmpresasController");
            
            if (conexion.PoseePermiso==1){              
                List<Empresa> empresaList = new List<Empresa>();
                var query = (from empresa in db.Empresas
                             where (empresa.Estado == true)
                             orderby empresa.Nombre
                             select new EmpresaDTO()
                             {
                                 CodigoEmpresa = empresa.CodigoEmpresa,
                                 Nombre = empresa.Nombre,
                                 Direccion = empresa.Direccion,
                                 Telefono = empresa.Telefono,
                                 Email = empresa.Email,
                                 Estado = empresa.Estado
                             }).ToList();

                return Ok(query);
            }
            else return Unauthorized();
        }

        // GET api/Empresas/5
        [ResponseType(typeof(EmpresaDTO))]
        public IHttpActionResult GetEmpresa(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString(); 
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "EmpresasController");
            
            if (conexion.PoseePermiso==1){ 
            var query = (from empresa in db.Empresas where(empresa.CodigoEmpresa== id)
                         orderby empresa.Nombre
                         select new EmpresaDTO()
                         {
                             CodigoEmpresa = empresa.CodigoEmpresa,
                             Nombre = empresa.Nombre,
                             Direccion = empresa.Direccion,
                             Telefono = empresa.Telefono,
                             Email = empresa.Email,
                             Estado = empresa.Estado
                         });

            
            if (query == null)
            {
                return NotFound();
            }

            return Ok(query);
            }
            else return Unauthorized();
        }

        // PUT api/Empresas/5
        public IHttpActionResult PutEmpresa(int id, Empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != empresa.CodigoEmpresa)
            {
                return BadRequest();
            }
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString(); 
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "EmpresasController");

            if (conexion.PoseePermiso == 1)
            {
                db.Entry(empresa).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(id))
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
            else return Unauthorized();
        }

        // POST api/Empresas
        [ResponseType(typeof(Empresa))]
        public IHttpActionResult PostEmpresa(Empresa empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString(); 
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "EmpresasController");

            if (conexion.PoseePermiso == 1){
                empresa.Estado = true;
                db.Empresas.Add(empresa);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = empresa.CodigoEmpresa }, empresa);
            } else return Unauthorized();
        }

        // DELETE api/Empresas/5
        [ResponseType(typeof(Empresa))]
        public IHttpActionResult DeleteEmpresa(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString(); 
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "EmpresasController");

            if (conexion.PoseePermiso == 1)
            {
                Empresa empresa = db.Empresas.Find(id);
                if (empresa == null)
                {
                    return NotFound();
                }

                //db.Empresas.Remove(empresa);
                empresa.Estado = false;
                db.SaveChanges();

                return Ok(empresa);
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

        private bool EmpresaExists(int id)
        {
            return db.Empresas.Count(e => e.CodigoEmpresa == id) > 0;
        }
    }
}