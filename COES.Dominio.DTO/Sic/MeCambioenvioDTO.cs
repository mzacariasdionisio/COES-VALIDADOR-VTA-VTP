using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_CAMBIOENVIO
    /// </summary>
    public class MeCambioenvioDTO : EntityBase, ICloneable
    {
        public int Ptomedicodi { get; set; }
        public int Tipoinfocodi { get; set; }
        public int Enviocodi { get; set; }
        public DateTime Cambenvfecha { get; set; }
        public string Cambenvdatos { get; set; }
        public string Cambenvcolvar { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        //
        public int Formatcodi { get; set; }
        public string Ptomedibarranomb { get; set; }
        public string Tipoinfoabrev { get; set; }
        public DateTime Enviofechaperiodo { get; set; }
        public string Emprabrev { get; set; }

        public int Camenviocodi { get; set; }
        public int Hojacodi { get; set; }

        public int Tipoptomedicodi { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
