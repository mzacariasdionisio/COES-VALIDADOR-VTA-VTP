using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SPO_CONCEPTO
    /// </summary>
    public class SpoConceptoDTO : EntityBase
    {
        public int Sconcodi { get; set; }
        public int? Ptomedicodi { get; set; }
        public int? Ptomedicodi2 { get; set; }
        public int? Numccodi { get; set; }
        public string Sconnomb { get; set; }
        public int? Sconactivo { get; set; }
        public int? Sconorden { get; set; }
        public int? Lectcodi { get; set; }

        public string Ptomedicalculado1 { get; set; }
        public string Ptomedicalculado2 { get; set; }
    }
}
