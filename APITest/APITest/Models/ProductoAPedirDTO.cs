using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class ProductoAPedirDTO
    {
        public int CodigoProductoAPedir { get; set; }
        public int CodigoSucursal { get; set; }
        public int CodigoEmpresa { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public int CodigoProducto { get; set; }
        public string Nombre { get; set; }
        public string UserId { get; set; }
        public bool Estado { get; set; }
    
    }
}