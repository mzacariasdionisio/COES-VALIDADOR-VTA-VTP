using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_AREA
    /// </summary>
    public partial class FtExtEnvioAreaDTO : EntityBase
    {
        public int Ftevercodi { get; set; } 
        public int Faremcodi { get; set; } 
        public int Envarcodi { get; set; } 
        public DateTime? Envarfecmaxrpta { get; set; } 
        public string Envarestado { get; set; } 
    }

    public partial class FtExtEnvioAreaDTO 
    {
        public int Ftenvcodi { get; set; }
        public int Estenvcodi { get; set; }
    }
    
}
