using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_LOGCORREO
    /// </summary>
    public partial class ReLogcorreoDTO : EntityBase
    {
        public int Relcorcodi { get; set; } 
        public int? Repercodi { get; set; } 
        public int? Retcorcodi { get; set; } 
        public string Relcorasunto { get; set; } 
        public string Relcorto { get; set; } 
        public string Relcorcc { get; set; } 
        public string Relcorbcc { get; set; } 
        public string Relcorcuerpo { get; set; } 
        public string Relcorusucreacion { get; set; } 
        public DateTime? Relcorfeccreacion { get; set; }
        public int? Relcorempresa { get; set; }
        public string Relcorarchivosnomb { get; set; }
    }

    public partial class ReLogcorreoDTO
    {        
        public string RelcorfeccreacionDesc { get; set; }        
        public string Tipo { get; set; }
        public string PeriodoDesc { get; set; }
        public string Emprnomb { get; set; }
    }
}
