using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_RELDATOREVAREA
    /// </summary>
    public class FtExtEnvioReldatorevareaDTO : EntityBase
    {
        public int Revadcodi { get; set; } 
        public int Ftedatcodi { get; set; } 
        public int Revacodi { get; set; } 
        public int Envarcodi { get; set; } 
        public int Ftevercodi { get; set; } 
    }
}
