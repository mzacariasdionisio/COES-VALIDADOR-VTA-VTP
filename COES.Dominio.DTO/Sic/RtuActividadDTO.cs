using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RTU_ACTIVIDAD
    /// </summary>
    public class RtuActividadDTO : EntityBase
    {
        public int Rtuactcodi { get; set; }
        public string Rtuactdescripcion { get; set; }
        public string Rtuactabreviatura { get; set; }
        public string Rtuactestado { get; set; }
        public string Rtuactusucreacion { get; set; }
        public DateTime? Rtuactfeccreacion { get; set; }
        public string Rtuactusumodificacion { get; set; }
        public DateTime? Rtuactfecmodificacion { get; set; }
        public string Rtuactreporte { get; set; }
        public int? Rturescodi { get; set; }
        public string Rturesdescripcion { get; set; }
        public string Rturesrol { get; set; }
    }
}
