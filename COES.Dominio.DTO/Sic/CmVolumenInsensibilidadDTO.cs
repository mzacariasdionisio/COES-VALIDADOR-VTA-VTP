using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_VOLUMEN_INSENSIBILIDAD
    /// </summary>
    public class CmVolumenInsensibilidadDTO : EntityBase
    {
        public int Volinscodi { get; set; } 
        public DateTime? Volinsfecha { get; set; } 
        public int Recurcodi { get; set; } 
        public int Topcodi { get; set; } 
        public decimal? Volinsvolmin { get; set; } 
        public decimal? Volinsvolmax { get; set; } 
        public DateTime? Volinsinicio { get; set; } 
        public DateTime? Volinsfin { get; set; } 
        public string Volinsusucreacion { get; set; } 
        public DateTime? Volinsfecreacion { get; set; } 
        public string Volinsusumodificacion { get; set; } 
        public DateTime? Volinsfecmodificacion { get; set; } 
    }
}
