using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_ESCENARIO
    /// </summary>
    public class PrEscenarioDTO : EntityBase
    {
        public DateTime Escefecha { get; set; } 
        public int Escecodi { get; set; } 
        public int Escenum { get; set; } 
        public string Escenomb { get; set; } 
    }
}
