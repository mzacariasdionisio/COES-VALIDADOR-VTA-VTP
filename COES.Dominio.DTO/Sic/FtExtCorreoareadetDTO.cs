using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_CORREOAREADET
    /// </summary>
    public partial class FtExtCorreoareadetDTO : EntityBase
    {
        public int Faremcodi { get; set; } 
        public int Faremdcodi { get; set; } 
        public string Faremdemail { get; set; } 
        public string Faremduserlogin { get; set; } 
        public string Faremdestado { get; set; } 
    }

    public partial class FtExtCorreoareadetDTO 
    {
        public string Faremnombre { get; set; }
    }
    
}
