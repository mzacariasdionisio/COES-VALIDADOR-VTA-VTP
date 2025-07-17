using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_PORCENTAJE_PORCENTAJE
    /// </summary>
    public class CpaPorcentajePorcentajeDTO
    {
        public int Cpappcodi { get; set; }           // Mapeo de CPAPPCODI
        public int Cpapcodi { get; set; }            // Mapeo de CPAPCODI
        public int Cparcodi { get; set; }            // Mapeo de CPARCODI
        public int Emprcodi { get; set; }            // Mapeo de EMPRCODI
        public decimal? Cpappgentotene { get; set; }  // Mapeo de CPAPPGENTOTENE
        public decimal? Cpappgentotpot { get; set; }  // Mapeo de CPAPPGENTOTPOT
        public decimal? Cpappdistotene { get; set; }  // Mapeo de CPAPPDISTOTENE
        public decimal? Cpappdistotpot { get; set; }  // Mapeo de CPAPPDISTOTPOT
        public decimal? Cpappultotene { get; set; }   // Mapeo de CPAPPULTOTENE
        public decimal? Cpappultotpot { get; set; }   // Mapeo de CPAPPULTOTPOT
        public decimal? Cpapptratot { get; set; }     // Mapeo de CPAPPTRATOT
        public decimal? Cpapptotal { get; set; }     // Mapeo de CPAPPTOTAL
        public decimal Cpappporcentaje { get; set; } // Mapeo de CPAPPPORCENTAJE
        public string Cpappusucreacion { get; set; }  // Mapeo de CPAPPUSUCREACION
        public DateTime Cpappfeccreacion { get; set; } // Mapeo de CPAPPFECCREACION

        //Additional
        public int Tipoemprcodi { get; set; }
        public string Tipoemprdesc { get; set; }
        public string Emprnomb { get; set; }
        public string Emprruc { get; set; }

        public decimal? CpapptotalComparar { get; set; }
        public decimal? DesviacionTotal { get; set; }
        public decimal CpappporcentajeComparar { get; set; }
        public decimal DesviacionPorcentaje { get; set; }
    }

}