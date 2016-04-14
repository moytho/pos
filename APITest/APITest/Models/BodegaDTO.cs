using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class BodegaDTO
    {

        public int CodigoBodega { get; set; }
        public int CodigoSucursal { get; set; }
        public int CodigoEmpresa { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }

    public class BodegaInformacion
    {

        public int CodigoBodega { get; set; }
        public int CodigoSucursal { get; set; }
        public string Sucursal { get; set; }
        public int CodigoEmpresa { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}