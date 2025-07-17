using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class DpoEstadoVersionDTO
    {
        public int Dpoevscodi { get; set; }
        public int Vergrpcodi { get; set; }
        public int Dpoevspadre { get; set; }
        public string Dpoevsrepvegt { get; set; }
        public string Dpoevsrepindt { get; set; }
        public string Dpoevsrepdesp { get; set; }
        public string Dpoevsusucreacion { get; set; }
        public DateTime Dpoevsfeccreacion { get; set; }
        public string Dpoevsusumodificacion { get; set; }
        public DateTime Dpoevsfecmodificacion { get; set; }
    }
}
