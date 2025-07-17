using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase de mapeo de la vista WV_SI_EMPRESA
    /// </summary>
    public class DemandaMercadoLibreDTO
    {
        public System.Int32 EmprCodi { get; set; }
        public System.String EmprRazSocial { get; set; }
        public System.Decimal DemandaMes1 { get; set; }
        public System.Decimal DemandaMes2 { get; set; }
        public System.Decimal DemandaMes3 { get; set; }
        public System.Decimal DemandaMes4 { get; set; }
        public System.Decimal DemandaMes5 { get; set; }
        public System.Decimal DemandaMes6 { get; set; }
        public System.Decimal DemandaMes7 { get; set; }
        public System.Decimal DemandaMes8 { get; set; }
        public System.Decimal DemandaMes9 { get; set; }
        public System.Decimal DemandaMes10 { get; set; }
        public System.Decimal DemandaMes11 { get; set; }
        public System.Decimal DemandaMes12 { get; set; }
        public System.Decimal DemandaMaxima { get; set; }
        public System.Decimal PotenciaMaximaRetirar { get; set; }
        public System.String Periodo { get; set; }
    }
}
