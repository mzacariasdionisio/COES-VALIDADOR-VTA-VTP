using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COES.Dominio.DTO.Campania
{
    public class DetRegHojaDDTO
    {
        public String Detreghdcodi { get; set; }
        public String Hojadcodi { get; set; }
        public string Anio { get; set; }
        public string Mes { get; set; }
        public Decimal? Valor { get; set; }
        public string Usucreacion { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Usumodificacion { get; set; }
        public DateTime Fechamodificacion { get; set; }
        public string IndDel { get; set; }

    }
}
