using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_PERIODOMEDIDOR
    /// </summary>
    public class MePeriodomedidorDTO : EntityBase
    {
        public int? Medicodi { get; set; }
        public int? Earcodi { get; set; }
        public DateTime? Permedifechaini { get; set; }
        public DateTime? Permedifechafin { get; set; }

        public string Medinombre { get; set; }
    }
}
