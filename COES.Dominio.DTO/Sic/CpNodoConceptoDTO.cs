using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_NODO_CONCEPTO
    /// </summary>
    public class CpNodoConceptoDTO : EntityBase
    {
        public int Cpnconcodi { get; set; } 
        public string Cpncontipo { get; set; } 
        public string Cpnconnombre { get; set; } 
        public string Cpnconunidad { get; set; } 
        public int? Cpnconestado { get; set; } 
    }
}
