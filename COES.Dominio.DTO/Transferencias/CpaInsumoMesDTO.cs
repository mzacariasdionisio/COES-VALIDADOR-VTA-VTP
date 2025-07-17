using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_INSUMO_MES
    /// </summary>
    public class CpaInsumoMesDTO
    {
        public int Cpainmcodi { get; set; }             // Mapeo de CPAINMCODI
        public int Cpainscodi { get; set; }             // Mapeo de CPAINSCODI
        public int Cparcodi { get; set; }               // Mapeo de CPARCODI
        public int Emprcodi { get; set; }               // Mapeo de EMPRCODI
        public int Equicodi { get; set; }               // Mapeo de EQUICODI
        public string Cpainmtipinsumo { get; set; }     // Mapeo de CPAINMTIPINSUMO
        public string Cpainmtipproceso { get; set; }    // Mapeo de CPAINMTIPPROCESO
        public int Cpainmmes { get; set; }              // Mapeo de CPAINMMES
        public decimal Cpainmtotal { get; set; }        // Mapeo de CPAINMTOTAL
        public string Cpainmusucreacion { get; set; }   // Mapeo de CPAINMUSUCREACION
        public DateTime Cpainmfeccreacion { get; set; } // Mapeo de CPAINMFECCREACION
    }
}
