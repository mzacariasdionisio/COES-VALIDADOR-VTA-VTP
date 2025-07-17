using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SMA_OFERTA
    /// </summary>
    public class SmaReporteDTO : EntityBase
    {

        public int Grupocodi { get; set; }

        public string Gruponomb { get; set; }

        public DateTime? Repofecha { get; set; }

        public string[] Repointvhini { get; set; } //Rango  30 mins
        public string[] Repointvmini { get; set; } //Rango  30 mins

        public string[] Repointvhfin { get; set; } // Rango 30 mins
        public string[] Repointvmfin { get; set; } // Rango 30 mins

        public string[] Repohoraini { get; set; } // Hora Inicio
        public string[] Repohorafin { get; set; } // Hora Fin

        public int Urscodi { get; set; }

        public decimal Repopotmaxofer { get; set; }

        public decimal Repoprecio { get; set; }

        public int? Reponrounit { get; set; } 


    }
}
