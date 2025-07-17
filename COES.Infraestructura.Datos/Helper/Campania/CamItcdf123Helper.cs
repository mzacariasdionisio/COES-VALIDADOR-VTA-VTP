using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper
{
    public class CamItcdf123Helper : HelperBase
    {
        public CamItcdf123Helper() : base(Consultas.CamItcdf123Sql) { }


        public string SqlGetItcdf123Codi
        {
            get { return base.GetSqlXml("GetItcdf123Codi"); }
        }

        public string SqlSaveItcdf123
        {
            get { return base.GetSqlXml("SaveItcdf123"); }
        }

        public string SqlUpdateItcdf123
        {
            get { return base.GetSqlXml("UpdateItcdf123"); }
        }

        public string SqlGetLastItcdf123Id
        {
            get { return base.GetSqlXml("GetLastItcdf123Id"); }
        }

        public string SqlDeleteItcdf123ById
        {
            get { return base.GetSqlXml("DeleteItcdf123ById"); }
        }

        public string SqlGetItcdf123ById
        {
            get { return base.GetSqlXml("GetItcdf123ById"); }
        }

    }
}
