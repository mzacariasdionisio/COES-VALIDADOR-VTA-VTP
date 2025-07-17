using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class WbNotificacionDTO
    {
        public int NotiCodi { get; set; }
        public string NotiTitulo { get; set; }
        public string NotiDescripcion { get; set; }
        public int NotiStatus { get; set; }
        public DateTime NotiDateTime { get; set; }
        public string NotiUsuCreacion { get; set; }
        public string NotiUsuModificacion { get; set; }
        public DateTime NotiEjecucion { get; set; }
        public string FechaEjecucion { get; set; }
    }

    public class WbTipoNotificacionDTO
    {
        public int Tiponoticodi { get; set; }
        public string Tiponotidesc { get; set; }
    }
}
