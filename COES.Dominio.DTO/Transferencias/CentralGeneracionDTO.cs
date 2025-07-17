using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la vista VW_EQ_CENTRAL_GENERACION
    /// </summary>
    public class CentralGeneracionDTO
    {
        public System.Int32 CentGeneCodi { get; set; }
        public System.String CentGeneNombre { get; set; }
        public System.String CentGeneEstado { get; set; }
        public System.DateTime CentGeneFecIns { get; set; }
        public System.DateTime CentGeneFecAct { get; set; }
        public System.Int32 FamCodi { get; set; }

    }
}
