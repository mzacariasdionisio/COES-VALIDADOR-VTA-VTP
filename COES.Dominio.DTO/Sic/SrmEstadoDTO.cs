using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SRM_ESTADO
    /// </summary>
    public class SrmEstadoDTO : EntityBase
    {
        public int Srmstdcodi { get; set; } 
        public string Srmstddescrip { get; set; } 
        public string Srmstdcolor { get; set; } 
        public string Srmstdusucreacion { get; set; } 
        public DateTime? Srmstdfeccreacion { get; set; } 
        public string Srmstdsumodificacion { get; set; } 
        public DateTime? Srmstdfecmodificacion { get; set; } 
    }
}
