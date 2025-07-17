using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper
{
    public class CamItcdf121DetHelper : HelperBase
    {
        public CamItcdf121DetHelper() : base(Consultas.CamItcdf121DetSql) { }


        public string SqlGetItcdf121DetCodi
        {
            get { return base.GetSqlXml("GetItcdf121DetCodi"); }
        }

        public string SqlSaveItcdf121Det
        {
            get { return base.GetSqlXml("SaveItcdf121Det"); }
        }

        public string SqlUpdateItcdf121Det
        {
            get { return base.GetSqlXml("UpdateItcdf121Det"); }
        }

        public string SqlGetLastItcdf121DetId
        {
            get { return base.GetSqlXml("GetLastItcdf121DetId"); }
        }

        public string SqlDeleteItcdf121DetById
        {
            get { return base.GetSqlXml("DeleteItcdf121DetById"); }
        }

        public string SqlGetItcdf121DetById
        {
            get { return base.GetSqlXml("GetItcdf121DetById"); }
        }

    }
}
