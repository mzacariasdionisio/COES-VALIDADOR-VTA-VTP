using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class FormatoD1ADet5DTO
    {
        public int FormatoD1ADet5Codi { get; set; }
        public int FormatoD1ACodi { get; set; }
        public int DataCatCodi { get; set; }
        public string EnElaboracion { get; set; }
        public string Presentado { get; set; }
        public string EnTramite { get; set; }
        public string Aprobado { get; set; }
        public string Firmado { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }

    }
}
