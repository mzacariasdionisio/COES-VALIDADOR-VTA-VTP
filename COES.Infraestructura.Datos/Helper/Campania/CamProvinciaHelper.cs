using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamProvinciaHelper : HelperBase
    {
        public CamProvinciaHelper() : base(Consultas.CamProvinciaSql) { }


        public string SqlGetListProvByDepId
        {
            get { return base.GetSqlXml("GetListProvByDepId"); }

        }


    }
}
