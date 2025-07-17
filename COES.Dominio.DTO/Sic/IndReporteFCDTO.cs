using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_REPORTE_FC
    /// </summary>
    public partial class IndReporteFCDTO : EntityBase, ICloneable
    {
        public int Irptfccodi { get; set; }
        public int Itotcodi { get; set; }
        public int Irptfctipcombustible { get; set; }
        public string Irptfcnomcombustible { get; set; }
        public decimal Irptfcmw { get; set; }
        public decimal Irptfcm3h { get; set; }
        public decimal Irptfc1000m3h { get; set; }
        public decimal Irptfckpch { get; set; }
        public decimal Irptfcmmpch { get; set; }
        public decimal Irptfclh { get; set; }
        public decimal Irptfcgalh { get; set; }
        public decimal Irptfccmtr { get; set; }
        public int Irptfcnumdias { get; set; }
        public string Irptfcrngdias { get; set; }
        public int Irptfcsec { get; set; }
        public string Irptfcusucreacion { get; set; }
        public DateTime Irptfcfeccreacion { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

}
