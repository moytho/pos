﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class JadeCore1Entities : DbContext
    {
        public JadeCore1Entities()
            : base("name=JadeCore1Entities")
        {
        }

        public void SetConnectionString(string connectionString)
        {
            this.Database.Connection.ConnectionString = connectionString;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Sucursal> Sucursals { get; set; }
        public virtual DbSet<Producto> Productoes { get; set; }
        public virtual DbSet<ProductoClasificacion> ProductoClasificacions { get; set; }
        public virtual DbSet<ProductoMarca> ProductoMarcas { get; set; }
        public virtual DbSet<ProductoTipo> ProductoTipoes { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<ClienteTipo> ClienteTipoes { get; set; }
        public virtual DbSet<ProductoAbastecimiento> ProductoAbastecimientoes { get; set; }
        public virtual DbSet<ProductoPrecio> ProductoPrecios { get; set; }
        public virtual DbSet<ProductoImagen> ProductoImagens { get; set; }
        public virtual DbSet<Bodega> Bodegas { get; set; }
        public virtual DbSet<ProductoInventario> ProductoInventarios { get; set; }
    }
}
