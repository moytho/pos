using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class ClienteDTO
    {
        public int CodigoCliente { get; set; }
        public int CodigoEmpresa { get; set; }
        public string Identificador { get; set; }
        public Nullable<int> CodigoClienteTipo { get; set; }
        public string NombreComercial { get; set; }
        public string Responsable { get; set; }
        public string Area { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string CorreoElectronico { get; set; }
        public string ClienteTipoDescripcion { get; set; }
        public bool PoseeCredito { get; set; }
        public Nullable<System.DateTime> FechaCorte { get; set; }
        public Nullable<int> DiasCreditos { get; set; }
        public decimal MontoCredito { get; set; }
        public bool Estado { get; set; }
    }
}