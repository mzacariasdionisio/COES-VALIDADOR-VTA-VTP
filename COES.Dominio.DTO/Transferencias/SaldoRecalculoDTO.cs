using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_SALDO_EMPRESA
    /// </summary>
    public class SaldoRecalculoDTO
    {
        public System.Int32 SalRecCodi { get; set; }
        public System.Int32 EmpCodi { get; set; }
        public System.Int32 PeriCodi { get; set; }
        public System.Int32 RecaCodi { get; set; }
        public System.Decimal SalRecSaldo { get; set; }
        public System.Int32 PeriCodiDestino { get; set; }
        public System.String SalRecUserName { get; set; }
        public System.DateTime SalRecFecIns { get; set; }
    }
}
