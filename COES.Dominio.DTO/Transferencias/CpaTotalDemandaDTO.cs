using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_TOTAL_DEMANDA
    /// </summary>
    public class CpaTotalDemandaDTO
    {
        public int Cpatdcodi { get; set; }

        public int Cpatdanio { get; set; }
        public string Cpatdajuste { get; set; }

        public int Cparcodi { get; set; }

        public string Cpatdtipo { get; set; }
        public int Cpatdmes { get; set; }

        public string Cpatdestado { get; set; }
        public string Cpatdusucreacion { get; set; }
        public DateTime Cpatdfeccreacion { get; set; }
        public string Cpatdusumodificacion { get; set; }
        public DateTime Cpatdfecmodificacion { get; set; }
    }
}

