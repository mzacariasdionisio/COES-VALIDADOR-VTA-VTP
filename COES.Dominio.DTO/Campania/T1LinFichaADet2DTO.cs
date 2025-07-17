using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class T1LinFichaADet2DTO
    {
        public int LinFichaADet2Codi { get; set; }
        public int LinFichaACodi { get; set; }
        public int? Tramo { get; set; }
        public decimal? R { get; set; }
        public decimal? X { get; set; }
        public decimal? B { get; set; }
        public decimal? G { get; set; }
        public decimal? R0 { get; set; }
        public decimal? X0 { get; set; }
        public decimal? B0 { get; set; }
        public decimal? G0 { get; set; }
        public string UsuCreacion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string UsuModificacion { get; set; }
        public DateTime FecModificacion { get; set; }
        public string IndDel { get; set; }
    }
}
