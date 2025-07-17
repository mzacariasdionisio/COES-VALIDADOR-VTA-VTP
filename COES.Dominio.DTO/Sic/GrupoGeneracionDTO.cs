using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class GrupoGeneracionDTO
    {
       
        public int Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public int? Grupopadre { get; set; }
        public DateTime? Equifechiniopcom { get; set; }
        public DateTime? Equifechfinopcom { get; set; } 
    }

}
