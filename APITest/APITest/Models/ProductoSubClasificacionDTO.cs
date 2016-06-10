using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Controllers
{
    public class ProductoSubClasificacionDTO
    {
        public int CodigoProductoSubClasificacion { get; set; }
        public int CodigoEmpresa { get; set; }
        public int CodigoProductoClasificacion { get; set; }
        public string ProductoClasificacionDescripcion { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
    
    }
}