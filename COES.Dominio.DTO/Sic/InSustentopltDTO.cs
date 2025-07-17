using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_SUSTENTOPLT
    /// </summary>
    public partial class InSustentopltDTO : EntityBase
    {
        public int Inpstcodi { get; set; }
        public int Inpsttipo { get; set; }
        public string Inpstnombre { get; set; }
        public string Inpstestado { get; set; }
        public string Inpstusumodificacion { get; set; }
        public DateTime? Inpstfecmodificacion { get; set; }
    }

    public partial class InSustentopltDTO
    {
        public string InpstfecmodificacionDesc { get; set; }
        public string InpstestadoDesc { get; set; }

        public List<InSustentopltItemDTO> Requisitos { get; set; }
    }
}
