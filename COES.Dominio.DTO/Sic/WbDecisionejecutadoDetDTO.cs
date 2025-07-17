using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla WB_DECISIONEJECUTADO_DET
    /// </summary>
    public class WbDecisionejecutadoDetDTO : EntityBase
    {
        public int Dejdetcodi { get; set; }
        public string Dejdetdescripcion { get; set; }
        public string Dejdetfile { get; set; }
        public int Desejecodi { get; set; }
        public string Dejdettipo { get; set; }
        public string Dejdetestado { get; set; }
        public string Desdetextension { get; set; }
        public string FileName { get; set; }
        public string Icono { get; set; }
    }
}
