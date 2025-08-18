
using Newtonsoft.Json;
using System.Collections.Generic;

namespace COES.Dominio.DTO.ValidacionVTEAVTP
{

    public class VteaValidadorDTO
    { 
        public int Resultado { get; set; }
        public string Mensaje { get; set; }

        [JsonProperty("info_empresa_resumen")]
        public List<InfoEmpresaResumen> InfoEmpresaResumen { get; set; }

        [JsonProperty("info_empresa_detalle")]
        public List<InfoEmpresaDetalle> InfoEmpresaDetalle { get; set; }

        [JsonProperty("info_declaracion_detalle")]
        public List<InfoDeclaracionDetalle> InfoDeclaracionDetalle { get; set; }

        [JsonProperty("info_declaracion_resumen")]
        public List<InfoDeclaracionResumen> InfoDeclaracionResumen { get; set; }

    }

    public class InfoEmpresaResumen {
        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("change")]
        public string Change { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("team_now")]
        public string TeamNow { get; set; }

        [JsonProperty("team_prev")]
        public string TeamPrev { get; set; }

        [JsonProperty("rate")]
        public double Rate { get; set; }
    }

    public class InfoEmpresaDetalle
    {
        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

    }

    public class InfoDeclaracionDetalle
    {
        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("empresa")]
        public string Empresa { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("barra")]
        public string Barra { get; set; }

        [JsonProperty("tipo")]
        public string Tipo { get; set; }

        [JsonProperty("dia")]
        public int Dia { get; set; }

        [JsonProperty("observado")]
        public string Observado { get; set; }

        [JsonProperty("indicador")]
        public int Indicador { get; set; }
    }

    public class InfoDeclaracionResumen
    {

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("empresa")]
        public string Empresa { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("barra")]
        public string Barra { get; set; }
        [JsonProperty("tipo")]
        public string Tipo { get; set; }
        [JsonProperty("observado")]
        public string Observado { get; set; }
    }

}
