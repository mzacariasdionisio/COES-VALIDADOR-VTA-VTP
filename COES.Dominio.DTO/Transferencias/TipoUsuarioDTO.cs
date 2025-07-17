using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_TIPO_USUARIO
    /// </summary>
    public class TipoUsuarioDTO
    {     
        public System.Int32 TipoUsuaCodi { get; set; }
        public System.String TipoUsuaNombre { get; set; }
        public System.String TipoUsuaEstado { get; set; }
        public System.String TipoUsuaUserName { get; set; }
        public System.DateTime TipoUsuaFecIns { get; set; }
        public System.DateTime TipoUsuaFecAct { get; set; }
        public bool bGrabar { get; set; }

    }
}
