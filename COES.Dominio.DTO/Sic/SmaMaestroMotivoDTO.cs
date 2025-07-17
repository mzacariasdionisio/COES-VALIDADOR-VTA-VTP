using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_MAESTRO_MOTIVO
    /// </summary>
    public partial class SmaMaestroMotivoDTO : EntityBase
    {
        public int Smammcodi { get; set; }
        public string Smammdescripcion { get; set; }
        public string Smammestado { get; set; }
        public string Smammusucreacion { get; set; }
        public DateTime Smammfeccreacion { get; set; }
        public string Smammusumodificacion { get; set; }
        public DateTime? Smammfecmodificacion { get; set; }
    }

    public partial class SmaMaestroMotivoDTO
    {
        public string SmammestadoDesc { get; set; }
        public string SmammfeccreacionDesc { get; set; }
        public string SmammfecmodificacionDesc { get; set; }
    }
}
