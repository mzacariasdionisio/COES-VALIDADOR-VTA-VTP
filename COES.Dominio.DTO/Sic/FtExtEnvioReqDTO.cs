using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_REQ
    /// </summary>
    public partial class FtExtEnvioReqDTO : EntityBase
    {
        public int Ftereqcodi { get; set; }
        public int Fevrqcodi { get; set; }
        public int Ftevercodi { get; set; }
        public int Ftereqflagarchivo { get; set; }
        public string Ftereqflageditable { get; set; }
        public string Ftereqflagrevisable { get; set; }
    }

    public partial class FtExtEnvioReqDTO
    {
        public string Fevrqliteral { get; set; }
        
        public List<FtExtEnvioRelreqarchivoDTO> ListaRelreqarchivo { get; set; }
        public List<FtExtEnvioRelrevarchivoDTO> ListaRelRevArchivo { get; set; }
        public FtExtEnvioRevisionDTO RevisionReq { get; set; }
        public FtExtEnvioRelreqrevDTO RelRevisionReq { get; set; }

        public bool EsObligatorioArchivo { get; set; }
        public bool EsFilaEditableExtranet { get; set; }
        public bool EsFilaRevisableIntranet { get; set; }

    }
}
