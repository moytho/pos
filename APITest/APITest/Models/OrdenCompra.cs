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
    
    public partial class OrdenCompra
    {
        public OrdenCompra()
        {
            this.OrdenCompraDetalles = new HashSet<OrdenCompraDetalle>();
        }
    
        public int CodigoOrdenCompra { get; set; }
        public int CodigoSucursal { get; set; }
        public int CodigoEmpresa { get; set; }
        public Nullable<int> NumeroLocal { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string UserId { get; set; }
        public Nullable<int> CodigoProveedor { get; set; }
        public Nullable<decimal> TotalEsperado { get; set; }
    
        public virtual ICollection<OrdenCompraDetalle> OrdenCompraDetalles { get; set; }
    }
}