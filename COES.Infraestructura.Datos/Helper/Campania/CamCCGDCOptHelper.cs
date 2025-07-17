using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCCGDCOptHelper : HelperBase
    {
        public CamCCGDCOptHelper() : base(Consultas.CamCCGDCOptSql) { }

        public string SqlGetCamCCGDC
        {
            get { return base.GetSqlXml("GetCamCCGDC"); }
        }

        public string SqlSaveCamCCGDC
        {
            get { return base.GetSqlXml("SaveCamCCGDC"); }
        }

        public string SqlUpdateCamCCGDC
        {
            get { return base.GetSqlXml("UpdateCamCCGDC"); }
        }

        public string SqlGetLastCamCCGDCCodi
        {
            get { return base.GetSqlXml("GetLastCamCCGDCCodi"); }
        }

        public string SqlDeleteCamCCGDCById
        {
            get { return base.GetSqlXml("DeleteCamCCGDCById"); }
        }

        public string SqlGetCamCCGDCById
        {
            get { return base.GetSqlXml("GetCamCCGDCById"); }
        }
    }
}
