using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_PORCENTAJE_ENVIO
    /// </summary>
    public class CpaPorcentajeEnvioDTO
    {
        public int Cpapecodi { get; set; }             // Mapeo de CPAPECODI
        public int Cpapcodi { get; set; }             // Mapeo de CPAPCODI
        public int Cparcodi { get; set; }             // Mapeo de CPARCODI
        public string Cpapetipo { get; set; }          // Mapeo de CPAPETIPO
        public int? Cpapemes { get; set; }            // Mapeo de CPAPEMES
        public int Cpapenumenvio { get; set; }        // Mapeo de CPAPENUMENVIO
        public string Cpapeusucreacion { get; set; }  // Mapeo de CPAPEUSUCREACION
        public DateTime Cpapefeccreacion { get; set; }// Mapeo de CPAPEFECCREACION
    }
}
