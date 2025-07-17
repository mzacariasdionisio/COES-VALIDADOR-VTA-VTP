using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_PARAMETRO
    /// </summary>
    public class CpaParametroDTO
    {
        public int Cpaprmcodi { get; set; }            // Mapeo de CPAPRMCODI
        public int Cparcodi { get; set; }              // Mapeo de CPARCODI
        public int Cpaprmanio { get; set; }            // Mapeo de CPAPRMANIO
        public int Cpaprmmes { get; set; }             // Mapeo de CPAPRMMES
        public string Cpaprmtipomd { get; set; }       // Mapeo de CPAPRMTIPOMD
        public DateTime Cpaprmfechamd { get; set; }    // Mapeo de CPAPRMFECHAMD
        public decimal Cpaprmcambio { get; set; }      // Mapeo de CPAPRMCAMBIO
        public decimal Cpaprmprecio { get; set; }      // Mapeo de CPAPRMPRECIO
        public string Cpaprmestado { get; set; }       // Mapeo de CPAPRMESTADO
        public int Cpaprmcorrelativo { get; set; }     // Mapeo de CPAPRMCORRELATIVO
        public string Cpaprmusucreacion { get; set; }  // Mapeo de CPAPRMUSUCREACION
        public DateTime Cpaprmfeccreacion { get; set; } // Mapeo de CPAPRMFECCREACION
        public string Cpaprmusumodificacion { get; set; } // Mapeo de CPAPRMUSUMODIFICACION (nullable)
        public DateTime? Cpaprmfecmodificacion { get; set; } // Mapeo de CPAPRMFECMODIFICACION (nullable)
        //CU05
        public string Aniomes { get; set; }
    }
}

