using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PFR_PERIODO
    /// </summary>
    public class PfrPeriodoDTO : EntityBase
    {
        public int Pfrpercodi { get; set; }
        public string Pfrpernombre { get; set; }
        public int Pfrperanio { get; set; }
        public int Pfrpermes { get; set; }
        public int Pfrperaniomes { get; set; }
        public string Pfrperusucreacion { get; set; }
        public DateTime? Pfrperfeccreacion { get; set; }
        public DateTime? Pfrperfecmodificacion { get; set; }
        public string Pfrperusumodificacion { get; set; }
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
