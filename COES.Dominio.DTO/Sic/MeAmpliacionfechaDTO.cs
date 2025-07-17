using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_AMPLIACIONFECHA
    /// </summary>
    public partial class MeAmpliacionfechaDTO : EntityBase
    {
        public int Amplicodi { get; set; }
        public DateTime Amplifecha { get; set; } 
        public int Emprcodi { get; set; } 
        public int Formatcodi { get; set; } 
        public DateTime Amplifechaplazo { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; }
    }

    public partial class MeAmpliacionfechaDTO
    {
        public string Emprnomb { get; set; }
        public string Formatnombre { get; set; }

        public int Formatdiafinplazo { get; set; }
        public int Formatdiaplazo { get; set; }

        public string AmplifechaDesc { get; set; }
        public string AmplifechaplazoDesc { get; set; }
        public string LastdateDesc { get; set; }

        public int MediaHora { get; set; }
    }
}
