using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class TrnPeriodoCnaDTO
    {
        public int Percnacodi { get; set; }
        public string Dd { get; set; }
        public string Dl { get; set; }
        public string Dm { get; set; }
        public string Dmm { get; set; }
        public string Dj { get; set; }
        public string Dvr { get; set; }
        public string Ds { get; set; }
        public string Semperiodo { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
    }
}
