using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class SecuenciaEventoEmpresaDTO : EntityBase
    {
        public string CodigoSECC { get; set; }
        public string Descripcion { get; set; }
    }
}
