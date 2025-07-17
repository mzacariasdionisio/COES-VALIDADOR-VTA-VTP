using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMO_FERIADO
    /// </summary>
    public partial class PmoFeriadoDTO : EntityBase
    {
        public int Pmfrdocodi { get; set; }
        public int? Pmanopcodi { get; set; }
        public DateTime? Pmfrdofecha { get; set; }
        public string Pmfrdodescripcion { get; set; }
        public int? Pmfrdoestado { get; set; }
        public string Pmfrdousucreacion { get; set; }
        public DateTime? Pmfrdofeccreacion { get; set; }
        public string Pmfrdousumodificacion { get; set; }
        public DateTime? Pmfrdofecmodificacion { get; set; }
    }

    public partial class PmoFeriadoDTO
    {
        public string PmfrdofechaDesc { get; set; }
        public string PmfrdofeccreacionDesc { get; set; }
        public string PmfrdofecmodificacionDesc { get; set; }
        public string PmfrdoestadoDesc { get; set; }
        public int NumSemana { get; set; }
    }
}
