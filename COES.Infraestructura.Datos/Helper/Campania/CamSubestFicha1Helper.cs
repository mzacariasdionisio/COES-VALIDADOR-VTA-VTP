using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamSubestFicha1Helper : HelperBase
    {
        public CamSubestFicha1Helper() : base(Consultas.CamSubestFicha1Sql) { }

        public string SqlGetSubestFicha1
        {
            get { return base.GetSqlXml("GetCamSubestFicha1"); }
        }

        public string SqlSaveSubestFicha1
        {
            get { return base.GetSqlXml("SaveCamSubestFicha1"); }
        }

        public string SqlUpdateSubestFicha1
        {
            get { return base.GetSqlXml("UpdateCamSubestFicha1"); }
        }

        public string SqlGetLastSubestFicha1Id
        {
            get { return base.GetSqlXml("GetLastCamSubestFicha1Id"); }
        }

        public string SqlDeleteSubestFicha1ById
        {
            get { return base.GetSqlXml("DeleteCamSubestFicha1ById"); }
        }

        public string SqlGetSubestFicha1ById
        {
            get { return base.GetSqlXml("GetCamSubestFicha1ById"); }
        }
    }

}