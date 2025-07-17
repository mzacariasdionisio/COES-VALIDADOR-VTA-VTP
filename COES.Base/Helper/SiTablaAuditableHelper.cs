using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Framework.Base.Helper
{
    public class SiTablaAuditableHelper: HelperBase
    {
        public SiTablaAuditableHelper(): base(Consultas.SiAuditoriaSql)
        { }
    }
}
