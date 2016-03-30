using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class ProductoMarcaDTO
    {
        public int CodigoProductoMarca { get; set; }
        public int CodigoEmpresa { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}