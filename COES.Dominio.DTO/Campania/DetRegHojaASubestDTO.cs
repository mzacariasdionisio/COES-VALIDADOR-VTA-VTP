using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class DetRegHojaASubestDTO
    {
        public int Detsubesthacodi { get; set; }
        public int Centralcodi { get; set; }
        public string Tipo { get; set; }
        public int Datacatcodi { get; set; }
        public int numData { get; set; }
        public string Valor { get; set; }
        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
        public string IndDel { get; set; }
    }
}
