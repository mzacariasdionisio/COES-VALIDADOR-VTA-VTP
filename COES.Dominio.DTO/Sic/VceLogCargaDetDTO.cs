using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla VCE_LOG_CARGA_DET
    /// </summary>
    public class VceLogCargaDetDTO : EntityBase
    {
        public int Crlcdnroregistros { get; set; } 
        public string Crlcdusuimport { get; set; } 
        public DateTime Crlcdhoraimport { get; set; } 
        public int Crlcccodi { get; set; }

        //- conpensaciones.JDEL - Inicio 03/01/2017: Cambio para atender el requerimiento.
        //CAMPOS DE LA CABECERA

        public int Crlccorden { get; set; }
        public string Crlccentidad { get; set; }
        public string Crlccnombtabla { get; set; }
        public int PecaCodi { get; set; }

        //ADICIONALES
        public DateTime Fecultactualizacion { get; set; } 

        //- JDEL Fin
        
    }
}
