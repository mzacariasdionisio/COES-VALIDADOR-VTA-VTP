using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla RTU_CONFIGURACION_GRUPO
    /// </summary>
    public class RtuConfiguracionGrupoDTO : EntityBase
    {
        public int Rtugrucodi { get; set; }
        public string Rtugruindreporte { get; set; }
        public int? Rtugruorden { get; set; }
        public int Rtuconcodi { get; set; }
        public string Rtugrutipo { get; set; }
    }
}
