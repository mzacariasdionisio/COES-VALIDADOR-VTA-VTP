using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla VCR_UNIDADEXONERADA
    /// </summary>
    public class VcrUnidadexoneradaDTO : EntityBase
    {
        public int Vcruexcodi { get; set; } 
        public int Vcrecacodi { get; set; }
        public int Emprcodi { get; set; }
        public int Equicodicen { get; set; } 
        public int Equicodiuni { get; set; } 
        public string Vcruexonerar { get; set; } 
        public string Vcruexobservacion { get; set; } 
        public string Vcruexusucreacion { get; set; } 
        public DateTime Vcruexfeccreacion { get; set; }

        //Atributos de consultas
        public string Emprnomb { get; set; }
        public string Equinombcen { get; set; }
        public string Equinombuni { get; set; } 
    }
}
