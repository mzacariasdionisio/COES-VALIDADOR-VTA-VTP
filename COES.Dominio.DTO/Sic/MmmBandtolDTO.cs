using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MMM_BANDTOL
    /// </summary>
    public class MmmBandtolDTO : EntityBase
    {
        public int Mmmtolcodi { get; set; }
        public int Immecodi { get; set; }
        public DateTime Mmmtolfechavigencia { get; set; }
        public string Mmmtolusucreacion { get; set; }
        public DateTime? Mmmtolfeccreacion { get; set; }
        public string Mmmtolnormativa { get; set; }
        public string Mmmtolusumodificacion { get; set; }
        public DateTime? Mmmtolfecmodificacion { get; set; }
        public string Mmmtolcriterio { get; set; }
        public decimal Mmmtolvalorreferencia { get; set; }
        public decimal Mmmtolvalortolerancia { get; set; }
        public string Mmmtolestado { get; set; }

        public string Immecodigo { get; set; }
        public string Immenombre { get; set; }

        public DateTime MmmtolfechavigenciaIni { get; set; }
        public DateTime MmmtolfechavigenciaFin { get; set; }
        public string MmmtolfechavigenciaDesc { get; set; }
        public string MmmtolfeccreacionDesc { get; set; }
        public string MmmtolfecmodificacionDesc { get; set; }
        public string MmmtolestadoDesc { get; set; }

        public string Periodo { get; set; }
        public string ClaseFila { get; set; }
        public bool Editable { get; set; }
    }
}
