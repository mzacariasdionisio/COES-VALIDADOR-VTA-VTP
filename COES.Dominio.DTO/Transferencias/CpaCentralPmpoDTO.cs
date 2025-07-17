using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_CENTRAL_PMPO
    /// </summary>
    public class CpaCentralPmpoDTO
    {
        public int Cpacnpcodi { get; set; }            // Mapeo de CPACNPCODI
        public int Cpacntcodi { get; set; }            // Mapeo de CPACNTCODI
        public int Cparcodi { get; set; }
        public int Ptomedicodi { get; set; }           // Mapeo de PTOMEDICODI
        public string Cpacnpusumodificacion { get; set; } // Mapeo de CPACNPUSUMODIFICACION
        public DateTime Cpacnpfecmodificacion { get; set; } // Mapeo de CPACNPFECMODIFICACION
        //CU04
        public string Ptomedidesc { get; set; }
    }
}
