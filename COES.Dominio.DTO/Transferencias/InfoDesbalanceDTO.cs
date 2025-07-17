using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class InfoDesbalanceDTO
    { 
        public System.Int32 BarrCodi { get; set; }
        public System.String BarrTransferencia { get; set; }
        public System.Int32 Dia { get; set; }
        public System.Decimal EnergiaDesbalance { get; set; }
        public System.Decimal EnergiaEntrega { get; set; }
        public System.Decimal EnergiaRetiro { get; set; }

        public System.Decimal DesbalanceMensual { get; set; }
        public System.Decimal DesbalanceDia { get; set; }
    }
}
