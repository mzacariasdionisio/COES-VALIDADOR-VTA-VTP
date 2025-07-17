using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCuestionarioH2VAHelper : HelperBase
    {
        public CamCuestionarioH2VAHelper() : base(Consultas.CamCuestionarioH2VASql) { }

        public string SqlGetCuestionarioH2VACodi
        {
            get { return base.GetSqlXml("GetCuestionarioH2VACodi"); }
        }

        public string SqlSaveCuestionarioH2VA
        {
            get { return base.GetSqlXml("SaveCuestionarioH2VA"); }
        }

        public string SqlUpdateCuestionarioH2VA
        {
            get { return base.GetSqlXml("UpdateCuestionarioH2VA"); }
        }

        public string SqlGetLastCuestionarioH2VAId
        {
            get { return base.GetSqlXml("GetLastCuestionarioH2VAId"); }
        }

        public string SqlDeleteCuestionarioH2VAById
        {
            get { return base.GetSqlXml("DeleteCuestionarioH2VAById"); }
        }

        public string SqlGetCuestionarioH2VAById
        {
            get { return base.GetSqlXml("GetCuestionarioH2VAById"); }
        }
    }
}
