//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace APITest.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductoInventario
    {
        public int CodigoProductoInventario { get; set; }
        public int CodigoProducto { get; set; }
        public int CodigoEmpresa { get; set; }
        public int CodigoSucursal { get; set; }
        public int CodigoBodega { get; set; }
        public Nullable<int> Cantidad { get; set; }
    
        public virtual Bodega Bodega { get; set; }
        public virtual Producto Producto { get; set; }
        public virtual Sucursal Sucursal { get; set; }
    }
}
