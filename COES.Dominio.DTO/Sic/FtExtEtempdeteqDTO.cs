using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_ETEMPDETEQ
    /// </summary>
    public partial class FtExtEtempdeteqDTO : EntityBase
    {
        public int Fetempcodi { get; set; }
        public int? Grupocodi { get; set; }
        public int? Equicodi { get; set; }
        public int Feeeqcodi { get; set; }
        public string Feeeqflagotraetapa { get; set; }
        public string Feeeqflagsistema { get; set; }
        
        public string Feeequsucreacion { get; set; }
        public DateTime? Feeeqfeccreacion { get; set; }
        public string Feeequsumodificacion { get; set; }
        public DateTime? Feeeqfecmodificacion { get; set; }
        public string Feeeqestado { get; set; }
        public string Feeeqflagcentral { get; set; }

    }

    public partial class FtExtEtempdeteqDTO
    {
        public string Equinomb { get; set; }
        public string Areanomb { get; set; }
        public string Emprnomb { get; set; }
        public string Famnomb { get; set; }
        public string Catenomb { get; set; }
        public int Emprcodi { get; set; }

        public int? Famcodi { get; set; }
        public int? Catecodi { get; set; }
        public int Areacodi { get; set; }

        public string FechaCreacionDesc { get; set; }
        public string FechaModificacionDesc { get; set; }
    }
}
