using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_LOG
    /// </summary>
    public partial class FtExtEnvioLogDTO : EntityBase
    {
        public int Ftelogcodi { get; set; }
        public int Ftenvcodi { get; set; }
        public string Ftelogusucreacion { get; set; }
        public DateTime Ftelogfeccreacion { get; set; }
        public string Ftelogobs { get; set; }
        public int Estenvcodi { get; set; }
        public string Ftelogcondicion { get; set; }
        public int? Envarcodi { get; set; }
        public DateTime? Ftelogfecampliacion { get; set; }
    }

    public partial class FtExtEnvioLogDTO
    {
        public string Estenvnomb { get; set; }
        public int Faremcodi { get; set; }
        public string Faremnombre { get; set; }
        public DateTime Envarfecmaxrpta { get; set; }
        public string FtelogfecampliacionDesc { get; set; }
        public string FtelogfeccreacionDesc { get; set; }
        
    }
}
