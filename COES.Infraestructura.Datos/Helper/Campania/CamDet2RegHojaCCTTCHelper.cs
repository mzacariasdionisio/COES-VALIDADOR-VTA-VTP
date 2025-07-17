using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamDet2RegHojaCCTTCHelper : HelperBase
    {
        public CamDet2RegHojaCCTTCHelper() : base(Consultas.CamDet2RegHojaCCTTCSql) { }


        public string SqlGetDet2RegHojaCCTTCCentralCodi
        {
            get { return base.GetSqlXml("GetDet2RegHojaCCTTCCentralCodi"); }
        }

        public string SqlSaveDet2RegHojaCCTTC
        {
            get { return base.GetSqlXml("SaveDet2RegHojaCCTTC"); }
        }

        public string SqlUpdateDet2RegHojaCCTTC
        {
            get { return base.GetSqlXml("UpdateDet2RegHojaCCTTC"); }
        }

        public string SqlGetLastDet2RegHojaCCTTCId
        {
            get { return base.GetSqlXml("GetLastDet2RegHojaCCTTCId"); }
        }

        public string SqlDeleteDet2RegHojaCCTTCById
        {
            get { return base.GetSqlXml("DeleteDet2RegHojaCCTTCById"); }
        }

        public string SqlGetDet2RegHojaCCTTCById
        {
            get { return base.GetSqlXml("GetDet2RegHojaCCTTCById"); }
        }


    }
}
