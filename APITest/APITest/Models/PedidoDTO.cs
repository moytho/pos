using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class PedidoDTO
    {
        public int CodigoPedido { get; set; }
        public int CodigoSucursal { get; set; }
        public int CodigoEmpresa { get; set; }
        public Nullable<int> CodigoPedidoLocal { get; set; }
        public int FechaCreacion { get; set; }
        public string UserId { get; set; }

        public virtual ICollection<PedidoDetalleDTO> PedidoDetalles { get; set; }
    }
}