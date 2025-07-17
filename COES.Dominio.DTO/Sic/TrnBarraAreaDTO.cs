using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla TRN_BARRA_AREA
    /// </summary>
    public class TrnBarraAreaDTO : EntityBase
    {
        public int Bararecodi { get; set; } 
        public int? Barrcodi { get; set; } 
        public string Bararearea { get; set; } 
        public string Barareejecutiva { get; set; } 
        public string Barareestado { get; set; } 
        public string Barareusucreacion { get; set; } 
        public DateTime? Bararefeccreacion { get; set; } 
        public string Barareusumodificacion { get; set; } 
        public DateTime? Bararefecmodificacion { get; set; } 
        public string sFechaCreacion { get; set; }
        public string Barrnombre { get; set; }
    }
}
