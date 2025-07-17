using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_TIPO_CONTRATO
    /// </summary>
    public class TipoContratoDTO 
    {
        public System.Int32 TipoContCodi { get; set; }
        public System.String TipoContNombre { get; set; }
        public System.String TipoContEstado { get; set; }
        public System.String TipoContUserName { get; set; }
        public System.DateTime TipoContFecIns { get; set; }
        public System.DateTime TipoContFecAct { get; set; }
        public bool bGrabar { get; set; }

    }
}
