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

    public class ProductoInventarioBusquedaFiltros
    {
        public int CodigoProductoMarca { get; set; }
        public int CodigoProductoClasificacion { get; set; }
        public int CodigoProductoSubClasificacion { get; set; }
        public int CodigoProductoAbastecimiento { get; set; }
        public string NombreProducto { get; set; }
        public string SKU { get; set; }
        public string CodigoLocal { get; set; }
        public int CodigoBodega { get; set; }
        public int Existencia { get; set; }
        public int ExistenciaMinima { get; set; }

    }

}