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
    [Authorize]
    public class ClientesController : ApiController
    {
        private JadeCore1Entities db = new JadeCore1Entities();
        private string connectionString = "";
        private string UserId = "";


        // GET api/Clientes
        public IHttpActionResult GetClientes()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            //Esta clase nos retornara 
            //- empresa a la que pertenece el usuario y 
            //-su respectiva cadena de conexion
            //-tambien si posee permiso para realizar esta accion
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ClientesController");
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
                        List<ClienteInformacion> clientes = (from cliente in db.Clientes
                                                 join clientetipo in db.ClienteTipoes 
                                                 on cliente.CodigoClienteTipo equals clientetipo.CodigoClienteTipo
                                                 into joinTable
                                                 from b in joinTable.Where(condicion=> cliente.CodigoEmpresa==conexion.CodigoEmpresa && cliente.Estado==true).DefaultIfEmpty()
                                                 select new ClienteInformacion { 
                                                  Area=cliente.Area,
                                                  Telefono=cliente.Telefono,
                                                  Direccion= cliente.Direccion,
                                                  CorreoElectronico=cliente.CorreoElectronico,
                                                  CodigoCliente=cliente.CodigoCliente,
                                                  Identificador=cliente.Identificador,
                                                  ClienteTipoDescripcion = cliente.ClienteTipo.Descripcion == null ? "Sin asignar" : cliente.ClienteTipo.Descripcion,
                                                  NombreComercial=cliente.NombreComercial,
                                                  Responsable=cliente.Responsable
                                                 }).ToList();
                        //Convirtiendo la Clase sucursal a sucursalDTO
                        //Esto con el afan de ocultar ciertos campos de nuestro objeto, si fuera el caso
                        //Mapper.CreateMap<Cliente, ClienteDTO>();
                        //List<ClienteDTO> clienteDTO = Mapper.Map<List<Cliente>, List<ClienteDTO>>(clientes);
                        return Ok(clientes);
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

        [ResponseType(typeof(Cliente))]
        [Route("api/clientespornombreeidentificacion/")]
        [AllowAnonymous]
        public IHttpActionResult GetClienteByNombreEIdentificacion([FromUri] string busqueda)
        {
            //UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            UserId = "e5e7523c-8f71-4bd7-a96c-1a4a2b1fdc93";
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ClientesController");

            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        //una manera diferente de obtener la(s) sucursal(es) y convertir a la clase SucursalDTO
                        var query = (from cliente in db.Clientes
                                     where (cliente.CodigoEmpresa == conexion.CodigoEmpresa && (cliente.NombreComercial.Contains(busqueda) || cliente.Identificador.Contains(busqueda)))
                                     orderby cliente.NombreComercial
                                     select new ClienteDTO()
                                     {
                                         CodigoEmpresa = cliente.CodigoEmpresa,
                                         CodigoCliente = cliente.CodigoCliente,
                                         NombreComercial = cliente.NombreComercial,
                                         Estado = cliente.Estado,
                                         Responsable = cliente.Responsable,
                                         Telefono = cliente.Telefono,
                                         Direccion = cliente.Direccion,
                                         Area = cliente.Area,
                                         CorreoElectronico = cliente.CorreoElectronico,
                                         Identificador = cliente.Identificador
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

        // GET api/Clientes/5
        [ResponseType(typeof(Cliente))]
        public IHttpActionResult GetCliente(int id)
        {
            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ClientesController");

            if (conexion.PoseePermiso == 1)
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                    using (JadeCore1Entities db = new JadeCore1Entities())
                    {
                        //una manera diferente de obtener la(s) sucursal(es) y convertir a la clase SucursalDTO
                        var query = (from cliente in db.Clientes
                                     where (cliente.CodigoCliente == id && cliente.CodigoEmpresa == conexion.CodigoEmpresa)
                                     orderby cliente.NombreComercial
                                     select new ClienteDTO()
                                     {
                                         CodigoEmpresa = cliente.CodigoEmpresa,
                                         CodigoCliente = cliente.CodigoCliente,
                                         NombreComercial = cliente.NombreComercial,
                                         Estado = cliente.Estado,
                                         Responsable=cliente.Responsable,
                                         Telefono=cliente.Telefono,
                                         Direccion=cliente.Direccion,
                                         Area=cliente.Area,
                                         CorreoElectronico=cliente.CorreoElectronico,
                                         Identificador=cliente.Identificador 
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

        // PUT api/Clientes/5
        public IHttpActionResult PutCliente(int id, Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cliente.CodigoCliente)
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
                    if (!ClienteBelongsToYourCompany(id, conexion.NameConnectionString, conexion.CodigoEmpresa))
                    {
                        return NotFound();
                    }
                    cliente.CodigoEmpresa = conexion.CodigoEmpresa;
                    db.Entry(cliente).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ClienteExists(id, conexion.NameConnectionString))
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

        // POST api/Clientes
        [ResponseType(typeof(Cliente))]
        public IHttpActionResult PostCliente(Cliente cliente)
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
                    cliente.CodigoEmpresa = conexion.CodigoEmpresa;
                    cliente.Estado = true;


                    db.Clientes.Add(cliente);
                    db.SaveChanges();
                    Mapper.CreateMap<Cliente, ClienteDTO>();
                    ClienteDTO clienteDTO = Mapper.Map<Cliente, ClienteDTO>(cliente);

                    return CreatedAtRoute("DefaultApi", new { id = cliente.CodigoCliente }, clienteDTO);
                }
            }
            else return Unauthorized();
        }

        // DELETE api/Clientes/5
        [ResponseType(typeof(Cliente))]
        public IHttpActionResult DeleteCliente(int id)
        {

            UserId = HttpContext.Current.User.Identity.GetUserId().ToString();
            ClaseConexion conexion = new ClaseConexion(UserId, this.GetType().FullName.ToString(), "ClientesController");

            if (conexion.PoseePermiso == 1)
            {
                connectionString = ConfigurationManager.ConnectionStrings[conexion.NameConnectionString].ConnectionString;
                using (JadeCore1Entities db = new JadeCore1Entities())
                {

                    Cliente cliente = db.Clientes.Find(id, conexion.CodigoEmpresa);
                    if (cliente == null)
                    {
                        return NotFound();
                    }
                    cliente.Estado = false;
                    //db.Clientes.Remove(cliente);
                    db.SaveChanges();
                    Mapper.CreateMap<Cliente, ClienteDTO>();
                    ClienteDTO clienteDTO = Mapper.Map<Cliente, ClienteDTO>(cliente);

                    return Ok(clienteDTO);
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

        private bool ClienteExists(int id, string nameConnectionString)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.Clientes.Count(e => e.CodigoCliente == id) > 0;
            }
        }

        private bool ClienteBelongsToYourCompany(int id, string nameConnectionString, int CodigoEmpresa)
        {
            connectionString = ConfigurationManager.ConnectionStrings[nameConnectionString].ConnectionString;
            using (JadeCore1Entities db = new JadeCore1Entities())
            {
                return db.Clientes.Count(e => e.CodigoEmpresa == CodigoEmpresa && e.CodigoCliente == id) > 0;
            }
        }
    }
}