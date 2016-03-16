using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class MarcaDTO
    {
        public int CodigoMarca { get; set; }
        public int CodigoEmpresa { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}