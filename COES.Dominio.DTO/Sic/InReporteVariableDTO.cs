using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IN_REPORTE_VARIABLE
    /// </summary>
    public class InReporteVariableDTO : EntityBase
    {
        public int Inrevacodi { get; set; }
        public string Inrevavalor { get; set; }
        public string Inrevausucreacion { get; set; }
        public DateTime? Inrevafeccreacion { get; set; }
        public string Inrevausumodificacion { get; set; }
        public DateTime? Inrevafecmodificacion { get; set; }
        public int Inrepcodi { get; set; }
        public string Inrevanota { get; set; }
        public int? Invarcodi { get; set; }
        public string Invardescripcion { get; set; }
        public string Invaridentificador { get; set; }
        public string Invarnota { get; set; }
        public string Invartipodato { get; set; }

    }

    public class VariableValor
    {
        public string Identificador { get; set; }
        public string Valor { get; set; }
    }

}
