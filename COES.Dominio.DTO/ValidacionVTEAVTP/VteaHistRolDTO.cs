
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace COES.Dominio.DTO.ValidacionVTEAVTP
{
    public class VteaHistRolDTO
    {
        public int Resultado { get; set; }
        public string Mensaje { get; set; }

        [JsonProperty("VTEARolHist")]
        public List<VteaRolHist> VteaRolHist { get; set; }
    }

    public class VteaRolHist
    {
        [JsonProperty("PERICODI")]
        public string Pericodi { get; set; }

        [JsonProperty("TIME")]
        public DateTime Time { get; set; }

        [JsonProperty("EMPRESA")]
        public string Empresa { get; set; }

        [JsonProperty("ROL")]
        public string Rol { get; set; }

    }

}
