using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla EVE_SUBCAUSAEVENTO
    /// </summary>
    public class EveSubcausaeventoDTO : EntityBase
    {
        public string Subcausadesc { get; set; } 
        public string Subcausaabrev { get; set; } 
        public int Subcausacodi { get; set; } 
        public int? Causaevencodi { get; set; }

        #region PR5
        public string SubcausaCmg { get; set; }
        public string Subcausacolor { get; set; }
        public Int16 Orden { get; set; }
        #endregion

        public string ConfiguradoRelacionAreaDesc { get; set; }
    }
}

