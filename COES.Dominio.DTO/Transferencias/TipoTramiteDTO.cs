using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_TIPO_TRAMITE
    /// </summary>
    public class TipoTramiteDTO
    {
       public System.Int32 TipoTramCodi { get; set; }
       public System.String TipoTramNombre { get; set; }
       public System.String TipoTramEstado { get; set; }
       public System.String TipoTramUserName { get; set; }
       public System.DateTime TipoTramFecIns { get; set; }
       public System.DateTime TipoTramFecAct { get; set; }

    }
}
