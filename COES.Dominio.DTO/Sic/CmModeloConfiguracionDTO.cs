using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_MODELO_CONFIGURACION
    /// </summary>
    public class CmModeloConfiguracionDTO : EntityBase
    {
        public int Modconcodi { get; set; }
        public int Modagrcodi { get; set; }
        public string Modcontipo { get; set; }
        public string Modconsigno { get; set; }

        public int? Topcodi { get; set; }
        public int? Recurcodi { get; set; }
        public int? Ptomedicodi { get; set; }
        public int? Equicodi { get; set; }
    }
}
