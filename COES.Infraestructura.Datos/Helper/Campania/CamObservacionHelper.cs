using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamObservacionHelper : HelperBase
    {
        public CamObservacionHelper() : base(Consultas.CamObservacionSql) { }

        public string SqlGetObservacionByProyCodi
        {
            get { return base.GetSqlXml("GetObservacionByProyCodi"); }
        }

        public string SqlSaveObservacion { 
            get { return base.GetSqlXml("SaveObservacion"); }
        }

        public string SqlUpdateObservacion
        {
            get { return base.GetSqlXml("UpdateObservacion"); }
        }

        public string SqlGetLastObservacionId
        {
            get { return base.GetSqlXml("GetLastObservacionId"); }
        }

        public string SqlDeleteObservacionById
        {
            get { return base.GetSqlXml("DeleteObservacionById"); }
        }

        public string SqlGetObservacionById
        {
            get { return base.GetSqlXml("GetObservacionById"); }
        }

         public string SqlEnviarObservacionByProyecto
        {
            get { return base.GetSqlXml("EnviarObservacionByProyecto"); }
        }

        public string SqlGetObservacionByPlanCodi
        {
            get { return base.GetSqlXml("GetObservacionByPlanCodi"); }
        }

          public string SqlUpdateObservacionByProyecto
        {
            get { return base.GetSqlXml("UpdateObservacionByProyecto"); }
        }

    }
}
