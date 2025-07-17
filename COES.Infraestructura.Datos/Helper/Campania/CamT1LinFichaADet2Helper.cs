using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamT1LinFichaADet2Helper : HelperBase
    {
        public CamT1LinFichaADet2Helper() : base(Consultas.CamT1LinFichaADet2Sql) { }

        public string SqlGetLinfichaaDet2
        {
            get { return base.GetSqlXml("GetCamT1LinFichaADet2"); }
        }

        public string SqlSaveLinfichaaDet2
        {
            get { return base.GetSqlXml("SaveCamT1LinFichaADet2"); }
        }

        public string SqlUpdateLinfichaaDet2
        {
            get { return base.GetSqlXml("UpdateCamT1LinFichaADet2"); }
        }

        public string SqlGetLastLinfichaaDet2Id
        {
            get { return base.GetSqlXml("GetLastCamT1LinFichaADet2Id"); }
        }

        public string SqlDeleteLinfichaaDet2ById
        {
            get { return base.GetSqlXml("DeleteCamT1LinFichaADet2ById"); }
        }

        public string SqlGetLinfichaaDet2ById
        {
            get { return base.GetSqlXml("GetCamT1LinFichaADet2ById"); }
        }
    }
}
