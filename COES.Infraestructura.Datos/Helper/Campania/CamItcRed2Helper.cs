using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamItcRed2Helper : HelperBase
    {
        public CamItcRed2Helper() : base(Consultas.CamItcRed2Sql) { }

        public string SqlGetCamItcRed2ByProyCodi
        {
            get { return base.GetSqlXml("GetCamItcRed2"); }
        }

        public string SqlSaveCamItcRed2
        {
            get { return base.GetSqlXml("SaveCamItcRed2"); }
        }

        public string SqlUpdateCamItcRed2
        {
            get { return base.GetSqlXml("UpdateCamItcRed2"); }
        }

        public string SqlGetLastCamItcRed2Id
        {
            get { return base.GetSqlXml("GetLastCamItcRed2Id"); }
        }

        public string SqlDeleteCamItcRed2ById
        {
            get { return base.GetSqlXml("DeleteCamItcRed2ById"); }
        }

        public string SqlGetCamItcRed2ById
        {
            get { return base.GetSqlXml("GetCamItcRed2ById"); }
        }
    }
}
