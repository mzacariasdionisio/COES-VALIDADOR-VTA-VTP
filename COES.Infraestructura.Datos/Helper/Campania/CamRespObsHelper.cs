using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamRespObsHelper : HelperBase
    {
        public CamRespObsHelper() : base(Consultas.CamRespObsSql) { }

        public string SqlGetRespuestaObsByObs
        {
            get { return base.GetSqlXml("GetRespuestaObsByObs"); }
        }

        public string SqlSaveRespuestaObs { 
            get { return base.GetSqlXml("SaveRespuestaObs"); }
        }

        public string SqlUpdateRespuestaObs
        {
            get { return base.GetSqlXml("UpdateRespuestaObs"); }
        }

        public string SqlGetLastRespuestaObsId
        {
            get { return base.GetSqlXml("GetLastRespuestaObsId"); }
        }

        public string SqlDeleteRespuestaObsById
        {
            get { return base.GetSqlXml("DeleteRespuestaObsById"); }
        }

        public string SqlGetRespuestaObsById
        {
            get { return base.GetSqlXml("GetRespuestaObsById"); }
        }

    }
}
