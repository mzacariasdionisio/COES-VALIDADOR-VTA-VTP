using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_SUSTENTO
    /// </summary>
    public partial class InSustentoDTO : EntityBase, ICloneable
    {
        public int Instcodi { get; set; }
        public string Instestado { get; set; }
        public string Instusumodificacion { get; set; }
        public DateTime? Instfecmodificacion { get; set; }
        public int Intercodi { get; set; }
        public int Inpstcodi { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class InSustentoDTO
    {
        public int Inpsttipo { get; set; }
        public List<InSustentoDetDTO> ListaItem { get; set; }

        public bool TienePlantillaCompleta { get; set; }

        public bool FlagTieneInclusion { get; set; }
        public bool FlagTieneExclusion { get; set; }
    }
}
