using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_REVAREA
    /// </summary>
    public partial class FtExtEnvioRevareaDTO : EntityBase
    {
        public int Revacodi { get; set; } 
        public string Revaestadoronda1 { get; set; } 
        public string Revahtmlronda1 { get; set; } 
        public string Revaestadoronda2 { get; set; } 
        public string Revahtmlronda2 { get; set; } 
        public int Ftevercodi { get; set; } 
    }

    public partial class FtExtEnvioRevareaDTO
    {
        public int Ftedatcodi { get; set; }
        public int Fevrqcodi { get; set; }
        public int Grupocodi { get; set; }

        public int Fteeqcodi { get; set; }
        public int Ftereqcodi { get; set; }
        public int Ftitcodi { get; set; }
        public int Revadcodi { get; set; }

        public int Faremcodi { get; set; }
        public string Envarestado { get; set; }

        public FtExtEnvioReldatorevareaDTO RelacionDatoRevisionArea { get; set; }
        public FtExtEnvioRelreqrevareaDTO RelacionReqRevisionArea { get; set; }
        public List<FtExtEnvioArchivoDTO> ListaArchivosRev { get; set; }
    }
}
