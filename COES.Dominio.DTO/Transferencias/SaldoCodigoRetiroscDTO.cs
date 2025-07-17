using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_SALDO_CORESC
    /// </summary>
    public class SaldoCodigoRetiroscDTO
    {
        public System.Int32 SalrscCodi { get; set; }
        public System.Int32 EmprCodi { get; set; }
        public System.Int32 SalrscVersion { get; set; }
        public System.Decimal SalrscsSaldo { get; set; }
        public System.Int32 PeriCodi { get; set; }
        public System.DateTime SalrscFecIns { get; set; }
        public System.String SalrscUserName { get; set; }

    }
}
