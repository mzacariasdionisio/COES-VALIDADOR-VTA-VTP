using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_TOTAL_TRANSMISORES
    /// </summary>
    public class CpaTotalTransmisoresDTO
    {
        public int Cpattcodi { get; set; }

        public int Cpattanio { get; set; }
        public string Cpattajuste { get; set; }

        public int Cparcodi { get; set; }
        public string Cpattestado { get; set; }

        public string Cpattusucreacion { get; set; }
        public DateTime Cpattfeccreacion { get; set; }
        public string Cpattusumodificacion { get; set; }
        public DateTime Cpattfecmodificacion { get; set; }
    }
}

