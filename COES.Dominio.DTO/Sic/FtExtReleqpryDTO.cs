using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_RELEQPRY
    /// </summary>
    public partial class FtExtReleqpryDTO : EntityBase
    {
        public int Ftreqpcodi { get; set; }
        public int? Equicodi { get; set; }
        public int? Grupocodi { get; set; }
        public int Ftprycodi { get; set; }
        public int? Ftreqpestado { get; set; }
        public string Ftreqpusucreacion { get; set; }
        public DateTime? Ftreqpfeccreacion { get; set; }
        public string Ftreqpusumodificacion { get; set; }
        public DateTime? Ftreqpfecmodificacion { get; set; }
    }

    public partial class FtExtReleqpryDTO
    {
        public string Emprnomb { get; set; }
        public string Ftpryeocodigo { get; set; }
        public string Ftpryeonombre { get; set; }
        public string Ftprynombre { get; set; }

        public string FtreqpestadoDesc { get; set; }
        public string FechaCreacionDesc { get; set; }
        public string FechaModificacionDesc { get; set; }

        public string Nombempresaelemento { get; set; }
        public int Idempresaelemento { get; set; }
        public string Nombreelemento { get; set; }
        public string Tipoelemento { get; set; }
        public string Areaelemento { get; set; }
        public string Estadoelemento { get; set; }

        public int? Famcodi { get; set; }
        public int? Catecodi { get; set; }

        public int? Idempresacopelemento { get; set; }
        public string Nombempresacopelemento { get; set; }

        public int NroItem { get; set; }
        public string Observaciones { get; set; }
        public string Equinomb { get; set; }
        public string Equiabrev { get; set; }
        public string Famnomb { get; set; }
        public string Equiestadodesc { get; set; }
        public string Emprnomb2 { get; set; }
        public string Areanomb { get; set; }

    }
}
