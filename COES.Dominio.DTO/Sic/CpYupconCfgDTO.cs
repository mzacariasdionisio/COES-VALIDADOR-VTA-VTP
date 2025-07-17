using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CP_YUPCON_CFG
    /// </summary>
    public class CpYupconCfgDTO : EntityBase
    {
        public int Yupcfgcodi { get; set; }
        public int Tyupcodi { get; set; }
        public int Topcodi { get; set; }
        public string Yupcfgtipo { get; set; }
        public DateTime Yupcfgfecha { get; set; }
        public int Yupcfgbloquehorario { get; set; }

        public string Yupcfgusuregistro { get; set; }
        public DateTime Yupcfgfecregistro { get; set; }
    }
}
