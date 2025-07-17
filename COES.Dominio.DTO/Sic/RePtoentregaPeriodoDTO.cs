using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RE_PTOENTREGA_PERIODO
    /// </summary>
    public class RePtoentregaPeriodoDTO : EntityBase
    {
        public int Reptopcodi { get; set; } 
        public int? Repentcodi { get; set; } 
        public int? Repercodi { get; set; } 
        public string Reptopusucreacion { get; set; } 
        public DateTime? Reptopfeccreacion { get; set; } 
        public string Reptopusumodificacion { get; set; } 
        public DateTime? Reptopfecmodificacion { get; set; } 
        public string Repentnombre { get; set; }
        public string Rentabrev { get; set; }
        public int? Rentcodi { get; set; }
    }

    public class ReEmpresaDTO
    {
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public int Tipoemprcodi { get; set; }

        public bool HabilitacionPE { get; set; }
        public bool HabilitacionRC { get; set; }
    }
}
