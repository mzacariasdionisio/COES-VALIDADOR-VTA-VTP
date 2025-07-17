using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SPO_NUMERAL
    /// </summary>
    public class SpoNumeralDTO : EntityBase
    {
        public int Numecodi { get; set; } 
        public int Areacodi { get; set; } 
        public int? Numediaplazo { get; set; } 
        public string Numeusucreacion { get; set; } 
        public DateTime? Numefeccreacion { get; set; } 
        public int? Numeactivo { get; set; }

        public string Numhisdescripcion { get; set; }
        public string Numhisabrev { get; set; }
        public string Estado { get; set; }
        public int Idestado { get; set; }
        public string FechaEstado { get; set; }
        public string Verrusucreacion { get; set; }
        public int? Vernnro { get; set; }
        public string Vernusumodificacion { get; set; }
        public int Verncodi { get; set; }

    }
}
