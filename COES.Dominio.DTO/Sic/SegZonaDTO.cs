using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SEG_ZONA
    /// </summary>
    public class SegZonaDTO : EntityBase
    {
        public int Zoncodi { get; set; } 
        public string Zonnombre { get; set; } 
    }
}
