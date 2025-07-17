using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCCGDAHelper : HelperBase
    {
        public CamCCGDAHelper() : base(Consultas.CamCCGDASql) { }

        public string SqlGetCCGDA
        {
            get { return base.GetSqlXml("GetCcgda"); }
        }

        public string SqlSaveCCGDA
        {
            get { return base.GetSqlXml("SaveCcgda"); }
        }

        public string SqlUpdateCCGDA
        {
            get { return base.GetSqlXml("UpdateCcgda"); }
        }

        public string SqlGetLastCCGDAById
        {
            get { return base.GetSqlXml("GetLastCCGDAById"); }
        }

        public string SqlDeleteCCGDAById
        {
            get { return base.GetSqlXml("DeleteCcgdaById"); }
        }

        public string SqlGetCcgdaById
        {
            get { return base.GetSqlXml("GetCcgdaById"); }
        }
    }
}
