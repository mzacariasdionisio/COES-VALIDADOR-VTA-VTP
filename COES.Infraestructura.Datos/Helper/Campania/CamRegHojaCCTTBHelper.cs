using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamRegHojaCCTTBHelper : HelperBase
    {
        public CamRegHojaCCTTBHelper() : base(Consultas.CamRegHojaCCTTBSql) { }


        public string SqlGetRegHojaCCTTBProyCodi
        {
            get { return base.GetSqlXml("GetRegHojaCCTTBProyCodi"); }
        }

        public string SqlSaveRegHojaCCTTB
        {
            get { return base.GetSqlXml("SaveRegHojaCCTTB"); }
        }

        public string SqlUpdateRegHojaCCTTB
        {
            get { return base.GetSqlXml("UpdateRegHojaCCTTB"); }
        }

        public string SqlGetLastRegHojaCCTTBId
        {
            get { return base.GetSqlXml("GetLastRegHojaCCTTBId"); }
        }

        public string SqlDeleteRegHojaCCTTBById
        {
            get { return base.GetSqlXml("DeleteRegHojaCCTTBById"); }
        }

        public string SqlGetRegHojaCCTTBById
        {
            get { return base.GetSqlXml("GetRegHojaCCTTBById"); }
        }


    }

}
