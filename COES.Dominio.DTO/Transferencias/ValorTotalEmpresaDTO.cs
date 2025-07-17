using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_VALOR_TOTAL_EMPRESA
    /// </summary>
    public class ValorTotalEmpresaDTO
    {
        public System.Int32 ValTotaEmpCodi { get; set; }
        public System.Int32 EmpCodi { get; set; }
        public System.Int32 ValTotaEmpVersion { get; set; }
        public System.Decimal ValTotaEmpTotal { get; set; }
        public System.Int32 PeriCodi { get; set; }
        public System.DateTime ValTotaEmpFecIns { get; set; }
        public System.Decimal Total { get; set; }
        public System.String ValTotaEmpUserName { get; set; }
    
    }
}
