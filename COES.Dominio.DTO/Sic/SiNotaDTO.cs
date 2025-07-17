using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_NOTA
    /// </summary>
    public partial class SiNotaDTO : EntityBase
    {
        public int Sinotacodi { get; set; }
        public int? Verscodi { get; set; }
        public string Sinotadesc { get; set; }
        public int Sinotaestado { get; set; }
        public DateTime Sinotaperiodo { get; set; }
        public int Mrepcodi { get; set; }
        public int Sinotaorden { get; set; }
        public int Sinotatipo { get; set; }

        public string Sinotausucreacion { get; set; }
        public DateTime Sinotafeccreacion { get; set; }
        public string Sinotausumodificacion { get; set; }
        public DateTime? Sinotafecmodificacion { get; set; }
    }

    public partial class SiNotaDTO
    {
        public string SinotaperiodoDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
    }
}
