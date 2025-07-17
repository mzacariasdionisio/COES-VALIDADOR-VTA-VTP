using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPO_REVISION_EPO
    /// </summary>
    public class EpoRevisionEpoDTO : EntityBase
    {
        public int Revepocodi { get; set; } 
        public int Estepocodi { get; set; } 
        public DateTime? Reveporevcoesfechaini { get; set; }
        public string StrReveporevcoesfechaini { get; set; }
        public string Reveporevcoescartarevisiontit { get; set; } 
        public string Reveporevcoescartarevisionenl { get; set; } 
        public string Reveporevcoescartarevisionobs { get; set; } 
        public string Reveporevcoesfinalizado { get; set; } 
        public DateTime? Revepocoesfechafin { get; set; }
        public string StrRevepocoesfechafin { get; set; }
        public DateTime? Revepoenvesttercinvfechaini { get; set; } 
        public string StrRevepoenvesttercinvfechaini { get; set; }
        public string Revepoenvesttercinvtit { get; set; } 
        public string Revepoenvesttercinvenl { get; set; } 
        public string Revepoenvesttercinvobs { get; set; } 
        public string Revepoenvesttercinvfinalizado { get; set; } 
        public DateTime? Revepoenvesttercinvinvfechafin { get; set; }
        public string StrRevepoenvesttercinvinvfechafin { get; set; }
        public DateTime? Reveporevterinvfechaini { get; set; } 
        public string StrReveporevterinvfechaini { get; set; }
        public string Reveporevterinvtit { get; set; } 
        public string Reveporevterinvenl { get; set; } 
        public string Reveporevterinvobs { get; set; } 
        public string Reveporevterinvfinalizado { get; set; } 
        public DateTime? Reveporevterinvfechafin { get; set; }
        public string StrReveporevterinvfechafin { get; set; }
        public DateTime? Revepolevobsfechaini { get; set; }
        public string StrRevepolevobsfechaini { get; set; }
        public string Revepolevobstit { get; set; } 
        public string Revepolevobsenl { get; set; } 
        public string Revepolevobsobs { get; set; } 
        public string Revepolevobsfinalizado { get; set; } 
        public DateTime? Revepolevobsfechafin { get; set; } 
        public string StrRevepolevobsfechafin { get; set; }
        public DateTime? Lastdate { get; set; } 
        public string Lastuser { get; set; }
        public int Reveporevcoesampl { get; set; }
        public int Reveporevterinvampl { get; set; }

        public string RevisionConfirmidadEstudioEstado { get; set; }
        public string RevisionConfirmidadEstudioEstadoColor { get; set; }

        public string EnvioEstudioTercerInvolucradoEstado { get; set; }
        public string EnvioEstudioTercerInvolucradoEstadoColor { get; set; }

        public string RevisionEstudioEstado { get; set; }
        public string RevisionEstudioEstadoColor { get; set; }

        public string LevantamientObservacionEstado { get; set; }
        public string LevantamientObservacionEstadoColor { get; set; }

        public string Estepocodiusu { get; set; }
        public string Esteponomb { get; set; }
        public int Revepopreampl { get; set; }

    }
}
