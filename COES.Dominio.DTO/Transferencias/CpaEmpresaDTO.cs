using System;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CPA_EMPRESA
    /// </summary>
    public class CpaEmpresaDTO
    {
        public int Cpaempcodi { get; set; }            // Mapeo de CPAEMPCODI
        public int Cparcodi { get; set; }              // Mapeo de CPARCODI
        public int Emprcodi { get; set; }              // Mapeo de EMPRCODI
        public string Cpaemptipo { get; set; }        // Mapeo de CPAEMPTIPO
        public string Cpaempestado { get; set; }      // Mapeo de CPAEMPESTADO
        public string Cpaempusucreacion { get; set; } // Mapeo de CPAEMPUSUCREACION
        public DateTime Cpaempfeccreacion { get; set; } // Mapeo de CPAEMPFECCREACION
        public string Cpaempusumodificacion { get; set; } // Mapeo de CPAEMPUSUMODIFICACION
        public DateTime? Cpaempfecmodificacion { get; set; } // Mapeo de CPAEMPFECMODIFICACION
        //Agregado para pasar el nombre de la empresa
        public string Emprnomb { get; set; }
        public string Emprnombconcatenado { get; set; }
    }
}
