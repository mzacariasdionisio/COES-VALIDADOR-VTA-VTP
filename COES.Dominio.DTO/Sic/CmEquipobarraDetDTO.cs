using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_EQUIPOBARRA_DET
    /// </summary>
    public class CmEquipobarraDetDTO : EntityBase
    {
        public int Cmebdecodi { get; set; } 
        public int Cmeqbacodi { get; set; } 
        public int? Barrcodi { get; set; } 
        public string Cmebdeusucreacion { get; set; } 
        public DateTime? Cmebdefeccreacion { get; set; } 
        public string Cmebdeusumodificacion { get; set; } 
        public DateTime? Cmebdefecmodificacion { get; set; } 
        public string Barrnombre { get; set; }
    }
}
