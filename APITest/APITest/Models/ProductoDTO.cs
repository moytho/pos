using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class ProductoDTO
    {
        public int CodigoProducto { get; set; }
        public int CodigoEmpresa { get; set; }
        public Nullable<int> CodigoProductoClasificacion { get; set; }
        public Nullable<int> CodigoProductoMarca { get; set; }
        public string CodigoLocal { get; set; }
        public string Nombre { get; set; }
        public Nullable<int> CodigoProductoTipo { get; set; }
        public string Descripcion { get; set; }
        public Nullable<decimal> PrecioCosto { get; set; }
        public Nullable<decimal> PrecioVenta { get; set; }
        public Nullable<bool> Estado { get; set; }
        public string SKU { get; set; }
        public Nullable<int> CodigoProductoAbastecimiento { get; set; }
        public Nullable<int> StockMinimo { get; set; }
        public Nullable<decimal> Alto { get; set; }
        public Nullable<decimal> Ancho { get; set; }
        public Nullable<decimal> Profundidad { get; set; }
        public string ImagenUrl { get; set; }
        public List<ProductoImagen> ProductoImagens { get; set; }
        public Nullable<int> CodigoProductoSubClasificacion { get; set; }
    }
}