using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_RESTRIC_CFGDET
    /// </summary>
    public class PrRestricCfgdetDTO : EntityBase
    {
        public int Resdetcodi { get; set; } 
        public string Resdetfuente { get; set; } 
        public int? Resdetcodigo { get; set; } 
        public string Resdetestado { get; set; } 
        public int Rescfgcodi { get; set; } 
    }
}
