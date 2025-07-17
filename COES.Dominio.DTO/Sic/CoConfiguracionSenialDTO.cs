using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_CONFIGURACION_SENIAL
    /// </summary>
    public partial class CoConfiguracionSenialDTO : EntityBase
    {
        public int Consencodi { get; set; } 
        public int? Courdecodi { get; set; } 
        public int? Cotidacodi { get; set; } 
        public int? Zonacodi { get; set; } 
        public int? Canalcodi { get; set; } 
        public int? Grupocodi { get; set; } 
        public int? Equicodi { get; set; } 
        public decimal? Consenvalinicial { get; set; } 
        public string Consenusucreacion { get; set; } 
        public DateTime? Consenfeccreacion { get; set; } 
        public string Consenusumodificacion { get; set; } 
        public DateTime? Consenfecmodificacion { get; set; } 
    }

    public partial class CoConfiguracionSenialDTO 
    {
        public int? Coperanio { get; set; }
        public int? Copermes { get; set; }
        public int? Covercodi { get; set; }
        public string Canalnomb { get; set; }
    }
}
