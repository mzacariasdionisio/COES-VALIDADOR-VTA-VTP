using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Framework.Base.Helper
{
    public class SiTablaAuditableConfHelper: HelperBase
    {
        public SiTablaAuditableConfHelper(): base(Consultas.SiTablaAuditableConfSql)
        { }

        public string SqlListarPorEstadoYTabla 
        {
            get { return GetSqlXml("ListarPorEstadoYTabla"); }
        }
    }
}
