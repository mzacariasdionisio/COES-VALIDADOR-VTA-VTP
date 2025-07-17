using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamT3CroFicha1Helper : HelperBase
    {
        public CamT3CroFicha1Helper() : base(Consultas.CamT3CroFicha1Sql) { }

        public string SqlGetCroficha1
        {
            get { return base.GetSqlXml("GetCamT3Croficha1"); }
        }

        public string SqlSaveCroficha1
        {
            get { return base.GetSqlXml("SaveCamT3Croficha1"); }
        }

        public string SqlUpdateCroficha1
        {
            get { return base.GetSqlXml("UpdateCamT3Croficha1"); }
        }

        public string SqlGetLastCroficha1Id
        {
            get { return base.GetSqlXml("GetLastCamT3Croficha1Id"); }
        }

        public string SqlDeleteCroficha1ById
        {
            get { return base.GetSqlXml("DeleteCamT3Croficha1ById"); }
        }

        public string SqlGetCroficha1ById
        {
            get { return base.GetSqlXml("GetCamT3Croficha1ById"); }
        }
    }
}
