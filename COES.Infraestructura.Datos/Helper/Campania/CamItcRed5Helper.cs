using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamItcRed5Helper : HelperBase
    {
        public CamItcRed5Helper() : base(Consultas.CamItcRed5Sql) { }

        public string SqlGetCamItcRed5ByProyCodi
        {
            get { return base.GetSqlXml("GetCamItcRed5"); }
        }

        public string SqlSaveCamItcRed5
        {
            get { return base.GetSqlXml("SaveCamItcRed5"); }
        }

        public string SqlUpdateCamItcRed5
        {
            get { return base.GetSqlXml("UpdateCamItcRed5"); }
        }

        public string SqlGetLastCamItcRed5Id
        {
            get { return base.GetSqlXml("GetLastCamItcRed5Id"); }
        }

        public string SqlDeleteCamItcRed5ById
        {
            get { return base.GetSqlXml("DeleteCamItcRed5ById"); }
        }

        public string SqlGetCamItcRed5ById
        {
            get { return base.GetSqlXml("GetCamItcRed5ById"); }
        }
    }
}
