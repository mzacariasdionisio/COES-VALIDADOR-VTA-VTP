using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_PORCENTAJE
    /// </summary>
    public class CpaPorcentajeDTO
    {
        public int Cpapcodi { get; set; }            // Mapeo de CPAPCODI
        public int Cparcodi { get; set; }            // Mapeo de CPARCODI
        public string Cpaplog { get; set; }          // Mapeo de CPAPLOG
        public string Cpapestpub { get; set; }         // Mapeo de CPAPESTPUB
        public string Cpapusucreacion { get; set; }  // Mapeo de CPAPUSUCREACION
        public DateTime Cpapfeccreacion { get; set; } // Mapeo de CPAPFECCREACION
        public string Cpapusumodificacion { get; set; }  // Mapeo de CPAPUSUMODIFICACION
        public DateTime Cpapfecmodificacion { get; set; } // Mapeo de CPAPFECMODIFICACION

        // Additional properties can be added as needed
    }

}