using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamPlanTransmisionHelper : HelperBase
    {

        public CamPlanTransmisionHelper() : base(Consultas.CamPlanTransmisionSql) { }

        public string SqlGetPlanTransmision
        {
            get { return base.GetSqlXml("GetPlanTransmision"); }
        }


        public string SqlSavePlanTransmision
        {
            get { return base.GetSqlXml("SavePlanTransmision"); }
        }

        public string SqlUpdatePlanTransmision
        {
            get { return base.GetSqlXml("UpdatePlanTransmision"); }
        }

        public string SqlGetLastPlanTransmisionId
        {
            get { return base.GetSqlXml("GetLastPlanTransmisionId"); }
        }

        public string SqlDeletePlanTransmisionById
        {
            get { return base.GetSqlXml("DeletePlanTransmisionById"); }
        }

        public string SqlGetPlanTransmisionById
        {
            get { return base.GetSqlXml("GetPlanTransmisionById"); }
        }

        public string SqlGetPlanTransmisionByFilters
        {
            get { return base.GetSqlXml("GetPlanTransmisionByFilters"); }
        }

        public string SqlGetPlanTransmisionByEstado
        {
            get { return base.GetSqlXml("GetPlanTransmisionByEstado"); }
        }

        public string SqlGetPlanTransmisionByEstadoEmpresa
        {
            get { return base.GetSqlXml("GetPlanTransmisionByEstadoEmpresa"); }
        }

        public string SqlDesactivatePlanById
        {
            get { return base.GetSqlXml("DesactivatePlanById"); }
        }

        public string SqlActivatePlanById
        {
            get { return base.GetSqlXml("ActivatePlanById"); }
        }

        public string SqlUpdatePlanEstadoById
        {
            get { return base.GetSqlXml("UpdatePlanEstadoById"); }
        }

         public string SqlUpdatePlanEstadoEnviarById
        {
            get { return base.GetSqlXml("UpdatePlanEstadoEnviarById"); }
        }

        public string SqlGetPlanTransmisionByEstadoVigente
        {
            get { return base.GetSqlXml("GetPlanTransmisionByEstadoVigente"); }
        }

        public string SqlGetPlanTransmisionByVigente
        {
            get { return base.GetSqlXml("GetPlanTransmisionByVigente"); }
        }

         public string SqlUpdateProyRegById
        {
            get { return base.GetSqlXml("UpdateProyRegById"); }
        }
    }
}
