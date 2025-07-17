using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class CrEtapaEventoDTO
    {
        public int CRETAPACODI { get; set; }
        public int CREVENCODI { get; set; }
        public int CRETAPA { get; set; }
        public DateTime CRFECHDESICION { get; set; }
        public string CREVENTODESCRIPCION { get; set; }
        public string CRRESUMENCRITERIO { get; set; }
        public string CRCOMENTARIOS_RESPONSABLES { get; set; }
        public DateTime LASTDATE { get; set; }
        public string LASTUSER { get; set; }
        public int CRCRITERIOCODI { get; set; }
    }
}
