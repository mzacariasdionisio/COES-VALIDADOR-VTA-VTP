using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla VCE_GRUPO_EXCLUIDO
    /// </summary>
    public class VceGrupoExcluidoDTO : EntityBase
    {
        public int Crgexccodi { get; set; }
        public int Pecacodi { get; set; }
        public int Grupocodi { get; set; }
        public string Crgexcusucreacion { get; set; }
        public DateTime Crgexcfeccreacion { get; set; }
        public string Crgexcusumodificacion { get; set; }
        public DateTime? Crgexcfecmodificacion { get; set; }

    }
}
