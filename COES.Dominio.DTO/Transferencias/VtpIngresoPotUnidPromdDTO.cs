using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VTP_INGRESO_POTUNID_PROMD
    /// </summary>
    public class VtpIngresoPotUnidPromdDTO : EntityBase
    {
        public int Inpuprcodi { get; set; } 
        public int Pericodi { get; set; } 
        public int Recpotcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodi { get; set; }
        public decimal Inpuprimportepromd { get; set; } 
        public string Inpuprusucreacion { get; set; } 
        public DateTime Inpuprfeccreacion { get; set; }
        public int? Grupocodi { get; set; }
        public string Inpuprunidadnomb { get; set; }
        public int? Inpuprficticio { get; set; }
        //Para los nombres
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public string Recpotnombre { get; set; }
        public string Perinombre { get; set; }
        public int Perianio { get; set; }
        public int Perimes { get; set; }
        public int Perianiomes { get; set; }
        public string Recanombre { get; set; }
        public List<decimal> lstImportesPromd { get; set; }
        public decimal PorcentajeVariacion { get; set; }

    }
}
