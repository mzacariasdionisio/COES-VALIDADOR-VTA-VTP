using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_REPORTE_INSUMOS
    /// </summary>
    public partial class IndReporteInsumosDTO : EntityBase, ICloneable
    {
        public int Irpinscodi { get; set; }
        public int Itotcodi { get; set; }
        public int Irpinstipo { get; set; }
        public decimal? D1 { get; set; }
        public decimal? D2 { get; set; }
        public decimal? D3 { get; set; }
        public decimal? D4 { get; set; }
        public decimal? D5 { get; set; }
        public decimal? D6 { get; set; }
        public decimal? D7 { get; set; }
        public decimal? D8 { get; set; }
        public decimal? D9 { get; set; }
        public decimal? D10 { get; set; }
        public decimal? D11 { get; set; }
        public decimal? D12 { get; set; }
        public decimal? D13 { get; set; }
        public decimal? D14 { get; set; }
        public decimal? D15 { get; set; }
        public decimal? D16 { get; set; }
        public decimal? D17 { get; set; }
        public decimal? D18 { get; set; }
        public decimal? D19 { get; set; }
        public decimal? D20 { get; set; }
        public decimal? D21 { get; set; }
        public decimal? D22 { get; set; }
        public decimal? D23 { get; set; }
        public decimal? D24 { get; set; }
        public decimal? D25 { get; set; }
        public decimal? D26 { get; set; }
        public decimal? D27 { get; set; }
        public decimal? D28 { get; set; }
        public decimal? D29 { get; set; }
        public decimal? D30 { get; set; }
        public decimal? D31 { get; set; }
        public string Irpinsusucreacion { get; set; }
        public DateTime Irpinsfeccreacion { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class IndReporteInsumosDTO
    {

    }
}
