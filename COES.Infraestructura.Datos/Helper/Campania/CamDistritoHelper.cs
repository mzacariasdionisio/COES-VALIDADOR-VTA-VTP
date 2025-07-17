using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamDistritoHelper : HelperBase
    {
        public CamDistritoHelper() : base(Consultas.CamDistritoSql) { }

        public string SqlGetListDistByProvDepId
        {

            get { return base.GetSqlXml("GetListDistByProvDepId"); }
        }

        public string SqlDepProvDistByDistritotId
        {

            get { return base.GetSqlXml("GetDepProvDistByDistritotId"); }
        }
    }
}
