using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EPO_REVISION_EO
    /// </summary>
    public class EpoRevisionEoDTO : EntityBase
    {
        public int Reveocodi { get; set; } 
        public int Esteocodi { get; set; } 
        public DateTime? Reveorevcoesfechaini { get; set; }
        public string StrReveorevcoesfechaini { get; set; }
        public string Reveorevcoescartarevisiontit { get; set; } 
        public string Reveorevcoescartarevisionenl { get; set; } 
        public string Reveorevcoescartarevisionobs { get; set; } 
        public string Reveorevcoesfinalizado { get; set; } 
        public DateTime? Reveocoesfechafin { get; set; }
        public string StrReveocoesfechafin { get; set; }
        public DateTime? Reveoenvesttercinvfechaini { get; set; }
        public string StrReveoenvesttercinvfechaini { get; set; }
        public string Reveoenvesttercinvtit { get; set; } 
        public string Reveoenvesttercinvenl { get; set; } 
        public string Reveoenvesttercinvobs { get; set; } 
        public string Reveoenvesttercinvfinalizado { get; set; } 
        public DateTime? Reveoenvesttercinvinvfechafin { get; set; }
        public string StrReveoenvesttercinvinvfechafin { get; set; }
        public DateTime? Reveorevterinvfechaini { get; set; }
        public string StrReveorevterinvfechaini { get; set; }
        public string Reveorevterinvtit { get; set; } 
        public string Reveorevterinvenl { get; set; } 
        public string Reveorevterinvobs { get; set; } 
        public string Reveorevterinvfinalizado { get; set; } 
        public DateTime? Reveorevterinvfechafin { get; set; }
        public string StrReveorevterinvfechafin { get; set; }
        public DateTime? Reveolevobsfechaini { get; set; } 
        public string StrReveolevobsfechaini { get; set; }
        public string Reveolevobstit { get; set; } 
        public string Reveolevobsenl { get; set; } 
        public string Reveolevobsobs { get; set; } 
        public string Reveolevobsfinalizado { get; set; } 
        public DateTime? Reveolevobsfechafin { get; set; }
        public string StrReveolevobsfechafin { get; set; }
        public DateTime? Lastdate { get; set; } 
        public string Lastuser { get; set; }
        public int Reveorevcoesampl { get; set; }
        public int Reveorevterinvampl { get; set; }

        public string RevisionConfirmidadEstudioEstado { get; set; }
        public string RevisionConfirmidadEstudioEstadoColor { get; set; }

        public string EnvioEstudioTercerInvolucradoEstado { get; set; }
        public string EnvioEstudioTercerInvolucradoEstadoColor { get; set; }

        public string RevisionEstudioEstado { get; set; }
        public string RevisionEstudioEstadoColor { get; set; }

        public string LevantamientObservacionEstado { get; set; }
        public string LevantamientObservacionEstadoColor { get; set; }
        

        public string Esteocodiusu { get; set; }
        public string Esteonomb { get; set; }
        public int Reveopreampl { get; set; }

    }
}
