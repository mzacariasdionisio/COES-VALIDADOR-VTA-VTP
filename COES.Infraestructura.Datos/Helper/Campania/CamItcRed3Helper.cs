using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamItcRed3Helper : HelperBase
    {
        public CamItcRed3Helper() : base(Consultas.CamItcRed3Sql) { }

        public string SqlGetCamItcRed3ByProyCodi
        {
            get { return base.GetSqlXml("GetCamItcRed3"); }
        }

        public string SqlSaveCamItcRed3
        {
            get { return base.GetSqlXml("SaveCamItcRed3"); }
        }

        public string SqlUpdateCamItcRed3
        {
            get { return base.GetSqlXml("UpdateCamItcRed3"); }
        }

        public string SqlGetLastCamItcRed3Id
        {
            get { return base.GetSqlXml("GetLastCamItcRed3Id"); }
        }

        public string SqlDeleteCamItcRed3ById
        {
            get { return base.GetSqlXml("DeleteCamItcRed3ById"); }
        }

        public string SqlGetCamItcRed3ById
        {
            get { return base.GetSqlXml("GetCamItcRed3ById"); }
        }
    }
}
