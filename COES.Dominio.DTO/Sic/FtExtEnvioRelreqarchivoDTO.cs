using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_RELREQARCHIVO
    /// </summary>
    public partial class FtExtEnvioRelreqarchivoDTO : EntityBase
    {
        public int Fterracodi { get; set; }
        public int Ftereqcodi { get; set; }
        public int Ftearccodi { get; set; }
    }

    public partial class FtExtEnvioRelreqarchivoDTO 
    {
        public FtExtEnvioArchivoDTO Archivo { get; set; }
    }
}
