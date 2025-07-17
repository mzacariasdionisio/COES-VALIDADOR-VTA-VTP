using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_CORREO
    /// </summary>
    public partial class SiCorreoDTO : EntityBase
    {
        public int Corrcodi { get; set; }
        public int? Enviocodi { get; set; }
        public int? Plantcodi { get; set; }
        public string Corrto { get; set; }
        public string Corrfrom { get; set; }
        public string Corrcc { get; set; }
        public DateTime? Corrfechaenvio { get; set; }
        public string Corrbcc { get; set; }
        public string Corrasunto { get; set; }
        public string Corrcontenido { get; set; }
        public DateTime? Corrfechaperiodo { get; set; }
        public int? Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Corrusuenvio { get; set; }
    }

    public partial class SiCorreoDTO
    {
        public string CorrasuntoDesc { get; set; }
        public string CorrfechaenvioDesc { get; set; }
        public string Archivos { get; set; }
    }
}
