using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class FormatoD1BDTO
    {
        public int FormatoD1BCodi { get; set; }
        public int ProyCodi { get; set; }
        public string NombreCarga { get; set; }
        public string Propietario { get; set; }
        public string FechaIngreso { get; set; }
        public string BarraConexion { get; set; }
        public string NivelTension { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }

        public string Empresa { get; set; }
        public List<FormatoD1BDetDTO> ListaFormatoDet1B { get; set; }

    }
}
