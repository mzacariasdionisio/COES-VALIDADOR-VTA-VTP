using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_EVENCLASE
    /// </summary>
    public class EveEvenclaseDTO : EntityBase
    {
        public int Evenclasecodi { get; set; } 
        public string Evenclasedesc { get; set; } 
        public int? Tipoevencodi { get; set; } 
        public string Evenclasetipo { get; set; } 
        public string Evenclaseabrev { get; set; } 
    }

}
