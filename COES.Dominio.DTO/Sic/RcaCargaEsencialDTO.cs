using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RCA_CARGA_ESENCIAL
    /// </summary>
    public class RcaCargaEsencialDTO : EntityBase
    {
        public int Rccarecodi { get; set; } 
        public int Emprcodi { get; set; } 
        public int? Equicodi { get; set; } 
        public decimal? Rccarecarga { get; set; } 
        public string Rccaredocumento { get; set; } 
        public DateTime? Rccarefecharecepcion { get; set; } 
        public string Rccareestado { get; set; } 
        public string Rccarenombarchivo { get; set; } 
        public string Rccareestregistro { get; set; } 
        public string Rccareusucreacion { get; set; } 
        public DateTime Rccarefeccreacion { get; set; } 
        public string Rccareusumodificacion { get; set; } 
        public DateTime? Rccarefecmodificacion { get; set; }

        public string Emprrazsocial { get; set; }
        public string Equinomb { get; set; }
        public string Areaabrev { get; set; }

        public string Areanomb { get; set; }

        public int Tipoemprcodi { get; set; }

        public bool EsUsuarioLibre { get; set; }
        public int Rccareorigen { get; set; }

        public int Rccaretipocarga { get; set; }
    }
}
