using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class ProductoInventarioDTO
    {
        public int CodigoProductoInventario { get; set; }
        public int CodigoProducto { get; set; }
        public int CodigoEmpresa { get; set; }
        public int CodigoSucursal { get; set; }
        public int CodigoBodega { get; set; }
        public Nullable<int> Cantidad { get; set; }
    
    }
}