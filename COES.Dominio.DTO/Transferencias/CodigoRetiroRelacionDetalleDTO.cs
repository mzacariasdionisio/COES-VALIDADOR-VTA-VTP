using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class CodigoRetiroRelacionDetalleDTO
    {
        public int Rerldtcodi { get; set; }
        public int Retrelcodi { get; set; }
        public int? Genemprcodivtea { get; set; }
        public int? Cliemprcodivtea { get; set; }
        public int? Tipconcodivtea { get; set; }
        public int? Tipusuvtea { get; set; }
        public int? Barrcodivtea { get; set; }
        public int? Coresocodvtea { get; set; }
        public int? Genemprcodivtp { get; set; }
        public int? Cliemprcodivtp { get; set; }
        public int? Tipconcodivtp { get; set; }
        public int? Tipusuvtp { get; set; }
        public int? Barrcodivtp { get; set; }
        public int? Coresocodvtp { get; set; }
        public int? Coregecodi { get; set; }
        public decimal Retrelvari { get; set; }
        public string Rerldtestado { get; set; }
        public string Rerldtusucreacion { get; set; }
        public DateTime Rerldtfeccreacion { get; set; }

        public string Genemprnombvtea { get; set; }
        public string Cliemprnombvtea { get; set; }
        public string Tipocontratovtea { get; set; }
        public string Tipousuariovtea { get; set; }
        public string Barrnombvtea { get; set; }
        public string Codigovtea { get; set; }
        public string Genemprnombvtp { get; set; }
        public string Cliemprnombvtp { get; set; }
        public string Tipocontratovtp { get; set; }
        public string Tipousuariovtp { get; set; }
        public string Barrnombvtp { get; set; }
        public string Codigovtp { get; set; }
        public decimal PorcentajeVarCalculado { get; set; }


    }
}
