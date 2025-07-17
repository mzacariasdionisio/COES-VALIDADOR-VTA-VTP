using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_INTERRUPCION_ACCESO
    /// </summary>
    public partial class ReInterrupcionAccesoDTO : EntityBase
    {
        public int Reinaccodi { get; set; } 
        public int? Emprcodi { get; set; } 
        public int? Repercodi { get; set; } 
        public string Reinacptoentrega { get; set; } 
        public string Reinacrechazocarga { get; set; } 
        public string Reinacusucreacion { get; set; } 
        public DateTime? Reinacfeccreacion { get; set; } 
        public string Reinacusumodificacion { get; set; } 
        public DateTime? Reinacfecmodificacion { get; set; } 
    }

    public partial class ReInterrupcionAccesoDTO 
    {
        public string Emprnomb { get; set; }
    }
}
