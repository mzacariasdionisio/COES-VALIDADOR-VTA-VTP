using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_PORCENTAJE_MONTO
    /// </summary>
    public class CpaPorcentajeMontoDTO
    {
        public int Cpapmtcodi { get; set; }            // Mapeo de CPAPMTCODI
        public int Cpapcodi { get; set; }              // Mapeo de CPAPCODI
        public int Cparcodi { get; set; }              // Mapeo de CPARCODI
        public int Emprcodi { get; set; }              // Mapeo de EMPRCODI
        public decimal? Cpapmtenemes01 { get; set; }   // Mapeo de CPAPMTENEMES01
        public decimal? Cpapmtenemes02 { get; set; }   // Mapeo de CPAPMTENEMES02
        public decimal? Cpapmtenemes03 { get; set; }   // Mapeo de CPAPMTENEMES03
        public decimal? Cpapmtenemes04 { get; set; }   // Mapeo de CPAPMTENEMES04
        public decimal? Cpapmtenemes05 { get; set; }   // Mapeo de CPAPMTENEMES05
        public decimal? Cpapmtenemes06 { get; set; }   // Mapeo de CPAPMTENEMES06
        public decimal? Cpapmtenemes07 { get; set; }   // Mapeo de CPAPMTENEMES07
        public decimal? Cpapmtenemes08 { get; set; }   // Mapeo de CPAPMTENEMES08
        public decimal? Cpapmtenemes09 { get; set; }   // Mapeo de CPAPMTENEMES09
        public decimal? Cpapmtenemes10 { get; set; }   // Mapeo de CPAPMTENEMES10
        public decimal? Cpapmtenemes11 { get; set; }   // Mapeo de CPAPMTENEMES11
        public decimal? Cpapmtenemes12 { get; set; }   // Mapeo de CPAPMTENEMES12
        public decimal? Cpapmtenetotal { get; set; }    // Mapeo de CPAPMTENTOTAL
        public decimal? Cpapmtpotmes01 { get; set; }   // Mapeo de CPAPMTPOTMES01
        public decimal? Cpapmtpotmes02 { get; set; }   // Mapeo de CPAPMTPOTMES02
        public decimal? Cpapmtpotmes03 { get; set; }   // Mapeo de CPAPMTPOTMES03
        public decimal? Cpapmtpotmes04 { get; set; }   // Mapeo de CPAPMTPOTMES04
        public decimal? Cpapmtpotmes05 { get; set; }   // Mapeo de CPAPMTPOTMES05
        public decimal? Cpapmtpotmes06 { get; set; }   // Mapeo de CPAPMTPOTMES06
        public decimal? Cpapmtpotmes07 { get; set; }   // Mapeo de CPAPMTPOTMES07
        public decimal? Cpapmtpotmes08 { get; set; }   // Mapeo de CPAPMTPOTMES08
        public decimal? Cpapmtpotmes09 { get; set; }   // Mapeo de CPAPMTPOTMES09
        public decimal? Cpapmtpotmes10 { get; set; }   // Mapeo de CPAPMTPOTMES10
        public decimal? Cpapmtpotmes11 { get; set; }   // Mapeo de CPAPMTPOTMES11
        public decimal? Cpapmtpotmes12 { get; set; }   // Mapeo de CPAPMTPOTMES12
        public decimal? Cpapmtpottotal { get; set; }   // Mapeo de CPAPMTPOTTOTAL
        public decimal? Cpapmttrames01 { get; set; }   // Mapeo de CPAPMTTRAMES01
        public decimal? Cpapmttrames02 { get; set; }   // Mapeo de CPAPMTTRAMES02
        public decimal? Cpapmttrames03 { get; set; }   // Mapeo de CPAPMTTRAMES03
        public decimal? Cpapmttrames04 { get; set; }   // Mapeo de CPAPMTTRAMES04
        public decimal? Cpapmttrames05 { get; set; }   // Mapeo de CPAPMTTRAMES05
        public decimal? Cpapmttrames06 { get; set; }   // Mapeo de CPAPMTTRAMES06
        public decimal? Cpapmttrames07 { get; set; }   // Mapeo de CPAPMTTRAMES07
        public decimal? Cpapmttrames08 { get; set; }   // Mapeo de CPAPMTTRAMES08
        public decimal? Cpapmttrames09 { get; set; }   // Mapeo de CPAPMTTRAMES09
        public decimal? Cpapmttrames10 { get; set; }   // Mapeo de CPAPMTTRAMES10
        public decimal? Cpapmttrames11 { get; set; }   // Mapeo de CPAPMTTRAMES11
        public decimal? Cpapmttrames12 { get; set; }   // Mapeo de CPAPMTTRAMES12
        public decimal? Cpapmttratotal { get; set; }   // Mapeo de CPAPMTTRATOTAL
        public string Cpapmtusucreacion { get; set; }  // Mapeo de CPAPMTUSUCREACION
        public DateTime Cpapmtfeccreacion { get; set; } // Mapeo de CPAPMTFECCREACION

        //Additional
        public int Tipoemprcodi { get; set; }
        public string Tipoemprdesc { get; set; }
        public string Emprnomb { get; set; }
        public string Emprruc { get; set; }
    }

}