using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class SolicitudCodigoPotenciaContratadaDTO
    {
        public int? RowSpan { get; set; }
        public int? CodigoAgrupacion { get; set; }
        public int? CoresoCodigo { get; set; }
        public int? CoregeCodigo { get; set; }
        public int? NumeroOrden { get; set; }
        public string TipoAgrupacion { get; set; }
        public decimal? PotenciaContrTotalFija { get; set; }
        public decimal? PotenciaContrHPFija { get; set; }
        public decimal? PotenciaContrHFPFija { get; set; }
        public decimal? PotenciaContrTotalVar { get; set; }
        public decimal? PotenciaContrHPVar { get; set; }
        public decimal? PotenciaContrHFPVar { get; set; }
        public string PotenciaContrObservacion { get; set; }
        public List<int> CoresoCodigoArray { get; set; }
        public List<int> CoregeCodigoArray { get; set; }
        //Es modificado por excel
        public int PotenciaEsExcel { get; set; }
    }
}
