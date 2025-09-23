
using Newtonsoft.Json;
using System.Collections.Generic;
namespace COES.Dominio.DTO.ValidacionVTEAVTP
{
    public class VteaDetailDTO
    {
        public int Resultado { get; set; }
        public string Mensaje { get; set; }

        [JsonProperty("tmp")]
        public List<Tmp> Tmp { get; set; }
    }

    public class Tmp
    {

        [JsonProperty("codigo")]
        public string codigo { get; set; }

        [JsonProperty("empresa")]
        public string Empresa { get; set; }

        [JsonProperty("cliente")]
        public string Cliente { get; set; }

        [JsonProperty("barra")]
        public string Barra { get; set; }

        [JsonProperty("hora")]
        public string Tipo { get; set; }

        [JsonProperty("cmg")]
        public double? cmg { get; set; }

        [JsonProperty("mwh")]
        public double mwh { get; set; }


    }
}
