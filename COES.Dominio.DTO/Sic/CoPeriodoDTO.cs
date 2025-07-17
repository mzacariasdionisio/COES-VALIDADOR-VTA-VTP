using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_PERIODO
    /// </summary>
    public class CoPeriodoDTO : EntityBase
    {
        public int Copercodi { get; set; } 
        public int? Coperanio { get; set; } 
        public int? Copermes { get; set; } 
        public string Copernomb { get; set; } 
        public string Coperestado { get; set; } 
        public string Coperusucreacion { get; set; } 
        public DateTime? Copperfeccreacion { get; set; } 
        public string Copperusumodificacion { get; set; } 
        public DateTime? Copperfecmodificacion { get; set; } 
        public string Descmes { get; set; }
        public string UltimaVersion { get; set; }
         
    }
}
