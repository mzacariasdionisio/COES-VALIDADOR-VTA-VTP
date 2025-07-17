using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCuestionarioH2VFHelper : HelperBase
    {
        public CamCuestionarioH2VFHelper() : base(Consultas.CamCuestionarioH2VFSql) { }

        public string SqlGetCuestionarioH2VFCodi
        {
            get { return base.GetSqlXml("GetCuestionarioH2VFByCodi"); }
        }

        public string SqlSaveCuestionarioH2VF
        {
            get { return base.GetSqlXml("SaveCuestionarioH2VF"); }
        }

        public string SqlUpdateCuestionarioH2VF
        {
            get { return base.GetSqlXml("UpdateCuestionarioH2VF"); }
        }

        public string SqlGetLastCuestionarioH2VFId
        {
            get { return base.GetSqlXml("GetLastCuestionarioH2VFId"); }
        }

        public string SqlDeleteCuestionarioH2VFById
        {
            get { return base.GetSqlXml("DeleteCuestionarioH2VFById"); }
        }

        public string SqlGetCuestionarioH2VFById
        {
            get { return base.GetSqlXml("GetCuestionarioH2VFById"); }
        }
    }
}
