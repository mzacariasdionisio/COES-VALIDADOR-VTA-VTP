using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_EQUIVALENCIAMODOP
    /// </summary>
    public class CmEquivalenciamodopDTO : EntityBase
    {
        public int Equimocodi { get; set; } 
        public int Grupocodi { get; set; } 
        public string Equimonombrencp { get; set; } 
        public string Equimousucreacion { get; set; } 
        public DateTime? Equimofeccreacion { get; set; } 
        public string Equimousumodificacion { get; set; } 
        public DateTime? Equimofecmodificacion { get; set; } 
        public string Gruponomb { get; set; }
    }
}
