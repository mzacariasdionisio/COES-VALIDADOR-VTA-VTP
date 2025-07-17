using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamT1LinFichaADet1Helper : HelperBase
    {
        public CamT1LinFichaADet1Helper() : base(Consultas.CamT1LinFichaADet1Sql) { }

        public string SqlGetLinfichaaDet1
        {
            get { return base.GetSqlXml("GetCamT1LinFichaADet1"); }
        }

        public string SqlSaveLinfichaaDet1
        {
            get { return base.GetSqlXml("SaveCamT1LinFichaADet1"); }
        }

        public string SqlUpdateLinfichaaDet1
        {
            get { return base.GetSqlXml("UpdateCamT1LinFichaADet1"); }
        }

        public string SqlGetLastLinfichaaDet1Id
        {
            get { return base.GetSqlXml("GetLastCamT1LinFichaADet1Id"); }
        }

        public string SqlDeleteLinfichaaDet1ById
        {
            get { return base.GetSqlXml("DeleteCamT1LinFichaADet1ById"); }
        }

        public string SqlGetLinfichaaDet1ById
        {
            get { return base.GetSqlXml("GetCamT1LinFichaADet1ById"); }
        }
    }
}
