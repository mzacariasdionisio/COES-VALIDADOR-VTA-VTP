using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class DpoConfiguracionDTO
    {
        public int Dpocngcodi { get; set; }
        public int Vergrpcodi { get; set; }
        public int Dpocngdias { get; set; }
        public int Dpocngpromedio { get; set; }
        public decimal Dpocngtendencia { get; set; }
        public decimal Dpocnggaussiano { get; set; }
        public string Dpocngusucreacion { get; set; }
        public DateTime Dpocngfeccreacion { get; set; }
        public string Dpocngusumodificacion { get; set; }
        public DateTime Dpocngfecmodificacion { get; set; }

        #region Assetec - PronosticoDemandaVeg 20240826
        public decimal Dpocngumbral { get; set; }
        public int Dpocngvmg { get; set; }
        public decimal Dpocngstd { get; set; }
        public string Dpocngfechora { get; set; }
        #endregion
    }
}
