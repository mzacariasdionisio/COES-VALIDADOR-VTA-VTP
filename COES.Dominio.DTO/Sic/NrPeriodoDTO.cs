using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla NR_PERIODO
    /// </summary>
    public class NrPeriodoDTO : EntityBase
    {
        public int Nrpercodi { get; set; } 
        public DateTime Nrpermes { get; set; } 
        public string Nrpereliminado { get; set; } 
        public string Nrperusucreacion { get; set; } 
        public DateTime? Nrperfeccreacion { get; set; } 
        public string Nrperusumodificacion { get; set; } 
        public DateTime? Nrperfecmodificacion { get; set; } 
    }
}
