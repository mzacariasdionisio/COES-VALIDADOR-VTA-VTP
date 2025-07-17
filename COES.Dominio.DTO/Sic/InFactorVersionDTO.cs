using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_FACTOR_VERSION
    /// </summary>
    public partial class InFactorVersionDTO : EntityBase
    {
        public int Infvercodi { get; set; }
        public DateTime Infverfechaperiodo { get; set; }
        public string InfverfechaperiodoDesc { get; set; }
        public string Infvertipoeq { get; set; }
        public string InfvertipoeqDesc { get; set; }
        public string Infverdisp { get; set; }
        public string InfverdispDesc { get; set; }
        public string Infverflagfinal { get; set; }
        public string InfverflagfinalDesc { get; set; }
        public string Infverflagdefinitivo { get; set; }
        public decimal Infverf1 { get; set; }
        public decimal Infverf2 { get; set; }
        public string Infverusucreacion { get; set; }
        public DateTime Infverfeccreacion { get; set; }
        public string InfverfeccreacionDesc { get; set; }
        public int Infvernro { get; set; }
        public decimal Infvercumpl { get; set; }
        public int Infverhorizonte { get; set; }
        public int Infvermodulo { get; set; }
    }

    public partial class InFactorVersionDTO
    {
        public int Mes { get; set; }
        public int Anio { get; set; }
    }
}