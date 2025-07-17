using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_RELREQREVAREA
    /// </summary>
    public class FtExtEnvioRelreqrevareaDTO : EntityBase
    {
        public int Revarqcodi { get; set; } 
        public int Revacodi { get; set; } 
        public int Ftereqcodi { get; set; } 
        public int Envarcodi { get; set; } 
        public int Ftevercodi { get; set; } 
    }
}
