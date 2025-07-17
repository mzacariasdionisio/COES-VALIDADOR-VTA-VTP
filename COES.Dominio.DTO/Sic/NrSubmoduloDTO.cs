using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla NR_SUBMODULO
    /// </summary>
    public class NrSubmoduloDTO : EntityBase
    {
        public int Nrsmodcodi { get; set; } 
        public string Nrsmodnombre { get; set; } 
    }
}
