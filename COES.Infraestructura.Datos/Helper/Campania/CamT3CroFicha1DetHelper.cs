using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamT3CroFicha1DetHelper : HelperBase
    {
        public CamT3CroFicha1DetHelper() : base(Consultas.CamT3CroFicha1DetSql) { }

        public string SqlGetCroficha1Det
        {
            get { return base.GetSqlXml("GetCamT3Croficha1Det"); }
        }

        public string SqlSaveCroficha1Det
        {
            get { return base.GetSqlXml("SaveCamT3Croficha1Det"); }
        }

        public string SqlUpdateCroficha1Det
        {
            get { return base.GetSqlXml("UpdateCamT3Croficha1Det"); }
        }

        public string SqlGetLastCroficha1DetId
        {
            get { return base.GetSqlXml("GetLastCamT3Croficha1DetId"); }
        }

        public string SqlDeleteCroficha1DetById
        {
            get { return base.GetSqlXml("DeleteCamT3Croficha1DetById"); }
        }

        public string SqlGetCroficha1DetById
        {
            get { return base.GetSqlXml("GetCamT3Croficha1DetById"); }
        }
    }
}
