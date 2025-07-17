using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla DOC_DIA_ESP
    /// </summary>
    public class DocDiaEspDTO : EntityBase
    {
        public int Diacodi { get; set; }
        public DateTime? Diafecha { get; set; }
        public int? Diatipo { get; set; }
        public string Diafrec { get; set; }
        public string Diadesc { get; set; }
    }
}
