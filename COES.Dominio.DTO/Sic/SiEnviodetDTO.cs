using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_ENVIODET
    /// </summary>
    public class SiEnviodetDTO : EntityBase
    {
        public int Enviocodi { get; set; }
        public int? Fdatpkcodi { get; set; }
    }
}
