using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamRegHojaCCTTAHelper : HelperBase
    {
        public CamRegHojaCCTTAHelper(): base(Consultas.CamRegHojaCCTTASql) { }


        public string SqlGetRegHojaCCTTAProyCodi
        {
            get { return base.GetSqlXml("GetRegHojaCCTTAProyCodi"); }
        }

        public string SqlSaveRegHojaCCTTA
        {
            get { return base.GetSqlXml("SaveRegHojaCCTTA"); }
        }

        public string SqlUpdateRegHojaCCTTA
        {
            get { return base.GetSqlXml("UpdateRegHojaCCTTA"); }
        }

        public string SqlGetLastRegHojaCCTTAId
        {
            get { return base.GetSqlXml("GetLastRegHojaCCTTAId"); }
        }

        public string SqlDeleteRegHojaCCTTAById
        {
            get { return base.GetSqlXml("DeleteRegHojaCCTTAById"); }
        }

        public string SqlGetRegHojaCCTTAById
        {
            get { return base.GetSqlXml("GetRegHojaCCTTAById"); }
        }


    }
}
