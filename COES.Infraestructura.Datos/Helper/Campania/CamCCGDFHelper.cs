using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCCGDFHelper : HelperBase
    {
        public CamCCGDFHelper() : base(Consultas.CamCCGDFSql) { }

        public string SqlGetCamCCGDF
        {
            get { return base.GetSqlXml("GetCamCCGDF"); }
        }

        public string SqlSaveCamCCGDF
        {
            get { return base.GetSqlXml("SaveCamCCGDF"); }
        }

        public string SqlUpdateCamCCGDF
        {
            get { return base.GetSqlXml("UpdateCamCCGDF"); }
        }

        public string SqlGetLastCamCCGDFCodi
        {
            get { return base.GetSqlXml("GetLastCamCCGDFCodi"); }
        }

        public string SqlDeleteCamCCGDFById
        {
            get { return base.GetSqlXml("DeleteCamCCGDFById"); }
        }

        public string SqlGetCamCCGDFById
        {
            get { return base.GetSqlXml("GetCamCCGDFById"); }
        }
    }
}
