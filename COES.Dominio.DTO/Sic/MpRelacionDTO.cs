using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MP_RELACION
    /// </summary>
    public class MpRelacionDTO : EntityBase
    {
        public int Mtopcodi { get; set; } 
        public int Mtrelcodi { get; set; } 
        public int Mrecurcodi1 { get; set; } 
        public int Mrecurcodi2 { get; set; } 
        public decimal? Mrelvalor { get; set; } 
        public string Mrelusumodificacion { get; set; } 
        public DateTime? Mrelfecmodificacion { get; set; } 
    }
}
