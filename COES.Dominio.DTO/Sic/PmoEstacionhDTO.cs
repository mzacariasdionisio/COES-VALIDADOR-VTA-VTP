using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMO_ESTACIONH
    /// </summary>
    public partial class PmoEstacionhDTO : EntityBase
    {
        public int Pmehcodi { get; set; }
        public string Pmehdesc { get; set; }
        public int Sddpcodi { get; set; }
        public string Pmehreferencia { get; set; }
        public int? Pmehorden { get; set; }
        public string Pmehusucreacion { get; set; }
        public DateTime? Pmehfeccreacion { get; set; }
        public string Pmehusumodificacion { get; set; }
        public DateTime? Pmehfecmodificacion { get; set; }
        public string Pmehestado { get; set; }
        public int? Pmehnumversion { get; set; }
        public string Pmehintegrante { get; set; }
    }

    public partial class PmoEstacionhDTO
    {
        public int? Sddpnum { get; set; }
        public string NombreSddp { get; set; }
        public string PmehfeccreacionDesc { get; set; }
        public string PmehfecmodificacionDesc { get; set; }
        public string PmehestadoDesc { get; set; }
    }
}
