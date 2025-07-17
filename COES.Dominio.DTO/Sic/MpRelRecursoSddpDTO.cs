using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MP_REL_RECURSO_SDDP
    /// </summary>
    public class MpRelRecursoSddpDTO : EntityBase
    {
        public int Mtopcodi { get; set; } 
        public int Mrecurcodi { get; set; } 
        public int Sddpcodi { get; set; } 
        public decimal? Mrsddpfactor { get; set; } 
    }
}
