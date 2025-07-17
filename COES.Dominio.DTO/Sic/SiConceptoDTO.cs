using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_CONCEPTO
    /// </summary>
    public class SiConceptoDTO : EntityBase
    {
        public int Consiscodi { get; set; }
        public string Consisabrev { get; set; }
        public string Consisdesc { get; set; }
        public string Consisactivo { get; set; }
        public int? Consisorden { get; set; }
    }
}
