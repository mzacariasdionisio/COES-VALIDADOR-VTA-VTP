using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_VOLUMEN_CALCULO
    /// </summary>
    public partial class CmVolumenCalculoDTO : EntityBase
    {
        public int Volcalcodi { get; set; }
        public DateTime Volcalfecha { get; set; }
        public int Volcalperiodo { get; set; }
        public string Volcaltipo { get; set; }

        public string Volcalusucreacion { get; set; }
        public DateTime Volcalfeccreacion { get; set; }
    }

    public partial class CmVolumenCalculoDTO
    {
        public string VolcalfechaDesc { get; set; }
        public string VolcalfeccreacionDesc { get; set; }
        public string VolcalperiodoDesc { get; set; }
        public string VolcaltipoDesc { get; set; }
    }
}
