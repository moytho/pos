using APITest.Conexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APITest.Controllers
{
    [AllowAnonymous]
    public class PruebasController : ApiController
    {
         public IHttpActionResult GetPruebas(int id)
        {
            ClaseConexionPrueba conexion = new ClaseConexionPrueba(id);
            string resultado=conexion.Prueba();
            return Ok(resultado);
        }
    }
}
