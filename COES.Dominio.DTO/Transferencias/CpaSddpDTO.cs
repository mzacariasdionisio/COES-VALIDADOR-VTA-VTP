using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_SDDP
    /// </summary>
    public class CpaSddpDTO
    {
        public int Cpsddpcodi { get; set; }             // Mapeo de CPSDDPCODI
        public int Cparcodi { get; set; }               // Mapeo de CPARCODI
        public int Cpsddpcorrelativo { get; set; }      // Mapeo de CPSDDPCORRELATIVO
        public string Cpsddpnomarchivo { get; set; }    // Mapeo de CPSDDPNOMARCHIVO
        public int Cpsddpsemanaini { get; set; }        // Mapeo de CPSDDPSEMANAINI
        public int Cpsddpanioini { get; set; }          // Mapeo de CPSDDPANIOINI
        public int Cpsddpnroseries { get; set; }        // Mapeo de CPSDDPNROSERIES
        public DateTime Cpsddpdiainicio { get; set; }   // Mapeo de CPSDDPDIAINICIO
        public string Cpsddpusucreacion { get; set; }   // Mapeo de CPSDDPUSUCREACION
        public DateTime Cpsddpfeccreacion { get; set; } // Mapeo de CPSDDPFECCREACION
    }
}
