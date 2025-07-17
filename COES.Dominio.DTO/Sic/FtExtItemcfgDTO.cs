using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ITEMCFG
    /// </summary>
    public partial class FtExtItemcfgDTO : EntityBase
    {
        public int Ftitcodi { get; set; }
        public int Fitcfgcodi { get; set; }
        public string Fitcfgflagcomentario { get; set; }
        public string Fitcfgflagvalorconf { get; set; }
        public string Fitcfgflagbloqedicion { get; set; }
        public string Fitcfgflagsustento { get; set; }
        public string Fitcfgflagsustentoconf { get; set; }
        public string Fitcfgflaginstructivo { get; set; }
        public string Fitcfgflagvalorobligatorio { get; set; }
        public string Fitcfgflagsustentoobligatorio { get; set; }
        public string Fitcfginstructivo { get; set; }
        public string Fitcfgusucreacion { get; set; }
        public DateTime? Fitcfgfeccreacion { get; set; }
        public string Fitcfgusumodificacion { get; set; }
        public DateTime? Fitcfgfecmodificacion { get; set; }
        public int Ftfmtcodi { get; set; }
    }

    public partial class FtExtItemcfgDTO
    {
        public int? Ftpropcodi { get; set; }
        public int? Concepcodi { get; set; }
        public int? Propcodi { get; set; }

        public string NombreFila { get; set; }
        public bool FlagUsado { get; set; }
        public int? CambioValor { get; set; }
    }
}
