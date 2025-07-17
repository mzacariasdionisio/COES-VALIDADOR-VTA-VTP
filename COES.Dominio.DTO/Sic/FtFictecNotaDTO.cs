using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_FICTECNOTA
    /// </summary>
    public class FtFictecNotaDTO : EntityBase
    {
        public int Ftnotacodi { get; set; }
        public int? Ftnotanum { get; set; }
        public string Ftnotdesc { get; set; }
        public string Ftnotausucreacion { get; set; }
        public DateTime? Ftnotafeccreacion { get; set; }
        public string Ftnotausumodificacion { get; set; }
        public DateTime? Ftnotafecmodificacion { get; set; }
        public int Ftnotestado { get; set; }
        public int Fteqcodi { get; set; }

        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
        public string FtnotafeccreacionDesc { get; set; }
        public string FtnotafecmodificacionDesc { get; set; }
    }
}
