using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCCGDDHelper : HelperBase
    {
        public CamCCGDDHelper() : base(Consultas.CamCCGDDSql) { }

        public string SqlGetCamCCGDD
        {
            get { return base.GetSqlXml("GetCamCCGDD"); }
        }

        public string SqlSaveCamCCGDD
        {
            get { return base.GetSqlXml("SaveCamCCGDD"); }
        }

        public string SqlUpdateCamCCGDD
        {
            get { return base.GetSqlXml("UpdateCamCCGDD"); }
        }

        public string SqlGetLastCamCCGDDCodi
        {
            get { return base.GetSqlXml("GetLastCamCCGDDCodi"); }
        }

        public string SqlDeleteCamCCGDDById
        {
            get { return base.GetSqlXml("DeleteCamCCGDDById"); }
        }

        public string SqlGetCamCCGDDById
        {
            get { return base.GetSqlXml("GetCamCCGDDById"); }
        }
    }
}
