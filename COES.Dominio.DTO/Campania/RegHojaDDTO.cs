using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class RegHojaDDTO
    {
        public String Hojadcodi { get; set; }
        public int Proycodi { get; set; }
        public string Cuenca { get; set; }
        public string Caudal { get; set; }
        public string Estado { get; set; }

        public string Empresa { get; set; }

        public string Proyecto { get; set; }
        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
        public string IndDel { get; set; }
        public List<DetRegHojaDDTO> ListDetRegHojaD { get; set; }


    }
}
