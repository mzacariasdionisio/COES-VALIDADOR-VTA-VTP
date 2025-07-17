using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Infraestructura.Datos.Helper
{
    public class CamT1LinFichaAHelper : HelperBase
    {
        public CamT1LinFichaAHelper() : base(Consultas.CamT1LinFichaASql) { }

        public string SqlGetLinfichaA
        {
            get { return base.GetSqlXml("GetCamT1LinFichaA"); }
        }

        public string SqlSaveLinfichaA
        {
            get { return base.GetSqlXml("SaveCamT1LinFichaA"); }
        }

        public string SqlUpdateLinfichaA
        {
            get { return base.GetSqlXml("UpdateCamT1LinFichaA"); }
        }

        public string SqlGetLastLinfichaAId
        {
            get { return base.GetSqlXml("GetLastCamT1LinFichaAId"); }
        }

        public string SqlDeleteLinfichaAById
        {
            get { return base.GetSqlXml("DeleteCamT1LinFichaAById"); }
        }

        public string SqlGetLinfichaAById
        {
            get { return base.GetSqlXml("GetCamT1LinFichaAById"); }
        }
    }
}

