using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_EVENTO_REQ
    /// </summary>
    public partial class FtExtEventoReqDTO : EntityBase
    {
        public int Fevrqcodi { get; set; }
        public int Ftevcodi { get; set; }
        public string Fevrqliteral { get; set; }
        public string Fevrqdesc { get; set; }
        public string Fevrqflaghidro { get; set; }
        public string Fevrqflagtermo { get; set; }
        public string Fevrqflagsolar { get; set; }
        public string Fevrqflageolico { get; set; }
        public string Fevrqestado { get; set; }
    }

    public partial class FtExtEventoReqDTO
    {
        public string UrlSustento { get; set; }
        public List<FtExtEnvioArchivoDTO> ListaArchivo { get; set; }
        public int Nuevo { get; set; }

        public bool EsObligatorioArchivo { get; set; }
        public bool EsFilaEditableExtranet { get; set; }
        public bool EsFilaRevisableIntranet { get; set; }

        public List<FtExtRelAreareqDTO> ListaAreasXRequisito { get; set; }
        public List<int> ListaAreasXRequisitoHidro { get; set; }
        public List<int> ListaAreasXRequisitoTermo { get; set; }
        public List<int> ListaAreasXRequisitoEolico { get; set; }
        public List<int> ListaAreasXRequisitoSolar { get; set; }

        public string ItemSustentoConfidencial { get; set; }

        public string UsuarioModificacion { get; set; }
        public string FechaModificacion { get; set; }

        public int? CambioValor { get; set; }
    }
}
