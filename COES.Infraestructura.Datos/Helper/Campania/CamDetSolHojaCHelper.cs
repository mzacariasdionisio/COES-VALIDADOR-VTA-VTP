using System;
using System.Collections.Generic;
using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamDetSolHojaCHelper : HelperBase
    {
        public CamDetSolHojaCHelper() : base(Consultas.CamDetSolHojaCSql) { }


        public string SqlGetDetSolHojaCProyCodi
        {
            get { return base.GetSqlXml("GetDetSolHojaCProyCodi"); }
        }

        public string SqlSaveDetSolHojaC
        {
            get { return base.GetSqlXml("SaveDetSolHojaC"); }
        }

        public string SqlUpdateDetSolHojaC
        {
            get { return base.GetSqlXml("UpdateDetSolHojaC"); }
        }

        public string SqlGetLastDetSolHojaCId
        {
            get { return base.GetSqlXml("GetLastDetSolHojaCId"); }
        }

        public string SqlDeleteDetSolHojaCById
        {
            get { return base.GetSqlXml("DeleteDetSolHojaCById"); }
        }

        public string SqlGetDetSolHojaCById
        {
            get { return base.GetSqlXml("GetDetSolHojaCById"); }
        }

    }
}
