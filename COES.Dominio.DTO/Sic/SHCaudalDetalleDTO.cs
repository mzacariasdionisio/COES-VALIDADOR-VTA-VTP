using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SH_CAUDAL_DETALLE
    /// </summary>
    public partial class SHCaudalDetalleDTO : EntityBase
    {
		public int IdDet { get; set; }
        public int IdCaudal { get; set; }
		public int Anio { get; set; }
		public decimal? M1 { get; set; }
		public decimal? M2 { get; set; }
		public decimal? M3 { get; set; }
		public decimal? M4 { get; set; }
		public decimal? M5 { get; set; }
		public decimal? M6 { get; set; }
		public decimal? M7 { get; set; }
		public decimal? M8 { get; set; }
		public decimal? M9 { get; set; }
		public decimal? M10 { get; set; }
		public decimal? M11 { get; set; }
		public decimal? M12 { get; set; }
		public string IndM1 { get; set; }
		public string IndM2 { get; set; }
		public string IndM3 { get; set; }
		public string IndM4 { get; set; }
		public string IndM5 { get; set; }
		public string IndM6 { get; set; }
		public string IndM7 { get; set; }
		public string IndM8 { get; set; }
		public string IndM9 { get; set; }
		public string IndM10 { get; set; }
		public string IndM11 { get; set; }
		public string IndM12 { get; set; }

    }

}
