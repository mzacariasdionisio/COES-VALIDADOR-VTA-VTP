using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class Itcdfp012DTO
    {
        public int Itcdfp012Codi { get; set; }
        public int ProyCodi { get; set; }
        public string CodigoSicli { get; set; }
        public string NombreCliente { get; set; }
        public string Subestacion { get; set; }
        public string Barra { get; set; }
        public string CodigoNivelTension { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }

        public string Empresa { get; set; }
        public string AreaDemanda { get; set; }

    }
}
