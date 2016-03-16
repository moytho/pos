using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class SucursalDTO
    {
        public int CodigoSucursal { get; set; }
        public int CodigoEmpresa { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Area { get; set; }
        public string Telefono { get; set; }
        public bool Estado { get; set; }
    }
}