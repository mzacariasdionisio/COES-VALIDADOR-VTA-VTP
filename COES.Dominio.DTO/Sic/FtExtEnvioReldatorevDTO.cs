using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_RELDATOREV
    /// </summary>
    public partial class FtExtEnvioReldatorevDTO : EntityBase
    {
        public int Ftedatcodi { get; set; } 
        public int Ftrevcodi { get; set; } 
        public int Frdrevcodi { get; set; } 
    }

    public partial class FtExtEnvioReldatorevDTO 
    {
        public FtExtEnvioRevisionDTO Revision { get; set; }
    }
}
