using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class ProductoAbastecimientoDTO
    {
        public int CodigoProductoAbastecimiento { get; set; }
        public int CodigoEmpresa { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> DiasAbastecimiento { get; set; }
        public bool Estado { get; set; }
    
    }
}