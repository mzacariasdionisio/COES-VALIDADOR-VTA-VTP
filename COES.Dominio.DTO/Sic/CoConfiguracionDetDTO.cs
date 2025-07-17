using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_CONFIGURACION_DET
    /// </summary>
    public partial class CoConfiguracionDetDTO : EntityBase
    {
        public int Courdecodi { get; set; } 
        public int? Conurscodi { get; set; } 
        public string Courdetipo { get; set; }
        public string Courdeoperacion { get; set; } 
        public string Courdereporte { get; set; } 
        public string Courdeequipo { get; set; } 
        public decimal? Courderequip { get; set; } 
        public DateTime? Courdevigenciadesde { get; set; } 
        public DateTime? Courdevigenciahasta { get; set; } 
        public string Courdeusucreacion { get; set; } 
        public DateTime? Courdefeccreacion { get; set; } 
        public string Courdeusumodificacion { get; set; } 
        public DateTime? Courdefecmodificacion { get; set; } 

        
    }

    public partial class CoConfiguracionDetDTO
    {
        public int Grupocodi { get; set; }
        public DateTime? FecInicioHabilitacion { get; set; }
        public DateTime? FecFinHabilitacion { get; set; }
        public int ContadorSenial { get; set; }
        public string Coverdesc { get; set; }
        public int Copercodi { get; set; }
        public int Covercodi { get; set; }

    }
}
