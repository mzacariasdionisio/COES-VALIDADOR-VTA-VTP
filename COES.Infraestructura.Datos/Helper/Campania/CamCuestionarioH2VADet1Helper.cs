using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCuestionarioH2VADet1Helper : HelperBase
    {
        public CamCuestionarioH2VADet1Helper() : base(Consultas.CamCuestionarioH2VADet1Sql) { }

        public string SqlGetCuestionarioH2VADet1Codi
        {
            get { return base.GetSqlXml("GetCuestionarioH2VADet1ByCodi"); }
        }

        public string SqlSaveCuestionarioH2VADet1
        {
            get { return base.GetSqlXml("SaveCuestionarioH2VADet1"); }
        }

        public string SqlUpdateCuestionarioH2VADet1
        {
            get { return base.GetSqlXml("UpdateCuestionarioH2VADet1"); }
        }

        public string SqlGetLastCuestionarioH2VADet1Id
        {
            get { return base.GetSqlXml("GetLastCuestionarioH2VADet1Id"); }
        }

        public string SqlDeleteCuestionarioH2VADet1ById
        {
            get { return base.GetSqlXml("DeleteCuestionarioH2VADet1ById"); }
        }

        public string SqlGetCuestionarioH2VADet1ById
        {
            get { return base.GetSqlXml("GetCuestionarioH2VADet1ById"); }
        }
    }
}
