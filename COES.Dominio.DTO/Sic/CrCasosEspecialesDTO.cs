using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class CrCasosEspecialesDTO : EntityBase
    {
        public int CRESPECIALCODI { get; set; }
        public string CREDESCRIPCION { get; set; }
        public string CREESTADO { get; set; }
        public DateTime? LASTDATE { get; set; }
        public string LASTUSER { get; set; }


    }
}
