using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_DESPACHOURS
    /// </summary>
    public class VcrDespachoursDTO : EntityBase
    {
        public int Vcdurscodi { get; set; } 
        public int Vcrecacodi { get; set; } 
        public string Vcdurstipo { get; set; } 
        public int Grupocodi { get; set; } 
        public string Gruponomb { get; set; }
        public int Equicodi { get; set; }
        public int Emprcodi { get; set; }
        public DateTime? Vcdursfecha { get; set; } 
        public decimal Vcdursdespacho { get; set; } 
        public string Vcdursusucreacion { get; set; } 
        public DateTime Vcdursfeccreacion { get; set; }
        //Atributos para l consulta
        public string Equinomb { get; set; }
    }
}
