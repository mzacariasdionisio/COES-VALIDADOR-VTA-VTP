using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla CAI_SDDP_PARAMSEM
    /// </summary>
    public class CaiSddpParamsemDTO : EntityBase
    {
        public int Sddppscodi { get; set; }
        public int Caiajcodi { get; set; }
        public int Sddppsnumsem { get; set; }
        public DateTime Sddppsdiaini { get; set; }
        public DateTime Sddppsdiafin { get; set; }
        public string Sddppsusucreacion { get; set; }
        public DateTime Sddppsfeccreacion { get; set; } 
    }
}
