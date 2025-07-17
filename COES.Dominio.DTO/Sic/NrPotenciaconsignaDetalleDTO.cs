using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla NR_POTENCIACONSIGNA_DETALLE
    /// </summary>
    public class NrPotenciaconsignaDetalleDTO : EntityBase
    {
        public int Nrpcdcodi { get; set; } 
        public int? Nrpccodi { get; set; } 
        public DateTime? Nrpcdfecha { get; set; } 
        public decimal? Nrpcdmw { get; set; } 
        public string Nrpcdmaximageneracion { get; set; } 
        public string Nrpcdusucreacion { get; set; } 
        public DateTime? Nrpcdfeccreacion { get; set; } 
        public string Nrpcdusumodificacion { get; set; } 
        public DateTime? Nrpcdfecmodificacion { get; set; } 
        public string Nrpceliminado { get; set; }
    }
}
