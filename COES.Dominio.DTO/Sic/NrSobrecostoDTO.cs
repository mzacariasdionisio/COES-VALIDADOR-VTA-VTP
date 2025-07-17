using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla NR_SOBRECOSTO
    /// </summary>
    public class NrSobrecostoDTO : EntityBase
    {
        public int Nrsccodi { get; set; }         
        public DateTime? Nrscfecha { get; set; } 
        public decimal? Nrsccodespacho0 { get; set; } 
        public decimal? Nrsccodespacho1 { get; set; } 
        public decimal? Nrscsobrecosto { get; set; } 
        public string Nrscnota { get; set; }         
        public string Nrsceliminado { get; set; } 
        public int? Nrscpadre { get; set; } 
        public string Nrscusucreacion { get; set; } 
        public DateTime? Nrscfeccreacion { get; set; } 
        public string Nrscusumodificacion { get; set; } 
        public DateTime? Nrscfecmodificacion { get; set; } 
        
    }
}
