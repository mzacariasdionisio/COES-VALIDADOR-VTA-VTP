using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_TIPOINFORMACION
    /// </summary>
    public class CoTipoinformacionDTO : EntityBase
    {
        public string Cotinfestado { get; set; } 
        public string Cotinfusucreacion { get; set; } 
        public DateTime? Cotinffeccreacion { get; set; } 
        public string Cotinfusumodificacion { get; set; } 
        public DateTime? Cotinffecmodificacion { get; set; } 
        public int Cotinfcodi { get; set; } 
        public string Cotinfdesc { get; set; } 
    }
}
