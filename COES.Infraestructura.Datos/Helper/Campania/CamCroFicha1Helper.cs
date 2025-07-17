using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCroFicha1Helper : HelperBase
    {
        public CamCroFicha1Helper() : base(Consultas.CamCroFicha1Sql) { }


        public string SqlGetCroFicha1ProyCodi
        {
            get { return base.GetSqlXml("GetCroFicha1ProyCodi"); }
        }

        public string SqlSaveCroFicha1
        {
            get { return base.GetSqlXml("SaveCroFicha1"); }
        }

        public string SqlUpdateCroFicha1
        {
            get { return base.GetSqlXml("UpdateCroFicha1"); }
        }

        public string SqlGetLastCroFicha1Id
        {
            get { return base.GetSqlXml("GetLastCroFicha1Id"); }
        }

        public string SqlDeleteCroFicha1ById
        {
            get { return base.GetSqlXml("DeleteCroFicha1ById"); }
        }

        public string SqlGetCroFicha1ById
        {
            get { return base.GetSqlXml("GetCroFicha1ById"); }
        }

    }
}
