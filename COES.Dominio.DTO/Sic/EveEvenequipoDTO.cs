using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_EVENEQUIPO
    /// </summary>
    public class EveEvenequipoDTO : EntityBase
    {
        public int Evencodi { get; set; } 
        public int Equicodi { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
    }

}
