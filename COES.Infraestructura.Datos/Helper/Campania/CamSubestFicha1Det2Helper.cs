using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamSubestFicha1Det2Helper : HelperBase
    {
        public CamSubestFicha1Det2Helper() : base(Consultas.CamSubestFicha1Det2Sql) { }

        public string SqlGetSubestFicha1Det2
        {
            get { return base.GetSqlXml("GetCamSubestFicha1Det2"); }
        }

        public string SqlSaveSubestFicha1Det2
        {
            get { return base.GetSqlXml("SaveCamSubestFicha1Det2"); }
        }

        public string SqlUpdateSubestFicha1Det2
        {
            get { return base.GetSqlXml("UpdateCamSubestFicha1Det2"); }
        }

        public string SqlGetLastSubestFicha1Det2Id
        {
            get { return base.GetSqlXml("GetLastCamSubestFicha1Det2Id"); }
        }

        public string SqlDeleteSubestFicha1Det2ById
        {
            get { return base.GetSqlXml("DeleteCamSubestFicha1Det2ById"); }
        }

        public string SqlGetSubestFicha1Det2ById
        {
            get { return base.GetSqlXml("GetCamSubestFicha1Det2ById"); }
        }
    }
}