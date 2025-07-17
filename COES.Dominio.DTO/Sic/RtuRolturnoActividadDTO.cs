using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RTU_ROLTURNO_ACTIVIDAD
    /// </summary>
    public class RtuRolturnoActividadDTO : EntityBase
    {
        public int Rtudetcodi { get; set; }
        public int Rtuactcodi { get; set; }
        public int Rturaccodi { get; set; }
    }
}
