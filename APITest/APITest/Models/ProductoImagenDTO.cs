using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class ProductoImagenDTO
    {
        public int CodigoProductoImagen { get; set; }
        public int CodigoProducto { get; set; }
        public int CodigoEmpresa { get; set; }
        public string ImagenUrl { get; set; }
        public bool Principal { get; set; }
    }
}