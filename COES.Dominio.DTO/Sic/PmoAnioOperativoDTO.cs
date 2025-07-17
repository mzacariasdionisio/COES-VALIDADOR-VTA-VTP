using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMO_ANIO_OPERATIVO
    /// </summary>
    public partial class PmoAnioOperativoDTO : EntityBase
    {
        public int Pmanopcodi { get; set; }
        public int? Pmanopanio { get; set; }
        public DateTime? Pmanopfecini { get; set; }
        public DateTime? Pmanopfecfin { get; set; }
        public int? Pmanopestado { get; set; }
        public int? Pmanopnumversion { get; set; }
        public string Pmanopusucreacion { get; set; }
        public DateTime? Pmanopfeccreacion { get; set; }
        public string Pmanopusumodificacion { get; set; }
        public DateTime? Pmanopfecmodificacion { get; set; }
        public string Pmanopdesc { get; set; }
        public int? Pmanopprocesado { get; set; }
    }

    public partial class PmoAnioOperativoDTO
    {
        public string PmanopNumFeriados { get; set; }
        public int NumVersiones { get; set; }
        public string PmanopfeciniDesc { get; set; }
        public string PmanopfecfinDesc { get; set; }
        public string PmanopfeccreacionDesc { get; set; }
        public string PmanopfecmodificacionDesc { get; set; }
        public string PmanopestadoDesc { get; set; }
        public string procesadoDesc { get; set; }
    }
}
