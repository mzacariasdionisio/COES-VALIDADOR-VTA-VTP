using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_TRANS_INFOBASE
    /// </summary>
    public class TransferenciaInformacionBaseDTO
    {
        public System.Int32 TinfbCodi { get; set; }
        public System.Int32 CoInfbCodi { get; set; }
        public System.Int32 BarrCodi { get; set; }
        public System.Int32 PeriCodi { get; set; }
        public System.Int32 EmprCodi { get; set; }
        public System.Int32 EquiCodi { get; set; }
        public System.String TinfbCodigo { get; set; }
        public System.Int32 TinfbVersion { get; set; }
        public System.String TinfbTipoInformacion { get; set; }
        public System.String TinfbEstado { get; set; }
        public System.String TinfbUserName { get; set; }
        public System.DateTime TinfbFecIns { get; set; }
        public System.DateTime TinfbFecAct { get; set; }

        public System.String EmprNombre { get; set; }
        public System.String CentGeneNombre { get; set; }
        public System.String BarrNombre { get; set; }
        public System.Decimal Total { get; set; }

        /* ASSETEC 202001 */
        public System.Int32? TrnEnvCodi { get; set; }
    }
}
