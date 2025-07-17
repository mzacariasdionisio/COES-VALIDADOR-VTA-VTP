using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla HT_CENTRAL_CFGDET
    /// </summary>
    public partial class HtCentralCfgdetDTO : EntityBase, ICloneable
    {
        public int Htcentcodi { get; set; } 
        public int Htcdetcodi { get; set; } 
        public int? Ptomedicodi { get; set; } 
        public int? Canalcodi { get; set; } 
        public decimal Htcdetfactor { get; set; } 
        public int Htcdetactivo { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class HtCentralCfgdetDTO
    {
        public string Ptomedielenomb { get; set; }
        public string Canalnomb { get; set; }
        public string HtcdetactivoDesc { get; set; }
        public int Tipo { get; set; }
    }
}
