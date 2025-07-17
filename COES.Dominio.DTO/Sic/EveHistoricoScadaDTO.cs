using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class EveHistoricoScadaDTO : EntityBase
    {
        public int EVEHISTSCDACODI { get; set; }
        public int EVENCODI { get; set; }
        public int? EVEHISTSCDAZONACODI { get; set; }
        public int? EVEHISTSCDACANALCODI { get; set; }
        public string EVEHISTSCDACODIEQUIPO { get; set; }
        public DateTime EVEHISTSCDAFECHDESCONEXION { get; set; }
        public DateTime LASTDATE { get; set; }
        public string LASTUSER { get; set; }
        public string ZONAABREV { get; set; }
        public string CANALNOMB { get; set; }
        public string strEVEHISTSCDAFECHDESCONEXION { get; set; }
    }
}
