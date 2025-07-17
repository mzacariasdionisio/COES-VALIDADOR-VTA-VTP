using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MP_PROPRECURSO
    /// </summary>
    public partial class MpProprecursoDTO : EntityBase
    {
        public int Mtopcodi { get; set; } 
        public int Mrecurcodi { get; set; } 
        public int Mpropcodi { get; set; } 
        public DateTime? Mprvalfecvig { get; set; } 
        public string Mprvalvalor { get; set; } 
    }

    public partial class MpProprecursoDTO
    {
        public int? OrdenPropiedad  { get; set; }
        public int AnchoPropiedad { get; set; }
        public string TipoDato { get; set; }
    }
}
