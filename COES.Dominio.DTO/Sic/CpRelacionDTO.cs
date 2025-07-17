using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{ 
    public class CpRelacionDTO
    {
        public int Recurcodi1 { get; set; }
        public int Recurcodi2 { get; set; }
        public int Cptrelcodi { get; set; }
        public int Topcodi { get; set; }
        public Decimal Cprelval1 { get; set; }
        public Decimal Cprelval2 { get; set; }
        public string Cprelusucreacion { get; set; }
        public DateTime Cprelfeccreacion { get; set; }
        public string Cprelusumodificacion { get; set; }
        public DateTime Cprelfecmodificacion { get; set; }

        public string Recurnombre { get; set; }
        public int Catcodi1 { get; set; }
        public int Catcodi2 { get; set; }
        public int Recurconsideragams { get; set; }
        public Decimal? Cpreltiempo { get; set; }

    }
}
