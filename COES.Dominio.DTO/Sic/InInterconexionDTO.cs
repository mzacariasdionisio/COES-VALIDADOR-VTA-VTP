using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_INTERCONEXION
    /// </summary>
    public class InInterconexionDTO : EntityBase
    {
        public int Intercodi { get; set; } 
        public string Interdecrip { get; set; } 
        public int? Ptomedicodi { get; set; } 
        public string Interenlace { get; set; } 
        public int? Ptomedicodisecun { get; set; }
        public string Internomlinea { get; set; }
    }
}
