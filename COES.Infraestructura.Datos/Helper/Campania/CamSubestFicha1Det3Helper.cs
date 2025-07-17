using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamSubestFicha1Det3Helper : HelperBase
    {
        public CamSubestFicha1Det3Helper() : base(Consultas.CamSubestFicha1Det3Sql) { }

        public string SqlGetSubestFicha1Det3
        {
            get { return base.GetSqlXml("GetCamSubestFicha1Det3"); }
        }

        public string SqlSaveSubestFicha1Det3
        {
            get { return base.GetSqlXml("SaveCamSubestFicha1Det3"); }
        }


        public string SqlGetLastSubestFicha1Det3Id
        {
            get { return base.GetSqlXml("GetLastCamSubestFicha1Det3Id"); }
        }

        public string SqlDeleteSubestFicha1Det3ById
        {
            get { return base.GetSqlXml("DeleteCamSubestFicha1Det3ById"); }
        }

        public string SqlGetSubestFicha1Det3ById
        {
            get { return base.GetSqlXml("GetCamSubestFicha1Det3ById"); }
        }
    }
}