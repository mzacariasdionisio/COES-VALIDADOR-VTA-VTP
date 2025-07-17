using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_RELREVAREAARCHIVO
    /// </summary>
    public class FtExtEnvioRelrevareaarchivoDTO : EntityBase
    {
        public int Revaacodi { get; set; } 
        public int Revacodi { get; set; } 
        public int Ftearccodi { get; set; } 
        public int Ftevercodi { get; set; } 
    }
}
