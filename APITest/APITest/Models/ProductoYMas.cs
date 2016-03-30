using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class ProductoYMas
    {
        public Producto producto { get; set; }
        public ProductoMarca productoMarca { get; set; }
        public ProductoClasificacion productoClasificacion { get; set; }
    }
    public class ProductoYMasDTO
    {
        public ProductoDTO producto { get; set; }
        public ProductoMarcaDTO productoMarca { get; set; }
        public ProductoClasificacionDTO productoClasificacion { get; set; }
    }
    public class ProductoInformacion 
    {
        public int CodigoProducto { get; set; }
        public Nullable<int> CodigoProductoMarca { get; set; }
        public string CodigoLocal { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Clasificacion { get; set; }
        public Nullable<decimal> PrecioCosto { get; set; }
        public Nullable<decimal> PrecioVenta { get; set; }
        public Nullable<bool> Estado { get; set; }

        
    }
}