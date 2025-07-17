using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_AREACONCEPTO
    /// </summary>
    public class PrAreaConceptoDTO : EntityBase
    {
        public int Arconcodi { get; set; }
        public int Areacode { get; set; }
        public int Concepcodi { get; set; }
        public string Arconusucreacion { get; set; }
        public DateTime? Arconfeccreacion { get; set; }
        public string Arconusumodificacion { get; set; }
        public DateTime? Arconfecmodificacion { get; set; }
        public int Arconactivo { get; set; }
    }
}
