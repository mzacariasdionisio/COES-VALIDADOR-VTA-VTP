using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_CALCULO
    /// </summary>
    public class CpaCalculoDTO
    {
        public int Cpaccodi { get; set; }            // Mapeo de CPACCODI
        public int Cparcodi { get; set; }            // Mapeo de CPARCODI
        public string Cpaclog { get; set; }          // Mapeo de CPACLOG
        public string Cpacusucreacion { get; set; }  // Mapeo de CPACUSUCREACION
        public DateTime Cpacfeccreacion { get; set; } // Mapeo de CPACFECCREACION

        //Additional
        public List<CpaCalculoEmpresaDTO> ListCalculoEmpresa { get; set; }
    }
}
