using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamItcPrm1Helper : HelperBase
    {
        public CamItcPrm1Helper() : base(Consultas.CamItcPrm1Sql) { }

        public string SqlGetCamItcPrm1Codi
        {
            get { return base.GetSqlXml("GetCamItcPrm1Codi"); }
        }

        public string SqlSaveCamItcPrm1
        {
            get { return base.GetSqlXml("SaveCamItcPrm1"); }
        }

        public string SqlUpdateCamItcPrm1
        {
            get { return base.GetSqlXml("UpdateCamItcPrm1"); }
        }

        public string SqlGetLastCamItcPrm1Id
        {
            get { return base.GetSqlXml("GetLastCamItcPrm1Id"); }
        }

        public string SqlDeleteCamItcPrm1ById
        {
            get { return base.GetSqlXml("DeleteCamItcPrm1ById"); }
        }

        public string SqlGetCamItcPrm1ById
        {
            get { return base.GetSqlXml("GetCamItcPrm1ById"); }
        }
    }
}
