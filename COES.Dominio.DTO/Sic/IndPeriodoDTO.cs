using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_PERIODO
    /// </summary>
    public partial class IndPeriodoDTO : EntityBase
    {
        public int Ipericodi { get; set; }
        public string Iperihorizonte { get; set; }
        public string Iperinombre { get; set; }
        public int Iperianio { get; set; }
        public int Iperimes { get; set; }
        public int Iperianiomes { get; set; }
        public string Iperiestado { get; set; }
        public int Iperianiofin { get; set; }
        public int Iperimesfin { get; set; }
        public string Iperiusucreacion { get; set; }
        public DateTime? Iperifeccreacion { get; set; }
        public string Iperiusumodificacion { get; set; }
        public DateTime? Iperifecmodificacion { get; set; }

    }

    public partial class IndPeriodoDTO 
    {
        public DateTime FechaIni { get; set; }
        public DateTime FechaFin { get; set; }
        public string FechaDesc { get; set; }
        public int TotalDias { get; set; }
        public int TotalHP { get; set; }
        public int TotalMes { get; set; }
        public bool EsUltimoAnio { get; set; }
    }
}
