using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamT2SubestFicha1Det2Helper : HelperBase
    {
        public CamT2SubestFicha1Det2Helper() : base(Consultas.CamT2SubestFicha1Det2Sql) { }

        public string SqlGetT2SubestFicha1Det2
        {
            get { return base.GetSqlXml("GetCamT2SubestFicha1Det2"); }
        }

        public string SqlSaveT2SubestFicha1Det2
        {
            get { return base.GetSqlXml("SaveCamT2SubestFicha1Det2"); }
        }

        public string SqlUpdateT2SubestFicha1Det2
        {
            get { return base.GetSqlXml("UpdateCamT2SubestFicha1Det2"); }
        }

        public string SqlGetLastT2SubestFicha1Det2Id
        {
            get { return base.GetSqlXml("GetLastCamT2SubestFicha1Det2Id"); }
        }

        public string SqlDeleteT2SubestFicha1Det2ById
        {
            get { return base.GetSqlXml("DeleteCamT2SubestFicha1Det2ById"); }
        }

        public string SqlGetT2SubestFicha1Det2ById
        {
            get { return base.GetSqlXml("GetCamT2SubestFicha1Det2ById"); }
        }
    }
}