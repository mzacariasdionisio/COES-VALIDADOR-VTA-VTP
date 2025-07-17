using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_TOTAL_TRANSMISORESDET
    /// </summary>
    public class CpaTotalTransmisoresDetDTO
    {
        public int Cpattdcodi { get; set; }
        public int Cpattcodi { get; set; }
        public int Emprcodi { get; set; }
        public decimal? Cpattdtotmes01 { get; set; }
        public decimal? Cpattdtotmes02 { get; set; }
        public decimal? Cpattdtotmes03 { get; set; }
        public decimal? Cpattdtotmes04 { get; set; }
        public decimal? Cpattdtotmes05 { get; set; }
        public decimal? Cpattdtotmes06 { get; set; }
        public decimal? Cpattdtotmes07 { get; set; }
        public decimal? Cpattdtotmes08 { get; set; }
        public decimal? Cpattdtotmes09 { get; set; }
        public decimal? Cpattdtotmes10 { get; set; }
        public decimal? Cpattdtotmes11 { get; set; }
        public decimal? Cpattdtotmes12 { get; set; }
        public string Cpattdusucreacion { get; set; }
        public DateTime Cpattdfeccreacion { get; set; }

        public string Emprnomb { get; set; }

        /* CU17: INICIO */
        //public int Emprcodi { get; set; }
        public decimal? Cpattdtotal { get; set; }
        public int Cparcodi { get; set; }
        /* CU17: FIN */
    }
}

