using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class ConsolidadoEnvioDTO
    {
        public int Emprcodi { get; set; }
        public string Empresa { get; set; }
        public string Central { get; set; }
        public short IdCentral { get; set; }
        public string GrupSSAA { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public short TipoGeneracion { get; set; }
        public int Equicodi { get; set; }
        public int Fenergcodi { get; set; }
        public string Fenergnomb { get; set; }
        public decimal Porcentaje { get; set; }
        public string Fenercolor { get; set; }
        public string Tgenerrer { get; set; }
        public int IdGrupo { get; set; }
        public decimal EnergInductiva { get; set; }
        public decimal EnergCapacitiva { get; set; }
        public decimal PotActiva { get; set; }
        public decimal TotalInductiva { get; set; }
        public decimal TotalCapacitiva { get; set; }
        public List<decimal> EInductiva { get; set; }
        public List<decimal> ECapacitiva { get; set; }
        public List<decimal> EActiva { get; set; }
        public string Tipogenerrer { get; set; }

    }
}
