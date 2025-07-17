using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_EVENTO
    /// </summary>
    public partial class FtExtEventoDTO : EntityBase
    {
        public int Ftevcodi { get; set; }
        public string Ftevnombre { get; set; }
        public DateTime Ftevfecvigenciaext { get; set; }
        public string Ftevestado { get; set; }
        public string Ftevusucreacion { get; set; }
        public DateTime Ftevfeccreacion { get; set; }
        public string Ftevusumodificacion { get; set; }
        public DateTime? Ftevfecmodificacion { get; set; }
        public string Ftevusumodificacionasig { get; set; }
        public DateTime? Ftevfecmodificacionasig { get; set; }
    }

    public partial class FtExtEventoDTO
    {
        public bool EsVigente { get; set; }
        public string EstadoActual { get; set; }
        public string FtevfecvigenciaextDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
        public string FtevfeccreacionDesc { get; set; }
        public string FtevfecmodificacionDesc { get; set; }
        public string FtevfecmodificacionasigDesc { get; set; }
    }
}
