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
        public int CodigoMarca { get; set; }
        public int CodigoClasificacion { get; set; }
        public int CodigoSubClasificacion { get; set; }
        public string NombreProducto { get; set; }
        public string CodigoLocal { get; set; }
        public int CodigoBodega { get; set; }
        public Nullable<int> Cantidad { get; set; }

    }

}