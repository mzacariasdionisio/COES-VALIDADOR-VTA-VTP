using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_CALCULO_CENTRAL
    /// </summary>
    public class CpaCalculoCentralDTO
    {
        public int Cpacccodi { get; set; }            // Mapeo de CPACCCODI
        public int Cpacecodi { get; set; }            // Mapeo de CPACECODI
        public int Cpaccodi { get; set; }            // Mapeo de CPACCODI
        public int Cparcodi { get; set; }            // Mapeo de CPARCODI
        public int Equicodi { get; set; }            // Mapeo de EQUICODI
        public int Barrcodi { get; set; }           // Mapeo de BARRCODI
        public decimal? Cpacctotenemwh { get; set; }  // Mapeo de CPACCTOTENEMWH
        public decimal? Cpacctotenesoles { get; set; }// Mapeo de CPACCTOTENESOLES
        public decimal? Cpacctotpotmwh { get; set; } // Mapeo de CPACCTOTPOTMWH
        public decimal? Cpacctotpotsoles { get; set; }// Mapeo de CPACCTOTPOTSOLES
        public string Cpaccusucreacion { get; set; } // Mapeo de CPACCUSUCREACION
        public DateTime Cpaccfeccreacion { get; set; }// Mapeo de CPACCFECCREACION

        //Additionals
        public string Emprnomb { get; set; }
        public string Equinomb { get; set; }
        public string BarrBarraTransferencia { get; set; }
    }
}
