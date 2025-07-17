using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ENVIO_DATO
    /// </summary>
    public partial class FtExtEnvioDatoDTO : EntityBase
    {
        public int Ftedatcodi { get; set; }
        public int Fteeqcodi { get; set; }
        public int Fitcfgcodi { get; set; }
        public string Ftedatvalor { get; set; }
        public string Ftedatflagvalorconf { get; set; }
        public string Ftedatcomentario { get; set; }
        public string Ftedatflagsustentoconf { get; set; }
        public int Ftedatflagmodificado { get; set; }
        public string Ftedatflageditable { get; set; }
        public string Ftedatflagrevisable { get; set; }
    }

    public partial class FtExtEnvioDatoDTO
    {
        public int Ftitcodi { get; set; }
        public int Ftitactivo { get; set; }
        public List<FtExtEnvioReldatoarchivoDTO> ListaRelDatoArchivo { get; set; }
        public List<FtExtEnvioRelrevarchivoDTO> ListaRelRevArchivo { get; set; }
        public FtExtEnvioReldatorevDTO RelRevisionDato { get; set; }
        public string Tipoelemento { get; set; }
        public int? Concepcodi { get; set; }
        public int? Propcodi { get; set; }
        public string Concepabrev { get; set; }
        public string Propabrev { get; set; }
        public int? Equicodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Famcodi { get; set; }

        public bool EsFilaEditableExtranet { get; set; }
        public bool EsFilaRevisableIntranet { get; set; }

        public int FtedatcodiOld { get; set; }

        public string FolderEnvio { get; set; }
        public string SubfolderEqGr { get; set; }
        public string NombFileZipValorNoConf { get; set; }
        public string NombFileZipValorConf { get; set; }
        public string NombFileZipSustentoNoConf { get; set; }
        public string NombFileZipSustentoConf { get; set; }
        public List<FtExtEnvioArchivoDTO> ListaArchivoValorNoConf { get; set; }
        public List<FtExtEnvioArchivoDTO> ListaArchivoValorConf { get; set; }
        public List<FtExtEnvioArchivoDTO> ListaArchivoSustentoNoConf { get; set; }
        public List<FtExtEnvioArchivoDTO> ListaArchivoSustentoConf { get; set; }
    }
}
