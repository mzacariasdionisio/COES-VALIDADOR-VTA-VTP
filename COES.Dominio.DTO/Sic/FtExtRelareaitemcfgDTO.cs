using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_RELAREAITEMCFG
    /// </summary>
    public partial class FtExtRelareaitemcfgDTO : EntityBase
    {
        public int Faremcodi { get; set; } 
        public int Ftitcodi { get; set; } 
        public int Friacodi { get; set; } 
        public string Friaestado { get; set; } 
        public DateTime? Friafeccreacion { get; set; } 
        public string Friausucreacion { get; set; } 
        public DateTime? Friafecmodificacion { get; set; } 
        public string Friausumodificacion { get; set; } 
    }

    public partial class FtExtRelareaitemcfgDTO
    {
        public string NombreArea { get; set; }
        public string NombrePropiedad { get; set; }
        public string Faremnombre { get; set; }
        
    }
}
