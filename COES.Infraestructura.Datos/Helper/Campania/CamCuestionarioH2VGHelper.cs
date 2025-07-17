using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCuestionarioH2VGHelper : HelperBase
    {
        public CamCuestionarioH2VGHelper() : base(Consultas.CamCuestionarioH2VGSql) { }

        public string SqlGetCuestionarioH2VGByCodi
        {
            get { return base.GetSqlXml("GetCuestionarioH2VG"); }
        }

        public string SqlSaveCuestionarioH2VG
        {
            get { return base.GetSqlXml("SaveCuestionarioH2VG"); }
        }

        public string SqlUpdateCuestionarioH2VG
        {
            get { return base.GetSqlXml("UpdateCuestionarioH2VG"); }
        }

        public string SqlGetLastCuestionarioH2VGId
        {
            get { return base.GetSqlXml("GetLastCuestionarioH2VGCodi"); }
        }

        public string SqlDeleteCuestionarioH2VGById
        {
            get { return base.GetSqlXml("DeleteCuestionarioH2VGById"); }
        }

        public string SqlGetCuestionarioH2VGById
        {
            get { return base.GetSqlXml("GetCuestionarioH2VGById"); }
        }
    }
}