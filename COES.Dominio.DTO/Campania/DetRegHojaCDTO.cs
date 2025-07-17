using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class DetRegHojaCDTO
    {
        public int Detareghccodi { get; set; }
        public int Fichaccodi { get; set; }
        public int Datacatcodi { get; set; }
        public string Anio { get; set; }
        public int Trimestre { get; set; }
        public string Valor { get; set; }
        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
        public string IndDel { get; set; }
        public string ProyCodi { get; set; }
        public string ProyNombre { get; set; }
        public string Empresa { get; set; }
    }
}
