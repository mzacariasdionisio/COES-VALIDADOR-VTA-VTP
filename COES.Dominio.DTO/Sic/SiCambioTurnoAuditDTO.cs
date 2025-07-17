using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_CAMBIO_TURNO_AUDIT
    /// </summary>
    public class SiCambioTurnoAuditDTO : EntityBase
    {
        public DateTime? Lastdate { get; set; } 
        public string Desaccion { get; set; } 
        public int? Cambioturnocodi { get; set; } 
        public int Turnoauditcodi { get; set; } 
        public string Lastuser { get; set; } 
    }
}
