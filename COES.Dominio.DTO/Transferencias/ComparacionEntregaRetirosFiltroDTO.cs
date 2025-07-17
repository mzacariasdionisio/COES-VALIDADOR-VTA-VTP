using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class ComparacionEntregaRetirosFiltroDTO
    {
        public int TipoInfoCodi { get; set; }
        public int PeriCodi1 { get; set; }
        public int VersionCodi1 { get; set; }
        public int PeriCodi2 { get; set; }
        public int VersionCodi2 { get; set; }
        public List<int> DiaArray { get; set; }
        public int? EmprCodi{ get; set; }
        public int? CliCodi{ get; set; }
        public int? BarrCodi{ get; set; }
        public string TipoEntregaCodi{ get; set; }

        public List<string> CodigoRetiroArray { get; set; }

    }

    public class ComparacionEntregaRetirosGraficoDTO
    { 
        public string Codigo { get; set; }
 
        public List<CostoMarginalGraficoValoresDTO> entregaRetiros { get; set; }

    }
}
