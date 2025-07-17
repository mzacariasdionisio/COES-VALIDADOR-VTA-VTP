using COES.Base.Core;
using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_TOTAL_DEMANDADET
    /// </summary>
    public class CpaTotalDemandaDetDTO : EntityBase
    {
        public int Cpatddcodi { get; set; }
        public int Cpatdcodi { get; set; }
        public int Emprcodi { get; set; }
        public decimal? Cpatddtotenemwh { get; set; }
        public decimal? Cpatddtotenesoles { get; set; }
        public decimal? Cpatddtotpotmw { get; set; }
        public decimal? Cpatddtotpotsoles { get; set; }
        public string Cpatddusucreacion { get; set; }
        public DateTime Cpatddfeccreacion { get; set; }

        public string Emprnomb { get; set; }

        /* CU17: INICIO */
        //public int Emprcodi { get; set; }
        public int Cparcodi { get; set; }
        public string Cpatdtipo { get; set; }
        public int Cpatdmes { get; set; }
        public string Cpatdestado { get; set; }
        /* CU17: FIN */

    }
}

