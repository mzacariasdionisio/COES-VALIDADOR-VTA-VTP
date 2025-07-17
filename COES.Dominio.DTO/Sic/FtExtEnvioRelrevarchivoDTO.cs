using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_RELREVARCHIVO
    /// </summary>
    public partial class FtExtEnvioRelrevarchivoDTO : EntityBase
    {
        public int Ftearccodi { get; set; } 
        public int Ftrevcodi { get; set; } 
        public int Ftrrvacodi { get; set; } 
    }

    public partial class FtExtEnvioRelrevarchivoDTO
    {
        public FtExtEnvioArchivoDTO Archivo { get; set; }
    }
}
