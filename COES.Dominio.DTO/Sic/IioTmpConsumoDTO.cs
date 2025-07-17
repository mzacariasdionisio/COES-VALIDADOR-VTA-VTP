using System;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IIO_TMP_CONSUMO
    /// </summary>
    public class IioTmpConsumoDTO
    {
        public int Psiclicodi { get; set; }
        public string Uconempcodi { get; set; }
        public string Sumucodi { get; set; }
        public DateTime Uconfecha { get; set; }
        public string Uconptosumincodi { get; set; }
        public decimal Uconenergactv { get; set; }
        public decimal Uconenergreac { get; set; }
        public int Ptomedicodi { get; set; }

    }
}