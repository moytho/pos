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
    
    public partial class ProductoPrecio
    {
        public int CodigoProductoPrecio { get; set; }
        public int CodigoProducto { get; set; }
        public int CodigoEmpresa { get; set; }
        public int CodigoClienteTipo { get; set; }
        public decimal PrecioVenta { get; set; }
        public bool Estado { get; set; }
    
        public virtual ClienteTipo ClienteTipo { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
