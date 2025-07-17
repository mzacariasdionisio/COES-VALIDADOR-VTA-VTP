using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class SaldoEmpresaDTO
    {
        public System.Int32 SalempCodi { get; set; }
        public System.Int32 EmpCodi { get; set; }
        public System.Int32 SalEmpVersion { get; set; }
        public System.Decimal SalEmpSaldo { get; set; }
        public System.Int32 PeriCodi { get; set; }
        public System.DateTime SalEmpFecIns { get; set; }
        public System.String SalEmpUserName { get; set; }
  
    }
}

