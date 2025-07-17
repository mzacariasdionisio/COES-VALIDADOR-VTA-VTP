using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EN_ESTENSAYO
    /// </summary>
    public class EnEstensayoDTO : EntityBase
    {
        public int? Ensayocodi { get; set; } 
        public int? Estadocodi { get; set; } 
        public DateTime? Estensayofecha { get; set; } 
        public string Estensayouser { get; set; } 
    }
}
