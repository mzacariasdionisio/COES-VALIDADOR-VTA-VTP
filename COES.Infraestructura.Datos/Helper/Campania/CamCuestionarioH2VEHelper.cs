using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCuestionarioH2VEHelper : HelperBase
    {
        public CamCuestionarioH2VEHelper() : base(Consultas.CamCuestionarioH2VESql) { }

        public string SqlGetCuestionarioH2VECodi
        {
            get { return base.GetSqlXml("GetCuestionarioH2VEByCodi"); }
        }

        public string SqlSaveCuestionarioH2VE
        {
            get { return base.GetSqlXml("SaveCuestionarioH2VE"); }
        }

        public string SqlUpdateCuestionarioH2VE
        {
            get { return base.GetSqlXml("UpdateCuestionarioH2VE"); }
        }

        public string SqlGetLastCuestionarioH2VEId
        {
            get { return base.GetSqlXml("GetLastCuestionarioH2VEId"); }
        }

        public string SqlDeleteCuestionarioH2VEById
        {
            get { return base.GetSqlXml("DeleteCuestionarioH2VEById"); }
        }

        public string SqlGetCuestionarioH2VEById
        {
            get { return base.GetSqlXml("GetCuestionarioH2VEById"); }
        }
    }
}
