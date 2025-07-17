using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_SECCION
    /// </summary>
    public partial class InSeccionDTO : EntityBase
    {
        public int Inseccodi { get; set; }
        public string Insecnombre { get; set; }
        public string Inseccontenido { get; set; }
        public string Insecusumodificacion { get; set; }
        public DateTime Insecfeccracion { get; set; }
        public string Insecusucreacion { get; set; }
        public DateTime Insecfeccreacion { get; set; }
        public int Inrepcodi { get; set; }

        public string sInseccontenido { get; set; }
    }
}
