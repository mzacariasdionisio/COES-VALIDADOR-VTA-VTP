using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Campania
{
    public class CamCroFicha1DetHelper : HelperBase
    {
        public CamCroFicha1DetHelper() : base(Consultas.CamCroFicha1DetSql) { }


        public string SqlGetCroFicha1DetProyCodi
        {
            get { return base.GetSqlXml("GetCroFicha1DetProyCodi"); }
        }

        public string SqlSaveCroFicha1Det
        {
            get { return base.GetSqlXml("SaveCroFicha1Det"); }
        }

        public string SqlUpdateCroFicha1Det
        {
            get { return base.GetSqlXml("UpdateCroFicha1Det"); }
        }

        public string SqlGetLastCroFicha1DetId
        {
            get { return base.GetSqlXml("GetLastCroFicha1DetId"); }
        }

        public string SqlDeleteCroFicha1DetById
        {
            get { return base.GetSqlXml("DeleteCroFicha1DetById"); }
        }

        public string SqlGetCroFicha1DetById
        {
            get { return base.GetSqlXml("GetCroFicha1DetById"); }
        }

    }
}
