using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamDet1RegHojaCCTTCHelper : HelperBase
    {
        public CamDet1RegHojaCCTTCHelper(): base(Consultas.CamDet1RegHojaCCTTCSql) { }


        public string SqlGetDet1RegHojaCCTTCCodi
        {
            get { return base.GetSqlXml("GetDet1RegHojaCCTTCCentralCodi"); }
        }

        public string SqlSaveDet1RegHojaCCTTC
        {
            get { return base.GetSqlXml("SaveDet1RegHojaCCTTC"); }
        }

        public string SqlUpdateDet1RegHojaCCTTC
        {
            get { return base.GetSqlXml("UpdateDet1RegHojaCCTTC"); }
        }

        public string SqlGetLastDet1RegHojaCCTTCId
        {
            get { return base.GetSqlXml("GetLastDet1RegHojaCCTTCId"); }
        }

        public string SqlDeleteDet1RegHojaCCTTCById
        {
            get { return base.GetSqlXml("DeleteDet1RegHojaCCTTCById"); }
        }

        public string SqlGetDet1RegHojaCCTTCById
        {
            get { return base.GetSqlXml("GetDet1RegHojaCCTTCById"); }
        }


    }
}
