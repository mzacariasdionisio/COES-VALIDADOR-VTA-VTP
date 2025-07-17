using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RTU_ROLTURNO
    /// </summary>
    public class RtuRolturnoDTO : EntityBase
    {
        public string Rturolusucreacion { get; set; }
        public DateTime? Rturolfeccreacion { get; set; }
        public string Rturolusumodificacion { get; set; }
        public DateTime? Rturolfecmodificacion { get; set; }
        public int Rturolcodi { get; set; }
        public int? Rturolanio { get; set; }
        public int? Rturolmes { get; set; }
        public int Rtunrodia { get; set; }
        public string Rtumodtrabajo { get; set; }
        public int Percodi { get; set; }
        public string Pernombre { get; set; }
        public int Actcodi { get; set; }
        public string Actnombre { get; set; }
        public string Rtuactabreviatura { get; set; }
    }
}
