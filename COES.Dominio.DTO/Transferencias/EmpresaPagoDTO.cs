using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_EMPRESA_PAGO
    /// </summary>
    public class EmpresaPagoDTO
    {
        public System.Int32 EmpPagoCodi { get; set; }
        public System.Int32 ValTotaEmpCodi { get; set; }
        public System.Int32 EmpCodi { get; set; }
        public System.Int32 ValTotaEmpVersion { get; set; }
        public System.Int32 PeriCodi { get; set; }
        public System.Int32 EmpPagoCodEmpPago { get; set; }
        public System.Decimal EmpPagoMonto { get; set; }
        public System.String EmpPagpUserName { get; set; }  
        public System.DateTime EmpPagoFecIns { get; set; }
        //para reporte
        public System.String EmprNomb { get; set; }
        public System.String EmprRuc { get; set; }
        public System.String EmprNombPago { get; set; }

        #region SIOSEIN
        public string Emprcodosinergmin { get; set; }
        public string Emprcodosinergminpago { get; set; }
        public bool EsNuevoRegistro { get; set; }
        #endregion
    }
}
