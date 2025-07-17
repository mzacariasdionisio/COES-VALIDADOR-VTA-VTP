using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RTU_ROLTURNO_DETALLE
    /// </summary>
    public class RtuRolturnoDetalleDTO : EntityBase
    {
        public int Rtudetcodi { get; set; }
        public int? Rtudetnrodia { get; set; }
        public string Rtudetmodtrabajo { get; set; }
        public int Rturolcodi { get; set; }
        public int Percodi { get; set; }
    }
}
