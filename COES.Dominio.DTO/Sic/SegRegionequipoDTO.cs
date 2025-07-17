using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SEG_REGIONEQUIPO
    /// </summary>
    public class SegRegionequipoDTO : EntityBase
    {
        public int Equicodi { get; set; } 
        public int Regcodi { get; set; } 
        public int Regecodi { get; set; } 
        public int Segcotipo { get; set; } 
        public DateTime Regefeccreacion { get; set; }
        public string Regeusucreacion  { get; set; }
        public string Equinomb { get; set; }
        public string Tipoequipo { get; set; }
        public int Tipo { get; set; }

        public int RegcodiExcel { get; set; }
    }
}
