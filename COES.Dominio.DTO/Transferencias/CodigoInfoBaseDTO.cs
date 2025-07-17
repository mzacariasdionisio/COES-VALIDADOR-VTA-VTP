using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_CODIGO_INFOBASE
    /// </summary>
    public class CodigoInfoBaseDTO
    {
        public System.Int32 CoInfBCodi { get; set; }
        public System.Int32 EmprCodi { get; set; }
        public System.Int32 BarrCodi { get; set; }
        public System.Int32 CentGeneCodi { get; set; }
        public System.String CoInfBCodigo { get; set; }
        public System.DateTime CoInfBFechaInicio { get; set; }
        public System.DateTime? CoInfBFechaFin { get; set; }
        public System.String CoInfBEstado { get; set; }
        public System.String CoInfBUserName { get; set; }
        public System.DateTime CoInfBFecIns { get; set; }
        public System.DateTime CoInfBFecAct { get; set; }
        public System.String CentGeneNombre { get; set; }
        public System.String BarrNombBarrTran { get; set; }
        public System.String EmprNomb { get; set; }
    }
}
