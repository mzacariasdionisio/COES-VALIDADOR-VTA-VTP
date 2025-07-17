using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MMM_JUSTIFICACION
    /// </summary>
    public class MmmJustificacionDTO : EntityBase
    {
        public int Mjustcodi { get; set; }
        public int Immecodi { get; set; }
        public int? Barrcodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Emprcodi { get; set; }
        public DateTime Mjustfecha { get; set; }
        public string Mjustdescripcion { get; set; }
        public string Mjustusucreacion { get; set; }
        public DateTime? Mjustfeccreacion { get; set; }
        public string Mjustusumodificacion { get; set; }
        public DateTime? Mjustfecmodificacion { get; set; }

        public string MjustfechaDesc { get; set; }
    }
}
