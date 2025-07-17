using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PrnFormularelDTO
    {
        public int Prfrelcodi { get; set; }
        public int Ptomedicodi { get; set; }
        public int Areacodi { get; set; }
        public int Ptomedicodicalc { get; set; }
        public int Prfrelfactor { get; set; }
        public string Prfrelusucreacion { get; set; }
        public DateTime Prfrelfeccreacion { get; set; }
        public string Prfrelusumodificacion { get; set; }
        public DateTime Prfrelfecmodificacion { get; set; }

        //Adicionales
        public int Prnselect { get; set; }//Auxiliar
        public string Ptomedidesc { get; set; }
    }
}
