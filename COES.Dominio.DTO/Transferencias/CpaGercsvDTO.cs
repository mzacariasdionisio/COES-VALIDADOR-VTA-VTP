using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_GERCSV
    /// </summary>
    public class CpaGercsvDTO
    {
        public int Cpagercodi { get; set; }            // Mapeo de CPAGERCODI
        public int Cpsddpcodi { get; set; }            // Mapeo de CPSDDPCODI
        public string Cpagergndarchivo { get; set; }  // Mapeo de CPAGERGNDARCHIVO
        public string Cpagerhidarchivo { get; set; }  // Mapeo de CPAGERHIDARCHIVO
        public string Cpagerterarchivo { get; set; }  // Mapeo de CPAGERTERARCHIVO
        public string Cpagerdurarchivo { get; set; }  // Mapeo de CPAGERDURARCHIVO
        public string Cpagerusucreacion { get; set; } // Mapeo de CPAGERUSUCREACION
        public DateTime Cpagerfeccreacion { get; set; } // Mapeo de CPAGERFECCREACION
    }
}
