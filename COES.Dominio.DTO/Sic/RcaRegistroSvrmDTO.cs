using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RCA_REGISTRO_SVRM
    /// </summary>
    public class RcaRegistroSvrmDTO : EntityBase
    {
        public int Rcsvrmcodi { get; set; } 
        public int? Equicodi { get; set; } 
        public decimal? Rcsvrmhperacmf { get; set; } 
        public decimal? Rcsvrmhperacmt { get; set; } 
        public decimal? Rcsvrmhfperacmf { get; set; } 
        public decimal? Rcsvrmhfperacmt { get; set; } 
        public decimal? Rcsvrmmaxdemcont { get; set; } 
        public decimal? Rcsvrmmaxdemdisp { get; set; } 
        public decimal? Rcsvrmmaxdemcomp { get; set; } 
        public string Rcsvrmdocumento { get; set; } 
        public DateTime? Rcsvrmfechavigencia { get; set; } 
        public string Rcsvrmestado { get; set; } 
        public string Rcsvrmestregistro { get; set; } 
        public string Rcsvrmusucreacion { get; set; } 
        public DateTime Rcsvrmfeccreacion { get; set; } 
        public string Rcsvrmusumodificacion { get; set; } 
        public DateTime? Rcsvrmfecmodificacion { get; set; }

        public string Emprrazsocial { get; set; }

        public string Equinomb { get; set; }

        public string Areaabrev { get; set; }

        public string Areanomb { get; set; }

        public string Osinergcodi { get; set; }

        public int Emprcodi { get; set; }

        public string Emprsum { get; set; }
    }
}
