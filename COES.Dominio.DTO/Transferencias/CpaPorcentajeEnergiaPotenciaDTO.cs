using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_PORCENTAJE_ENERGIAPOTENCIA
    /// </summary>
    public class CpaPorcentajeEnergiaPotenciaDTO
    {
        public int Cpapepcodi { get; set; }                // Mapeo de CPAPEPCODI
        public int Cpapcodi { get; set; }                  // Mapeo de CPAPCODI
        public int Cparcodi { get; set; }                  // Mapeo de CPARCODI
        public int Emprcodi { get; set; }                  // Mapeo de EMPRCODI
        public decimal? Cpapepenemes01 { get; set; }       // Mapeo de CPAPEPENEMES01
        public decimal? Cpapepenemes02 { get; set; }       // Mapeo de CPAPEPENEMES02
        public decimal? Cpapepenemes03 { get; set; }       // Mapeo de CPAPEPENEMES03
        public decimal? Cpapepenemes04 { get; set; }       // Mapeo de CPAPEPENEMES04
        public decimal? Cpapepenemes05 { get; set; }       // Mapeo de CPAPEPENEMES05
        public decimal? Cpapepenemes06 { get; set; }       // Mapeo de CPAPEPENEMES06
        public decimal? Cpapepenemes07 { get; set; }       // Mapeo de CPAPEPENEMES07
        public decimal? Cpapepenemes08 { get; set; }       // Mapeo de CPAPEPENEMES08
        public decimal? Cpapepenemes09 { get; set; }       // Mapeo de CPAPEPENEMES09
        public decimal? Cpapepenemes10 { get; set; }       // Mapeo de CPAPEPENEMES10
        public decimal? Cpapepenemes11 { get; set; }       // Mapeo de CPAPEPENEMES11
        public decimal? Cpapepenemes12 { get; set; }       // Mapeo de CPAPEPENEMES12
        public decimal? Cpapepenetotal { get; set; }        // Mapeo de CPAPEPENETOTAL
        public decimal? Cpapeppotmes01 { get; set; }       // Mapeo de CPAPEPPOTMES01
        public decimal? Cpapeppotmes02 { get; set; }       // Mapeo de CPAPEPPOTMES02
        public decimal? Cpapeppotmes03 { get; set; }       // Mapeo de CPAPEPPOTMES03
        public decimal? Cpapeppotmes04 { get; set; }       // Mapeo de CPAPEPPOTMES04
        public decimal? Cpapeppotmes05 { get; set; }       // Mapeo de CPAPEPPOTMES05
        public decimal? Cpapeppotmes06 { get; set; }       // Mapeo de CPAPEPPOTMES06
        public decimal? Cpapeppotmes07 { get; set; }       // Mapeo de CPAPEPPOTMES07
        public decimal? Cpapeppotmes08 { get; set; }       // Mapeo de CPAPEPPOTMES08
        public decimal? Cpapeppotmes09 { get; set; }       // Mapeo de CPAPEPPOTMES09
        public decimal? Cpapeppotmes10 { get; set; }       // Mapeo de CPAPEPPOTMES10
        public decimal? Cpapeppotmes11 { get; set; }       // Mapeo de CPAPEPPOTMES11
        public decimal? Cpapeppotmes12 { get; set; }       // Mapeo de CPAPEPPOTMES12
        public decimal? Cpapeppottotal { get; set; }       // Mapeo de CPAPEPPOTTOTAL
        public string Cpapepusucreacion { get; set; }      // Mapeo de CPAPEPUSUCREACION
        public DateTime Cpapepfeccreacion { get; set; }    // Mapeo de CPAPEPFECCREACION

        //Additional
        public int Tipoemprcodi { get; set; }
		public string Tipoemprdesc { get; set; }
		public string Emprnomb { get; set; }
        public string Emprruc { get; set; }
    }


}