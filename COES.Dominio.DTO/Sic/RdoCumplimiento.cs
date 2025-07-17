using System;
using System.Collections.Generic;
using COES.Base.Core;
namespace COES.Dominio.DTO.Sic
{
    public class RdoCumplimiento : EntityBase
    {
        public string NombreEmpresa { get; set; }
        public string Hora3 { get; set; }
        public string Hora6 { get; set; }
        public string Hora9 { get; set; }
        public string Hora12 { get; set; }
        public string Hora15 { get; set; }
        public string Hora18 { get; set; }
        public string Hora21 { get; set; }
        public string Hora24 { get; set; }
        public DateTime Rdofechaini { get; set; }
        public DateTime Rdofechafin { get; set; }
        public string sRdofechaini { get; set; }
        public string sRdofechafin { get; set; }
        public int codFormato { get; set; }

        public int Formatcodi { get; set; }
        public int Emprcodi { get; set; }

        public string TipoInforme { get; set; }
        public string Periodo { get; set; }
        public string EtapaInforme { get; set; }

        public string HtmlReporte { get; set; }
        public string TipoFalla { get; set; }

    }
}
