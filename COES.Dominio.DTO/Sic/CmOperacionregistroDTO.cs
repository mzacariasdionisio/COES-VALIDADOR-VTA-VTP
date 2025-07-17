using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_OPERACIONREGISTRO
    /// </summary>
    public class CmOperacionregistroDTO : EntityBase
    {
        public int Operegcodi { get; set; } 
        public int Grupocodi { get; set; } 
        public int? Subcausacodi { get; set; } 
        public DateTime? Operegfecinicio { get; set; } 
        public DateTime? Operegfecfin { get; set; } 
        public string Operegusucreacion { get; set; } 
        public DateTime? Operegfeccreacion { get; set; } 
        public string Operegusumodificacion { get; set; } 
        public DateTime? Operegfecmodificacion { get; set; } 
        public string Gruponomb { get; set; }
        public string Subcausadesc { get; set; }
    }
}
