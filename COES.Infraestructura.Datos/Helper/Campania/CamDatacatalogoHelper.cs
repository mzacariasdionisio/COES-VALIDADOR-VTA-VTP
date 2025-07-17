using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamDatacatalogoHelper : HelperBase
    {
        public CamDatacatalogoHelper() : base(Consultas.CamDatacatalogoSql){ }

        public string SqlGetParametria
        {
            get { return base.GetSqlXml("GetParametria"); }
        }
        public string SqlGetParamSubestacion
        {
            get { return base.GetSqlXml("GetParamSubestacion"); }
        }
        public string SqlGetParametriaAll
        {
            get { return base.GetSqlXml("GetParametriaAll"); }
        }
    }
}
