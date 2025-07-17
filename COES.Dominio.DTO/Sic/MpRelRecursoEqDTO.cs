using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MP_REL_RECURSO_EQ
    /// </summary>
    public class MpRelRecursoEqDTO : EntityBase
    {
        public int Mtopcodi { get; set; } 
        public int Mrecurcodi { get; set; } 
        public int Equicodi { get; set; } 
        public decimal? Mreqfactor { get; set; } 
    }
}
