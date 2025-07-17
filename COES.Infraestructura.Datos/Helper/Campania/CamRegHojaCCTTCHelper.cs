using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamRegHojaCCTTCHelper : HelperBase
    {
        public CamRegHojaCCTTCHelper(): base(Consultas.CamRegHojaCCTTCSql) { }


        public string SqlGetRegHojaCCTTCProyCodi
        {
            get { return base.GetSqlXml("GetRegHojaCCTTCProyCodi"); }
        }

        public string SqlSaveRegHojaCCTTC
        {
            get { return base.GetSqlXml("SaveRegHojaCCTTC"); }
        }

        public string SqlUpdateRegHojaCCTTC
        {
            get { return base.GetSqlXml("UpdateRegHojaCCTTC"); }
        }

        public string SqlGetLastRegHojaCCTTCId
        {
            get { return base.GetSqlXml("GetLastRegHojaCCTTCId"); }
        }

        public string SqlDeleteRegHojaCCTTCById
        {
            get { return base.GetSqlXml("DeleteRegHojaCCTTCById"); }
        }

        public string SqlGetRegHojaCCTTCById
        {
            get { return base.GetSqlXml("GetRegHojaCCTTCById"); }
        }


    }
}
