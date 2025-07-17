using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamITCFE01Helper : HelperBase
    {
        public CamITCFE01Helper(): base(Consultas.camITCFE01Sql) { }


        public string SqlGetRegITCFE01ProyCodi
        {
            get { return base.GetSqlXml("GetRegITCFE01ProyCodi"); }
        }

        public string SqlSaveRegITCFE01
        {
            get { return base.GetSqlXml("SaveRegITCFE01"); }
        }

        public string SqlUpdateRegITCFE01
        {
            get { return base.GetSqlXml("UpdateRegITCFE01"); }
        }

        public string SqlGetLastRegITCFE01Id
        {
            get { return base.GetSqlXml("GetLastRegITCFE01Id"); }
        }

        public string SqlDeleteRegITCFE01ById
        {
            get { return base.GetSqlXml("DeleteRegITCFE01ById"); }
        }

        public string SqlGetRegITCFE01ById
        {
            get { return base.GetSqlXml("GetRegITCFE01ById"); }
        }


    }
}
