using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class SolHojaCDTO
    {
        public int Solhojaccodi { get; set; }
        public int Proycodi { get; set; }
        public DateTime? Fecpuestaope { get; set; }
        public List<DetSolHojaCDTO> ListaDetSolHojaCDTO { get; set; }
        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
        public string IndDel { get; set; }
    }
}
