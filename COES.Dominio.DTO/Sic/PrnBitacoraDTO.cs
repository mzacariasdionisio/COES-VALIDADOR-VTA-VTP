using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PrnBitacoraDTO
    {
        public int Prnbitcodi { get; set; }

        public int? Emprcodi { get; set; }
        public DateTime? Medifecha { get; set; }

        public string Prnbithorainicio { get; set; }
        public string Prnbithorafin { get; set; }
        public string Prnbittipregistro { get; set; }
        public string Prnbitocurrencia { get; set; }

        public int Grupocodi { get; set; }

        public decimal? Prnbitconstipico { get; set; }
        public decimal? Prnbitconsprevisto { get; set; }
        public decimal? Prnbitptodiferencial { get; set; }

        public int? Ptomedicodi { get; set; }
        public int? Lectcodi { get; set; }
        public int? Tipoemprcodi { get; set; }

        public string Prnbitvalor { get; set; } // Area

        public string Emprnomb { get; set; }
        public string Gruponomb { get; set; }
        public string Ptomedidesc { get; set; }
    }
}
