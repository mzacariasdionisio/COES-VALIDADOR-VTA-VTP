using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class FormatoD1CDTO
    {
        public int FormatoD1CCodi { get; set; }
        public int ProyCodi { get; set; }
        public string PlanReducir { get; set; }
        public string Alternativa { get; set; }
        public string Otro { get; set; }

        public string Empresa { get; set; }
        public List<FormatoD1CDetDTO> ListaFormatoDe1CDet { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }


    }
}
