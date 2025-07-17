using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CO_PROCESO_ERROR
    /// </summary>
    public partial class CoProcesoErrorDTO : EntityBase
    {
        public int Proerrcodi { get; set; } 
        public int? Prodiacodi { get; set; } 
        public string Proerrmsg { get; set; } 
        public string Proerrtipo { get; set; } 
        public string Proerrusucreacion { get; set; } 
        public DateTime? Proerrfeccreacion { get; set; }        
    }

    public partial class CoProcesoErrorDTO : EntityBase
    {
        public DateTime? Fecha { get; set; }
        public string Tablanomb { get; set; }
    }
}
