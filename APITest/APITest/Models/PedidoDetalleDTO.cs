using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APITest.Models
{
    public class PedidoDetalleDTO
    {
        public int CodigoPedidoDetalle { get; set; }
        public int CodigoPedido { get; set; }
        public int CodigoSucursal { get; set; }
        public int CodigoEmpresa { get; set; }
        public int CodigoProducto { get; set; }
        public int Cantidad { get; set; }
    
    }
}