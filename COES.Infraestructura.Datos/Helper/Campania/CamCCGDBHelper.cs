using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCCGDBHelper : HelperBase
    {
        public CamCCGDBHelper() : base(Consultas.CamCCGDBSql) { }

        public string SqlGetCamCCGDB
        {
            get { return base.GetSqlXml("GetCamCCGDB"); }
        }

        public string SqlSaveCamCCGDB
        {
            get { return base.GetSqlXml("SaveCamCCGDB"); }
        }

        public string SqlUpdateCamCCGDB
        {
            get { return base.GetSqlXml("UpdateCamCCGDB"); }
        }

        public string SqlGetLastCamCCGDBCodi
        {
            get { return base.GetSqlXml("GetLastCamCCGDBCodi"); }
        }

        public string SqlDeleteCamCCGDBById
        {
            get { return base.GetSqlXml("DeleteCamCCGDBById"); }
        }

        public string SqlGetCamCCGDBById
        {
            get { return base.GetSqlXml("GetCamCCGDBById"); }
        }
    }
}
