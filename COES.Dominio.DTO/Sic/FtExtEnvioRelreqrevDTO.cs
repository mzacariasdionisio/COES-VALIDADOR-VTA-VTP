using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_RELREQREV
    /// </summary>
    public partial class FtExtEnvioRelreqrevDTO : EntityBase
    {
        public int Ftrevcodi { get; set; }
        public int Frrrevcodi { get; set; }
        public int Ftereqcodi { get; set; }
    }

    public partial class FtExtEnvioRelreqrevDTO
    {
        
    }
}
