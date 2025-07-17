using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla FT_EXT_RELPLTCORRETAPA
    /// </summary>
    public partial class FtExtRelpltcorretapaDTO : EntityBase
    {
        public int Plantcodi { get; set; }
        public int Ftetcodi { get; set; }
        public int Fcoretcodi { get; set; }
        public int Estenvcodi { get; set; }
        public int Tpcorrcodi { get; set; }
        public int? Ftrpcetipoespecial { get; set; }
        public int? Ftrpcetipoampliacion { get; set; }
    }

    public partial class FtExtRelpltcorretapaDTO
    {
        
        public string Ftetnombre { get; set; }
        public string Tpcorrdescrip { get; set; }
    }
}
