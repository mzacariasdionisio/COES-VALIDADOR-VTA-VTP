using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_YUPCON_CFGDET
    /// </summary>
    public partial class CpYupconCfgdetDTO : EntityBase, ICloneable
    {
        public int Ycdetcodi { get; set; }
        public int Yupcfgcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Topcodi { get; set; }
        public int Recurcodi { get; set; }
        public decimal Ycdetfactor { get; set; }

        public int Ycdetactivo { get; set; }
        public string Ycdetusuregistro { get; set; }
        public string Ycdetusumodificacion { get; set; }
        public DateTime? Ycdetfecregistro { get; set; }
        public DateTime? Ycdetfecmodificacion { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class CpYupconCfgdetDTO
    {
        public string Recurnombre { get; set; }
        public string Ptomedielenomb { get; set; }
        public string FactorDesc { get; set; }
        public int Famcodi { get; set; }

        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }
    }
}
