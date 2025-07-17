using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_VALOR_TRANS_EMPRESA
    /// </summary>
    public class ValorTransferenciaEmpresaDTO 
    {
        public System.Int32 ValTranEmpCodi { get; set; }
        public System.Int32 EmpCodi { get; set; }
        public System.Int32 ValTranEmpVersion { get; set; }
        public System.Decimal ValTranEmpTotal { get; set; }
        public System.Int32 PeriCodi { get; set; }
        public System.DateTime ValTranEmpFecIns { get; set; }
        public System.String ValtranUserName { get; set; }

    }
}
