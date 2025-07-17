using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SRM_CRITICIDAD
    /// </summary>
    public class SrmCriticidadDTO : EntityBase
    {
        public int Srmcrtcodi { get; set; } 
        public string Srmcrtdescrip { get; set; } 
        public string Srmcrtcolor { get; set; } 
        public string Srmcrtusucreacion { get; set; } 
        public DateTime? Srmcrtfeccreacion { get; set; } 
        public string Srmcrtusumodificacion { get; set; } 
        public DateTime? Srmcrtfecmodificacion { get; set; } 
    }
}
