
using Newtonsoft.Json;
using System.Collections.Generic;
namespace COES.Dominio.DTO.ValidacionVTEAVTP
{
    public class VteaDcUnitDTO
    {
        public int Resultado { get; set; }
        public string Mensaje { get; set; }

        [JsonProperty("VTEADCUNIT")]
        public List<VteaDcUnit> VteaDcUnit { get; set; }
    }

    public class VteaDcUnit
    {
        [JsonProperty("CODIGO")]
        public string Codigo { get; set; }

        [JsonProperty("EMPRESA")]
        public string Empresa { get; set; }

        [JsonProperty("CLIENTE")]
        public string Cliente { get; set; }

        [JsonProperty("BARRA")]
        public string Barra { get; set; }

        [JsonProperty("TIPO")]
        public string Tipo { get; set; }

        [JsonProperty("DIA")]
        public string Dia { get; set; }

        [JsonProperty("OBSERVADO")]
        public string Observado { get; set; }
        [JsonProperty("INDICADOR")]
        public string Indicador { get; set; }

    }
}
