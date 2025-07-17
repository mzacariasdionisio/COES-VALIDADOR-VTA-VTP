using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_DATO_SENIAL
    /// </summary>
    public class CoDatoSenialDTO : EntityBase
    {
        public int Codasecodi { get; set; } 
        public int? Canalcodi { get; set; } 
        public string Canalnomb { get; set; }
        public DateTime? Codasefechahora { get; set; } 
        public decimal? Codasevalor { get; set; } 
        public string Codaseusucreacion { get; set; } 
        public DateTime? Codasefeccreacion { get; set; } 
    }
}
