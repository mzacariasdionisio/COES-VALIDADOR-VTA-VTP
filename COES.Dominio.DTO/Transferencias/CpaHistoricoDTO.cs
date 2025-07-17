using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_HISTORICO
    /// </summary>
    public class CpaHistoricoDTO
    {
        public int Cpahcodi { get; set; }               // Mapeo de CPAHCODI
        public int Cparcodi { get; set; }               // Mapeo de CPARCODI
        public string Cpahtipo { get; set; }            // Mapeo de CPAHTIPO
        public string Cpahusumodificacion { get; set; } // Mapeo de CPAHUSUMODIFICACION
        public DateTime Cpahfecmodificacion { get; set; } // Mapeo de CPAHFECMODIFICACION

        //Additional
        public string CpahtipoDesc { get; set; }
        public string CpahfecmodificacionDesc { get; set; }


    }
}