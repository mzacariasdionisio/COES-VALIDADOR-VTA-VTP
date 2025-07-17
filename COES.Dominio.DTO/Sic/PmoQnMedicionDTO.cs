using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PMO_QN_MEDICION
    /// </summary>
    public partial class PmoQnMedicionDTO : EntityBase
    {
        public int Qnmedcodi { get; set; }
        public int Sddpcodi { get; set; }
        public int Qnlectcodi { get; set; }
        public int Qnbenvcodi { get; set; }
        public DateTime Qnmedfechaini { get; set; }
        public DateTime Qnmedfechafin { get; set; }
        public int Qnmedsemini { get; set; }
        public int Qnmedsemfin { get; set; }
        public DateTime Qnmedanio { get; set; }
        public decimal? Qnmedh1 { get; set; }
        public decimal? Qnmedh2 { get; set; }
        public decimal? Qnmedh3 { get; set; }
        public decimal? Qnmedh4 { get; set; }
        public decimal? Qnmedh6 { get; set; }
        public decimal? Qnmedh5 { get; set; }
        public decimal? Qnmedh7 { get; set; }
        public decimal? Qnmedh8 { get; set; }
        public decimal? Qnmedh9 { get; set; }
        public decimal? Qnmedh10 { get; set; }
        public decimal? Qnmedh11 { get; set; }
        public decimal? Qnmedh12 { get; set; }
        public decimal? Qnmedh13 { get; set; }
        public int? Qnmedo1 { get; set; }
        public int? Qnmedo2 { get; set; }
        public int? Qnmedo3 { get; set; }
        public int? Qnmedo4 { get; set; }
        public int? Qnmedo5 { get; set; }
        public int? Qnmedo6 { get; set; }
        public int? Qnmedo7 { get; set; }
        public int? Qnmedo8 { get; set; }
        public int? Qnmedo9 { get; set; }
        public int? Qnmedo10 { get; set; }
        public int? Qnmedo11 { get; set; }
        public int? Qnmedo12 { get; set; }
        public int? Qnmedo13 { get; set; }
    }

    public partial class PmoQnMedicionDTO
    {
        public string NombreSddp { get; set; }
        public int Sddpnum { get; set; }
        public string Referencia { get; set; }
        public int Orden { get; set; }
        public string FechainiDesc { get; set; }
        public string FechafinDesc { get; set; }
        public int Anio { get; set; }
        public int OrigenInformacion { get; set; }
    }
}
