using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_VERSION_DATDET
    /// </summary>
    public partial class SiVersionDatdetDTO : EntityBase
    {
        public int Vdatdtcodi { get; set; }
        public string Vdatdtvalor { get; set; }        
        public DateTime? Vdatdtfecha { get; set; }
        public int Verdatcodi { get; set; }
        public int Vercnpcodi { get; set; }
        public int Vdatdtid { get; set; }
    }

    public partial class SiVersionDatdetDTO 
    {
        public decimal? ValorDecimal { get; set; }
    }
}
