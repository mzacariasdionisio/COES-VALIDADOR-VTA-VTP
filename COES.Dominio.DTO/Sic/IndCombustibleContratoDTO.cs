using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class IndCombustibleContratoDTO
    {
        public DateTime? CbctrtFechaIniPer { get; set; }
        public decimal? CbctrtTransGas { get; set; }
        public decimal? CbctrtCapaDistGas { get; set; }
        public decimal? CbctrtStockUtil { get; set; }
        public DateTime? CbctrtFechaDia { get; set; }

        #region Campos para consulta
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
        public string Gruponomb { get; set; }
        public string Grupoabrev { get; set; }
        public int Emprcodi { get; set; }
        public int Equipadre { get; set; }
        public int Equicodi { get; set; }
        public int Grupocodi { get; set; }
        public decimal? PotEfectiva { get; set; }
        public decimal? PotAsegurada { get; set; }
        public decimal? PotAsegurada2 { get; set; }
        #endregion
    }
}
