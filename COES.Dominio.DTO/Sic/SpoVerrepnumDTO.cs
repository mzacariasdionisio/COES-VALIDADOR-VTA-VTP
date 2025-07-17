using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SPO_VERREPNUM
    /// </summary>
    public class SpoVerrepnumDTO : EntityBase
    {
        public int? Verrcodi { get; set; } 
        public int? Verncodi { get; set; } 
        public int Verrncodi { get; set; }

        public string Numhisabrev { get; set; }
        public int Vernnro { get; set; }
        public int Numecodi { get; set; }
    }
}
