using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_MENUREPORTE
    /// </summary>
    public class SiMenureporteDTO : EntityBase
    {
        public int Repcodi { get; set; } 
        public string Repdescripcion { get; set; } 
        public string Repabrev { get; set; } 
        public int? Reptiprepcodi { get; set; } 
        public int? Repcatecodi { get; set; } 
        public int? Repstado { get; set; } 
        public string Repusucreacion { get; set; } 
        public DateTime? Repffeccreacion { get; set; } 
        public string Repusumodificacion { get; set; } 
        public DateTime? Repfecmodificacion { get; set; }
        public int? Repprojectcodi { get; set; }        
        public int? Reporden { get; set; }

        public string Mreptituloweb { get; set; }
        public string Mreptituloexcel { get; set; }
    }
}
