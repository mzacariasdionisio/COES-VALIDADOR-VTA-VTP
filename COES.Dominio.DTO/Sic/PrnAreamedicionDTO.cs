using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class PrnAreamedicionDTO : EntityBase
    {
        public int Areamedcodi { get; set; }
        public int Areacodi { get; set; }
        public DateTime Areamedfecinicial { get; set; }
        public DateTime Areamedfecfinal { get; set; }
        public string Areamedestado { get; set; }
        public DateTime Areamedfeccreacion { get; set; }
        public string Areamedusucreacion { get; set; }
        public DateTime Areamedfecmodificacion { get; set; }
        public string Areamedusumodificacion { get; set; }

        #region Adicionales

        public string Areanomb { get; set; }
        public string Areaabrev { get; set; }

        #endregion
        
    }
}
