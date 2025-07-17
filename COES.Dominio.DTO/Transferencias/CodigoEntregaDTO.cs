using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_CODIGO_ENTREGA
    /// </summary>
    public class CodigoEntregaDTO
    {
        public System.Int32 CodiEntrCodi { get; set; }
        public System.Int32 EmprCodi { get; set; }
        public System.Int32 BarrCodi { get; set; }
        public System.Int32 CentGeneCodi { get; set; }
        public System.String CodiEntrCodigo { get; set; }
        public System.DateTime CodiEntrFechaInicio { get; set; }
        public System.DateTime? CodiEntrFechaFin { get; set; }
        public System.String CodiEntrEstado { get; set; }
        public System.String CodiEntrUserName { get; set; }
        public System.DateTime CodiEntrFecIns { get; set; }
        public System.DateTime CodiEntrFecAct { get; set; }
        public System.String CentGeneNombre { get; set; }
        public System.String BarrNombBarrTran { get; set; }
        public System.String EmprNomb { get; set; }

    }
}
