using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PrnHorasolDTO
    {
        public int Areacodi { get; set; }
        public DateTime Prnhsfecha { get; set; }
        public DateTime Prnhssalida { get; set; }
        public DateTime Prnhspuesta { get; set; }
        public DateTime Prnhshorassol { get; set; }
        public string Prnhsusucreacion { get; set; }
        public DateTime Prnhsfeccreacion { get; set; }
    }
}
