using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_CONJUNTOENLACE
    /// </summary>
    public class CmConjuntoenlaceDTO : EntityBase
    {
        public int Cnjenlcodi { get; set; } 
        public int Configcodi { get; set; } 
        public int Grulincodi { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
    }
}
