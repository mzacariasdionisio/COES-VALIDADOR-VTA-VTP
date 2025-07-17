using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_PTOENTREGA_SUMINISTRADOR
    /// </summary>
    public class RePtoentregaSuministradorDTO : EntityBase
    {
        public int Reptsmcodi { get; set; } 
        public int? Repercodi { get; set; } 
        public int? Repentcodi { get; set; }

        public int? Emprcodi { get; set; } 
        public string Reptsmusucreacion { get; set; } 
        public DateTime? Reptsmfeccreacion { get; set; } 
        public string Reptsmusumodificacion { get; set; } 
        public DateTime? Reptsmfecmodificacion { get; set; } 
        public string Emprnomb { get; set; }
        public string Repentnombre { get; set; }
    }
}
