using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_INSUMO
    /// </summary>
    public class CpaInsumoDTO
    {
        public int Cpainscodi { get; set; }             // Mapeo de CPAINSCODI
        public int Cparcodi { get; set; }               // Mapeo de CPARCODI
        public string Cpainstipinsumo { get; set; }     // Mapeo de CPAINSTIPINSUMO
        public string Cpainstipproceso { get; set; }    // Mapeo de CPAINSTIPPROCESO
        public string Cpainslog { get; set; }           // Mapeo de CPAINSLOG
        public string Cpainsusucreacion { get; set; }   // Mapeo de CPAINSUSUCREACION
        public DateTime Cpainsfeccreacion { get; set; } // Mapeo de CPAINSFECCREACION
        
        //Atributos de consulta
        public string Cpainsdescinsumo { get; set; }
        public string Cpainsfecusuario { get; set; }
    }
}
