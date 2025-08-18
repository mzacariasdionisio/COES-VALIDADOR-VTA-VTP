
using Newtonsoft.Json;
using System.Collections.Generic;

namespace COES.Dominio.DTO.ValidacionVTEAVTP
{
    public class VteaVersionDTO
    {
        public int Resultado { get; set; }
        public string Mensaje { get; set; }

        [JsonProperty("VersionVTEA")]
        public List<TableVersionVteaDTO> Versiones { get; set; }

    }
    public class TableVersionVteaDTO
    {
        public int RecaCodi { get; set; }
        public string RecaNombre { get; set; }
    }
 }
