using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_AGRUPACION
    /// </summary>
    public partial class PrAgrupacionDTO : EntityBase
    {
        public int Agrupcodi { get; set; }
        public string Agrupnombre { get; set; }
        public int Agrupfuente { get; set; }
        public string Agrupestado { get; set; }
        public string Agrupusucreacion { get; set; }
        public DateTime? Agrupfeccreacion { get; set; }
        public string Agrupusumodificacion { get; set; }
        public DateTime? Agrupfecmodificacion { get; set; }
    }
    public partial class PrAgrupacionDTO
    {
        public string AgrupfeccreacionDesc { get; set; }
        public string AgrupfecmodificacionDesc { get; set; }
        public string AgrupestadoDesc { get; set; }
    }
}
