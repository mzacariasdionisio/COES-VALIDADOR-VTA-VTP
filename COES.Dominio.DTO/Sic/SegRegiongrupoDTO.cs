using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SEG_REGIONGRUPO
    /// </summary>
    public class SegRegiongrupoDTO : EntityBase
    {
        public int Regcodi { get; set; } 
        public int Grupocodi { get; set; } 
        public int Reggcodi { get; set; } 
        public int Segcotipo { get; set; }
        public DateTime Reggfeccreacion { get; set; }
        public string Reggusucreacion { get; set; }
        public string Equinomb { get; set; }
        public string Tipoequipo { get; set; }

        public int RegcodiExcel { get; set; }
    }
}
