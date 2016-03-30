using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class ClienteYMas
    {
        public Cliente cliente { get; set; }
        public ClienteTipo clienteTipo { get; set; }
    }
    public class ClienteYMasDTO
    {
        public ClienteDTO cliente { get; set; }
        public ClienteTipoDTO clienteTipo { get; set; }
    }
    public class ClienteInformacion
    {
        public int CodigoCliente { get; set; }
        public int CodigoEmpresa { get; set; }
        public string Identificador { get; set; }
        public string NombreComercial { get; set; }
        public string Responsable { get; set; }
        public string Area { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string CorreoElectronico { get; set; }
        public string ClienteTipoDescripcion { get; set; }
        public bool Estado { get; set; }
    }
}