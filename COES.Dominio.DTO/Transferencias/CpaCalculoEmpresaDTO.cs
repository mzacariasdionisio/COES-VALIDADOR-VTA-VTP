using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_CALCULO_EMPRESA
    /// </summary>
    public class CpaCalculoEmpresaDTO
    {
        public int Cpacecodi { get; set; }            // Mapeo de CPACECODI
        public int Cpaccodi { get; set; }             // Mapeo de CPACCODI
        public int Cparcodi { get; set; }            // Mapeo de CPARCODI
        public int Emprcodi { get; set; }             // Mapeo de EMPRCODI
        public string Cpacetipo { get; set; }         // Mapeo de CPACETIPO
        public int Cpacemes { get; set; }             //Mapeo de CPACEMES
        public decimal? Cpacetotenemwh { get; set; }   // Mapeo de CPACETOTENEMWH
        public decimal? Cpacetotenesoles { get; set; } // Mapeo de CPACETOTENESOLES
        public decimal? Cpacetotpotmwh { get; set; }  // Mapeo de CPACETOTPOTMWH
        public decimal? Cpacetotpotsoles { get; set; } // Mapeo de CPACETOTPOTSOLES
        public string Cpaceusucreacion { get; set; }  // Mapeo de CPACEUSUCREACION
        public DateTime Cpacefeccreacion { get; set; } // Mapeo de CPACEFECCREACION

        //Additional
        public string Emprnomb { get; set; }
        public List<CpaCalculoCentralDTO> ListCalculoCentral { get; set; }
    }
}
