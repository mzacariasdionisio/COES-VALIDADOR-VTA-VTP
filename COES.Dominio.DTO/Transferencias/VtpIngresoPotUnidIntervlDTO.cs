using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_INGRESO_POTUNID_INTERVL
    /// </summary>
    public class VtpIngresoPotUnidIntervlDTO : EntityBase
    {
        public int Inpuincodi { get; set; } 
        public int Pericodi { get; set; } 
        public int Recpotcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodi { get; set; }
        public int Ipefrcodi { get; set; }
        public int Inpuinintervalo { get; set; }
        public int Inpuindia { get; set; }
        public decimal Inpuinimporte { get; set; } 
        public string Inpuinusucreacion { get; set; } 
        public DateTime Inpuinfeccreacion { get; set; }
        public int? Grupocodi { get; set; }
        public string Inpuinunidadnomb { get; set; }
        public int? Inpuinficticio { get; set; }

        public string Emprnomb { get; set; }

        public string Equinomb { get; set; }
    }
}
