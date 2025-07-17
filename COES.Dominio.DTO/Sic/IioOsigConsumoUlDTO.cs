using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IIO_OSIG_CONSUMO_UL
    /// </summary>
    public class IioOsigConsumoUlDTO
    {
        public int Psiclicodi { get; set; }
        public string Ulconcodempresa { get; set; }
        public string Ulconcodsuministro { get; set; }
        public DateTime Ulconfecha { get; set; }
        public string Ulconcodbarra { get; set; }
        public decimal Ulconenergactv { get; set; }
        public decimal Ulconenergreac { get; set; }
        public int Ptomedicodi { get; set; }
        public string Ulconusucreacion { get; set; }
        public DateTime Ulconfeccreacion { get; set; }
    }
}