using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_EQ
    /// </summary>
    public partial class FtExtEnvioEqDTO : EntityBase
    {
        public int Fteeqcodi { get; set; }
        public int Ftevercodi { get; set; }
        public int? Equicodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Ftfmtcodi { get; set; }
        public string Fteeqestado { get; set; }
        public int Fteeqflagespecial { get; set; }
        public int? Fteeqcodiorigen { get; set; }
        public string Fteeqflagaprob { get; set; }
    }

    public partial class FtExtEnvioEqDTO
    { 
        public int Idelemento { get; set; }        
        public string Nombreelemento { get; set; }
        public string Areaelemento { get; set; }
        public string Nombempresaelemento { get; set; }
        public int Idempresaelemento { get; set; }
        public int? Idempresacopelemento { get; set; }
        public string Nombempresacopelemento { get; set; }
        public string Tipoelemento { get; set; }
        public string TipoYCodigo { get; set; }

        public int Ftenvcodi { get; set; }
        public int? Famcodi { get; set; }
        public int? Catecodi { get; set; }
        public string Catenomb { get; set; }
        public string Famnomb { get; set; }
        public int? IdEnvioEq { get; set; }
        public int? Ftitcodi { get; set; }
        public int? Ftedatflagmodificado { get; set; }
        public int? Equipadre { get; set; }
        public int Ftetcodi { get; set; }

        public List<FtExtEnvioDatoDTO> ListaDato { get; set; }
        public List<FtExtEnvioRelrevarchivoDTO> ListaRelRevArchivo { get; set; }

        public FtExtEnvioRevisionDTO RevisionEq { get; set; }
        public FtExtEnvioReleeqrevDTO RelRevisionEq { get; set; }

        public DateTime? Ftenvfecvigencia { get; set; }
        public bool EsEqVigente { get; set; }
        public string Estadoelemento {  get; set; }
        public string EstadoelementoDesc { get; set; }
    }
}
