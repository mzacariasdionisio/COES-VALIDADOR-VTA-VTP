using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_TIPO_DATO
    /// </summary>
    public class CoTipoDatoDTO : EntityBase
    {
        public string Cotidausumodificacion { get; set; } 
        public DateTime? Cotidafecmodificacion { get; set; } 
        public int Cotidacodi { get; set; } 
        public string Cotidaindicador { get; set; } 
        public string Cotidausucreacion { get; set; } 
        public DateTime? Cotidafeccreacion { get; set; } 
    }
}
