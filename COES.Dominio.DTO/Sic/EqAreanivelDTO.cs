using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EQ_AREANIVEL
    /// </summary>
    public class EqAreaNivelDTO : EntityBase
    {
        public int ANivelCodi { get; set; } 
        public string ANivelNomb { get; set; } 
    }
}
