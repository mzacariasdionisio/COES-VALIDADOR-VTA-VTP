using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCCGDEHelper : HelperBase
    {
        public CamCCGDEHelper() : base(Consultas.CamCCGDESql) { }

        public string SqlGetCamCCGDE
        {
            get { return base.GetSqlXml("GetCamCCGDE"); }
        }

        public string SqlSaveCamCCGDE
        {
            get { return base.GetSqlXml("SaveCamCCGDE"); }
        }

        public string SqlUpdateCamCCGDE
        {
            get { return base.GetSqlXml("UpdateCamCCGDE"); }
        }

        public string SqlGetLastCamCCGDECodi
        {
            get { return base.GetSqlXml("GetLastCamCCGDECodi"); }
        }

        public string SqlDeleteCamCCGDEById
        {
            get { return base.GetSqlXml("DeleteCamCCGDEById"); }
        }

        public string SqlGetCamCCGDEById
        {
            get { return base.GetSqlXml("GetCamCCGDEById"); }
        }
    }
}
