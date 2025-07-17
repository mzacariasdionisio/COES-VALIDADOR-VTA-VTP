using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_RELEQEMPLT
    /// </summary>
    public partial class FtExtReleqempltDTO : EntityBase
    {
        public int Ftreqecodi { get; set; } 
        public int Equicodi { get; set; } 
        public int Emprcodi { get; set; } 
        public int? Ftreqeestado { get; set; } 
        public string Ftreqeusucreacion { get; set; } 
        public DateTime? Ftreqefeccreacion { get; set; } 
        public string Ftreqeusumodificacion { get; set; } 
        public DateTime? Ftreqefecmodificacion { get; set; } 
    }

    public partial class FtExtReleqempltDTO
    {
        
        public string Emprnomb { get; set; }

        public string FtreqeestadoDesc { get; set; }
        public string FechaCreacionDesc { get; set; }
        public string FechaModificacionDesc { get; set; }
    }
}
