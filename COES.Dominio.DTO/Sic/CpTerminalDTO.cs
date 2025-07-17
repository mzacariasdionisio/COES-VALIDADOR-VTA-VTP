using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class CpTerminalDTO
    {
        public int Termcodi { get; set; }
        public String Termnombre { get; set; }
        public short Ttermcodi { get; set; }
        public int Recurcodi { get; set; }
        public int Nodocodi { get; set; }
        public int Topcodi { get; set; }
        public string Lastuser { get; set; }
        public DateTime Lastdate { get; set; }

        public string Recurnombre { get; set; }
        public int Recurcodisicoes { get; set; }
       // public int TerminalIDSicoes { get; set; }
    }
}
