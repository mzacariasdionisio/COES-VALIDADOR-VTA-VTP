using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla ME_HEADCOLUMN
    /// </summary>
    public class MeHeadcolumnDTO : EntityBase
    {
        public int Formatcodi { get; set; } 
        public int Hojacodi { get; set; } 
        public int Emprcodi { get; set; } 
        public int? Headpos { get; set; } 
        public int? Headlen { get; set; } 
        public int? Headrow { get; set; } 
        public string Headnombre { get; set; } 
    }
}
