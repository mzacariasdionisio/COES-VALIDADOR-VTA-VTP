using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCuestionarioH2VCHelper : HelperBase
    {
        public CamCuestionarioH2VCHelper() : base(Consultas.CamCuestionarioH2VCSql) { }

        public string SqlGetCuestionarioH2VCCodi
        {
            get { return base.GetSqlXml("GetCuestionarioH2VCByCodi"); }
        }

        public string SqlSaveCuestionarioH2VC
        {
            get { return base.GetSqlXml("SaveCuestionarioH2VC"); }
        }

        public string SqlUpdateCuestionarioH2VC
        {
            get { return base.GetSqlXml("UpdateCuestionarioH2VC"); }
        }

        public string SqlGetLastCuestionarioH2VCId
        {
            get { return base.GetSqlXml("GetLastCuestionarioH2VCId"); }
        }

        public string SqlDeleteCuestionarioH2VCById
        {
            get { return base.GetSqlXml("DeleteCuestionarioH2VCById"); }
        }

        public string SqlGetCuestionarioH2VCById
        {
            get { return base.GetSqlXml("GetCuestionarioH2VCById"); }
        }
    }
}
