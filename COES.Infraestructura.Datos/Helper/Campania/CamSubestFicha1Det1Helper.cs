using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamSubestFicha1Det1Helper : HelperBase
    {
        public CamSubestFicha1Det1Helper() : base(Consultas.CamSubestFicha1Det1Sql) { }

        public string SqlGetSubestFicha1Det1
        {
            get { return base.GetSqlXml("GetCamSubestFicha1Det1"); }
        }

        public string SqlSaveSubestFicha1Det1
        {
            get { return base.GetSqlXml("SaveCamSubestFicha1Det1"); }
        }

        public string SqlUpdateSubestFicha1Det1
        {
            get { return base.GetSqlXml("UpdateCamSubestFicha1Det1"); }
        }

        public string SqlGetLastSubestFicha1Det1Id
        {
            get { return base.GetSqlXml("GetLastCamSubestFicha1Det1Id"); }
        }

        public string SqlDeleteSubestFicha1Det1ById
        {
            get { return base.GetSqlXml("DeleteCamSubestFicha1Det1ById"); }
        }

        public string SqlGetSubestFicha1Det1ById
        {
            get { return base.GetSqlXml("GetCamSubestFicha1Det1ById"); }
        }
    }
}