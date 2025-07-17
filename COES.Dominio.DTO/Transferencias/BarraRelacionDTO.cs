using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class BarraRelacionDTO
    {
        public int BareCodi { get; set; }
        public int IndexLista { get; set; }
        public int BareBarrCodiTra { get; set; }
        public int BareBarrCodiSum { get; set; }
        public string BareUsuarioRegistro { get; set; }
        public DateTime BareFechaRegistro { get; set; }
        public string BareEstado { get; set; }
        public string BarrNombSum { get; set; }
        public string BarrTension { get; set; }
        public int NroRegistros { get; set; }
        public string IdGenerado { get; set; }
    }
}
