using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_REVISION
    /// </summary>
    public partial class FtExtEnvioRevisionDTO : EntityBase
    {
        public int Ftrevcodi { get; set; } 
        public string Ftrevhtmlobscoes { get; set; } 
        public string Ftrevhtmlrptaagente { get; set; } 
        public string Ftrevhtmlrptacoes { get; set; } 
        public string Ftrevestado { get; set; } 
    }

    public partial class FtExtEnvioRevisionDTO 
    {
        public int Ftedatcodi { get; set; }
        public int Fevrqcodi { get; set; }
        public int Grupocodi { get; set; }

        public int Fteeqcodi { get; set; }
        public int Ftereqcodi { get; set; }
        public int Ftitcodi { get; set; }

        public List<FtExtEnvioArchivoDTO> ListaArchivosRev { get; set; }
    }
}
