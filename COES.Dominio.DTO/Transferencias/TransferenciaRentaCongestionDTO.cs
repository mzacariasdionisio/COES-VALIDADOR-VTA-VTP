using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RCG_RENTA_CONGESTION
    /// </summary>
    public class TransferenciaRentaCongestionDTO
    {
        public System.Int32 Tretcodi { get; set; }
        public System.Decimal Rcrencrenta { get; set; }

        public System.DateTime Rcrencfeccreacion { get; set; }
        public System.String Rcrencusucreacion { get; set; }
        
        public System.String EmprNombre { get; set; }
        public System.Decimal RentaTotal { get; set; }
        public System.Decimal Reparto { get; set; }

        //Para detalle de reporte

        public System.String EmprNombreCliente { get; set; }
        public System.String BarrBarraTransferencia { get; set; }

        public System.String TretCodigo { get; set; }

        public System.Decimal Licitacion { get; set; }
        public System.Decimal Bilateral { get; set; }

        public System.String Observacion { get; set; }
        public System.String Fechaobservacion { get; set; }

    }
}
