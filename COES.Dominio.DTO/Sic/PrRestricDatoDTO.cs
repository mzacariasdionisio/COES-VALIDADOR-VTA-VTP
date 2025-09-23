using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_RESTRIC_DATO
    /// </summary>
    public partial class PrRestricDatoDTO : EntityBase
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public int Rescfgcodi { get; set; } 
        public int Resdatcodi { get; set; } 
        public string Resdatdato { get; set; } 
        public string Resdatflag { get; set; } 
        public DateTime? Resdatfechaini { get; set; } 
        public int Resenvcodi { get; set; } 
        public DateTime? Resdatfechafin { get; set; } 
    }
    public partial class PrRestricDatoDTO 
    {
        public string Rescfgnombre { get; set; }
        public string ResdatfechainiDesc { get; set; }
        public string ResdatfechafinDesc { get; set; }
        public string Resdathoraini { get; set; }
        public string Resdathorafin { get; set; }
        public string Resdatdato1 { get; set; }
        public string Resdatdato2 { get; set; }
    }
}
