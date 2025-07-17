using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamItcRed1Helper : HelperBase
    {
        public CamItcRed1Helper() : base(Consultas.CamItcRed1Sql) { }

        public string SqlGetCamItcRed1ByProyCodi
        {
            get { return base.GetSqlXml("GetCamItcRed1"); }
        }

        public string SqlSaveCamItcRed1
        {
            get { return base.GetSqlXml("SaveCamItcRed1"); }
        }

        public string SqlUpdateCamItcRed1
        {
            get { return base.GetSqlXml("UpdateCamItcRed1"); }
        }

        public string SqlGetLastCamItcRed1Id
        {
            get { return base.GetSqlXml("GetLastCamItcRed1Id"); }
        }

        public string SqlDeleteCamItcRed1ById
        {
            get { return base.GetSqlXml("DeleteCamItcRed1ById"); }
        }

        public string SqlGetCamItcRed1ById
        {
            get { return base.GetSqlXml("GetCamItcRed1ById"); }
        }
    }
}
