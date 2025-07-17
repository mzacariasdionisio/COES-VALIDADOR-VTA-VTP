using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamDepartamentoHelper : HelperBase
    {

        public CamDepartamentoHelper(): base(Consultas.CamDepartamentoSql) { }

        public string SqlGetDepartamentos
        {
            get { return base.GetSqlXml("GetDepartamentos"); }
        }
    }
}
