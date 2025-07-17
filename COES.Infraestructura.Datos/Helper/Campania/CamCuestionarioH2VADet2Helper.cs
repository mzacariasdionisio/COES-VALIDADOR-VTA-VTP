using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCuestionarioH2VADet2Helper : HelperBase
    {
        public CamCuestionarioH2VADet2Helper() : base(Consultas.CamCuestionarioH2VADet2Sql) { }

        public string SqlGetCuestionarioH2VADet2Codi
        {
            get { return base.GetSqlXml("GetCuestionarioH2VADet2ByCodi"); }
        }

        public string SqlSaveCuestionarioH2VADet2
        {
            get { return base.GetSqlXml("SaveCuestionarioH2VADet2"); }
        }

        public string SqlUpdateCuestionarioH2VADet2
        {
            get { return base.GetSqlXml("UpdateCuestionarioH2VADet2"); }
        }

        public string SqlGetLastCuestionarioH2VADet2Id
        {
            get { return base.GetSqlXml("GetLastCuestionarioH2VADet2Id"); }
        }

        public string SqlDeleteCuestionarioH2VADet2ById
        {
            get { return base.GetSqlXml("DeleteCuestionarioH2VADet2ById"); }
        }

        public string SqlGetCuestionarioH2VADet2ById
        {
            get { return base.GetSqlXml("GetCuestionarioH2VADet2ById"); }
        }
    }
}
