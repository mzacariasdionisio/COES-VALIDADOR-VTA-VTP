using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamItcPrm2Helper : HelperBase
    {
        public CamItcPrm2Helper() : base(Consultas.CamItcPrm2Sql) { }

        public string SqlGetCamItcPrm2ByProyCodi
        {
            get { return base.GetSqlXml("GetCamItcPrm2"); }
        }

        public string SqlSaveCamItcPrm2
        {
            get { return base.GetSqlXml("SaveCamItcPrm2"); }
        }

        public string SqlUpdateCamItcPrm2
        {
            get { return base.GetSqlXml("UpdateCamItcPrm2"); }
        }

        public string SqlGetLastCamItcPrm2Id
        {
            get { return base.GetSqlXml("GetLastCamItcPrm2Id"); }
        }

        public string SqlDeleteCamItcPrm2ById
        {
            get { return base.GetSqlXml("DeleteCamItcPrm2ById"); }
        }

        public string SqlGetCamItcPrm2ById
        {
            get { return base.GetSqlXml("GetCamItcPrm2ById"); }
        }
    }
}
