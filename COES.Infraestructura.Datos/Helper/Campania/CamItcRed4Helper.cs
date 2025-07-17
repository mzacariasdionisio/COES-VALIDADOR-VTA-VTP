using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamItcRed4Helper : HelperBase
    {
        public CamItcRed4Helper() : base(Consultas.CamItcRed4Sql) { }

        public string SqlGetCamItcRed4ByProyCodi
        {
            get { return base.GetSqlXml("GetCamItcRed4"); }
        }

        public string SqlSaveCamItcRed4
        {
            get { return base.GetSqlXml("SaveCamItcRed4"); }
        }

        public string SqlUpdateCamItcRed4
        {
            get { return base.GetSqlXml("UpdateCamItcRed4"); }
        }

        public string SqlGetLastCamItcRed4Id
        {
            get { return base.GetSqlXml("GetLastCamItcRed4Id"); }
        }

        public string SqlDeleteCamItcRed4ById
        {
            get { return base.GetSqlXml("DeleteCamItcRed4ById"); }
        }

        public string SqlGetCamItcRed4ById
        {
            get { return base.GetSqlXml("GetCamItcRed4ById"); }
        }
    }
}
