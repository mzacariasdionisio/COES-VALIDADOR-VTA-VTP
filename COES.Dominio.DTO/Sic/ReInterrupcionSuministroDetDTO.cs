using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_INTERRUPCION_SUMINISTRO_DET
    /// </summary>
    public partial class ReInterrupcionSuministroDetDTO : EntityBase
    {
        public string Reintdevidenciaresp { get; set; } 
        public string Reintdconformidadsumi { get; set; } 
        public string Reintdcomentariosumi { get; set; } 
        public string Reintdevidenciasumi { get; set; } 
        public int Reintdcodi { get; set; } 
        public int? Reintcodi { get; set; } 
        public string Reintdestado { get; set; } 
        public int? Reintdorden { get; set; } 
        public int? Emprcodi { get; set; } 
        public decimal? Reintdorcentaje { get; set; } 
        public string Reintdconformidadresp { get; set; } 
        public string Reintdobservacionresp { get; set; } 
        public string Reintddetalleresp { get; set; } 
        public string Reintdcomentarioresp { get; set; }

        public string Reintdconformidadsumioriginal { get; set; }
        public string Reintdcomentariosumioriginal { get; set; }

        public string Reintdconformidadresporiginal { get; set; }
        public string Reintdobservacionresporiginal { get; set; }
        public string Reintddetalleresporiginal { get; set; }
        public string Reintdcomentarioresporiginal { get; set; }
        public string Reintdevidenciaresporiginal { get; set; }

        public string Reintddisposicion { get; set; }
        public string Reintdcompcero { get; set; }
    }

    public partial class ReInterrupcionSuministroDetDTO 
    {
        public string Emprnomb { get; set; }
        public int? SumId { get; set; }
        public string SumNomb { get; set; }
    }
}
