
using Newtonsoft.Json;
using System.Collections.Generic;

namespace COES.Dominio.DTO.ValidacionVTEAVTP
{
    public class TrnPeriodoDTO
    {
        
        public int Resultado { get; set; }
        public string Mensaje { get; set; }

        [JsonProperty("Mesval")]
        public List<TablePeriodoDTO> Periodos { get; set; }
    }

    public class TablePeriodoDTO {
        public int PeriCodi { get; set; }
        public string PeriNombre { get; set; }
        public int PeriAnio { get; set; }
        public int PeriMes { get; set; }
    }
}
