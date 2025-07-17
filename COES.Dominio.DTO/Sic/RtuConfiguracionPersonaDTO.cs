using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RTU_CONFIGURACION_PERSONA
    /// </summary>
    public class RtuConfiguracionPersonaDTO : EntityBase
    {
        public int Rtugrucodi { get; set; }
        public int Rtupercodi { get; set; }
        public int? Rtuperorden { get; set; }
        public int Percodi { get; set; }
    }
}
