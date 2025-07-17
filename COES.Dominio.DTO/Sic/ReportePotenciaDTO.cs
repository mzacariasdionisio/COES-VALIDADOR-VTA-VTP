using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class ReportePotenciaEmpresa
    {
        public string EmpresaGrupo { get; set; }
        public List<ReportePotenciaTipoGeneracion> DatosTipogeneracion { get; set; }
        public List<decimal?> TotalPotenciaEmpresa { get; set; }
    }

    public class ReportePotenciaTipoGeneracion
    {
        public string Empresa { get; set; }
        public string TipoGeneracionGrupo { get; set; }
        public List<ReportePotenciaCentralDTO> DatosCental { get; set; }
        public List<decimal?> PotenciaEmpresaTipo { get; set; }
        public string TipoGeneracionTotal { get; set; }
    }

    public class ReportePotenciaCentralDTO
    {
        public string Empresa { get; set; }
        public string GrupoCentral { get; set; }
        public string TipoGeneracion { get; set; }
        public List<decimal?> TotalPe { get; set; }
        public string NombreTipoGeneracion { get; set; }
        public List<DetalleReportePotenciaDTO> DatosPotencia { get; set; }
    }

    public class DetalleReportePotenciaDTO
    {
        public string Empresa { get; set; }
        public string Central { get; set; }
        public string GeneradorModo { get; set; }
        public List<decimal?> Potencia { get; set; }
        public string Combustible { get; set; }
    }
}
