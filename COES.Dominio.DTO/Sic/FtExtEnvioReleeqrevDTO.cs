using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_RELEEQREV
    /// </summary>
    public class FtExtEnvioReleeqrevDTO : EntityBase
    {
        public int Freqrvcodi { get; set; } 
        public int Fteeqcodi { get; set; } 
        public int Ftrevcodi { get; set; } 
    }
}
