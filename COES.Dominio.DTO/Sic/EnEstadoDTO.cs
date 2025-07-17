using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EN_ESTADOS
    /// </summary>
    public class EnEstadoDTO : EntityBase
    {
        public int? Estadocodi { get; set; } 
        public string Estadonombre { get; set; } 
        public string Estadocolor { get; set; } 
    }
}
