using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class IndCompensacionPruebaAleatoriaDTO
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Empresa { get; set; }
        public string Central { get; set; }
        public string Unidad { get; set; }
        public string ModoOperacion { get; set; }
        public int HrasIndParcial { get; set; }
        public string PrimerArranque { get; set; }
        public string Exitosa { get; set; }
        public string Compensar { get; set; }
        public int Grupocodi { get; set; }
        public int? Equicodi { get; set; }
    }
}
