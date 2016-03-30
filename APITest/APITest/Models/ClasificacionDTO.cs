﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class ProductoClasificacionDTO
    {
        public int CodigoProductoClasificacion { get; set; }
        public int CodigoEmpresa { get; set; }
        public Nullable<int> CodigoClasificacionPadre { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        //public ProductoClasificacion ProductoClasificacion2 { get; set; }
        
    }
}