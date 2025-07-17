using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_RELDATOARCHIVO
    /// </summary>
    public partial class FtExtEnvioReldatoarchivoDTO : EntityBase
    {
        public int Ftedatcodi { get; set; }
        public int Ftearccodi { get; set; }
        public int Fterdacodi { get; set; }
    }

    public partial class FtExtEnvioReldatoarchivoDTO 
    {
        public FtExtEnvioArchivoDTO Archivo { get; set; }
    }
}
