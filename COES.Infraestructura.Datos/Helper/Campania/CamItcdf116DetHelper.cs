using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper
{
    public class CamItcdf116DetHelper : HelperBase
    {
        public CamItcdf116DetHelper() : base(Consultas.CamItcdf116DetSql) { }


        public string SqlGetItcdf116DetCodi
        {
            get { return base.GetSqlXml("GetItcdf116DetCodi"); }
        }

        public string SqlSaveItcdf116Det
        {
            get { return base.GetSqlXml("SaveItcdf116Det"); }
        }

        public string SqlUpdateItcdf116Det
        {
            get { return base.GetSqlXml("UpdateItcdf116Det"); }
        }

        public string SqlGetLastItcdf116DetId
        {
            get { return base.GetSqlXml("GetLastItcdf116DetId"); }
        }

        public string SqlDeleteItcdf116DetById
        {
            get { return base.GetSqlXml("DeleteItcdf116DetById"); }
        }

        public string SqlGetItcdf116DetById
        {
            get { return base.GetSqlXml("GetItcdf116DetById"); }
        }

    }
}
