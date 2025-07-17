using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCuestionarioH2VBHelper: HelperBase
    {
        public CamCuestionarioH2VBHelper() : base(Consultas.CamCuestionarioH2VBSql) { }

        public string SqlGetCuestionarioH2VBCodi
        {
            get { return base.GetSqlXml("GetCuestionarioH2VBByCodi"); }
        }

        public string SqlSaveCuestionarioH2VB
        {
            get { return base.GetSqlXml("SaveCuestionarioH2VB"); }
        }

        public string SqlUpdateCuestionarioH2VB
        {
            get { return base.GetSqlXml("UpdateCuestionarioH2VB"); }
        }

        public string SqlGetLastCuestionarioH2VBId
        {
            get { return base.GetSqlXml("GetLastCuestionarioH2VBId"); }
        }

        public string SqlDeleteCuestionarioH2VBById
        {
            get { return base.GetSqlXml("DeleteCuestionarioH2VBById"); }
        }

        public string SqlGetCuestionarioH2VBById
        {
            get { return base.GetSqlXml("GetCuestionarioH2VBById"); }
        }
    }
}
